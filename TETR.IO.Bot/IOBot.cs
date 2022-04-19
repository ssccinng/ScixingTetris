using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Carter.Request;

using ScixingTetrisCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace TETR.IO.Bot
{

    public class MoveResult
    {
        public bool hold { get; set; }
        public List<string> moves { get; set; } = new List<string>();
        public int[][] expected_cells { get; set; }

    }

    public class BotSetting
    {
        public int NextCnt { get; set; } = 6;
        public int Level { get; set; } = 8;
        public int BPM { get; set; } = 200;
        //public bool AutoLevel { get; set; } = true;

    }
    public class IOBot : CarterModule
    {
        static Queue<MinoType> _nextQueue = new();
        static TetrisGameBoard _IOBoard = new(ShowHeight: 22);
        static int _garbage = 0;
        static object _lockQueue = new();
        static object _lockBoard = new();
        static BotSetting _botSetting = new BotSetting();

        static DateTime _startTime;
        static int _nowIdx = 0;
        public IOBot()
        {

            


            Post("/newGame", async (req, res) =>
            {
                Init();
                var nextQueue = await req.Bind<string[]>();
                AddNext(nextQueue);
                _IOBoard.GameStart();
                Console.WriteLine("新的一局开始了！");
                Console.WriteLine($"序列为！{string.Join(",", nextQueue[..20])}...");

            });

            Post("/endGame", async (req, res) =>
            {
                Console.WriteLine("游戏结束！");
            });

            Post("/newPieces", async (req, res) =>
            {
                var nextQueue = await req.Bind<string[]>();
                AddNext(nextQueue);
                Console.WriteLine("添加新序列");
            });
            Get("/GetPieces", async (req, res) =>
            {
                await res.WriteAsJsonAsync(_IOBoard.NextQueue);
            });
            Post("/nextMove", async (req, res) =>
            {
                var garbage = await req.Bind<int>();
                // 重置
                await res.WriteAsJsonAsync(GetMove(garbage));
                //await res.WriteAsync(System.Text.Encoding.ASCII.GetString(path));
                //await res.WriteAsync(path);
            });

            Post("/resetBoard", async (req, res) =>
            {
                var result = await req.Bind<JsonDocument>();
                resetBoard(result);
                //if (!result.ValidationResult.IsValid)
                //{
                //    res.StatusCode = 422;
                //    await res.Negotiate(result.ValidationResult.GetFormattedErrors());
                //    return;
                //}

                //resetBorad(board);
                Console.WriteLine("重置地图");
            });
            Post("/pendingGarbage", async (req, res) =>
            {
                var nextQueue = req.BindAndValidate<string[]>();
                Console.WriteLine("重置游戏红条");
            });
        }

        private void Init()
        {
            _IOBoard = new();
            _garbage = 0;
            //_startTime = DateTime.Now;
            _nowIdx = 0;
            try
            {
                _botSetting = JsonSerializer.Deserialize<BotSetting>(System.IO.File.ReadAllText("TetrSetting.json"));
            }
            catch (Exception)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                System.IO.File.WriteAllTextAsync("TetrSetting.json", JsonSerializer.Serialize(_botSetting, options));
            }
            // 读取配置文件
        }

        private void AddNext(string[] nextQueue)
        {
            lock (_lockQueue)
            {

                for (int i = 0; i < nextQueue.Length; ++i)
                {
                    _IOBoard.NextQueue.Enqueue(
                        nextQueue[i] switch
                        {
                            "I" => TetrisMino.I,
                            "O" => TetrisMino.O,
                            "S" => TetrisMino.S,
                            "Z" => TetrisMino.Z,
                            "T" => TetrisMino.T,
                            "J" => TetrisMino.J,
                            "L" => TetrisMino.L,
                            _ => null,
                        }
                        );
                }
            }
        }

        private void resetBoard(JsonDocument board)
        {

            if (board is null) return;

            _garbage = board.RootElement.GetProperty("garbage").GetInt32();
            JsonElement data = board.RootElement.GetProperty("board");
            lock (_lockBoard)
            {
                for (int i = 0; i < data.GetArrayLength(); ++i)
                {
                    for (int j = 0; j < data[i].GetArrayLength(); ++j)
                    {
                        _IOBoard.Field[39 - i][j] = (byte)(data[i][j].GetString() == null ? 0 : 1);
                    }
                }
            }

        }


        private MoveResult GetMove(int garbage)
        {
            int[] field1 = new int[24];
            int[] field2 = new int[17];
            for (int i = 17; i < 40; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (_IOBoard.Field[39 - i][j] == 1) field1[i - 17] |= (1 << j);
                }

            }

            var path = ZZZTOJCore.TetrisAI(field2, field1, 10, 22, _IOBoard.B2B,
                    _IOBoard.Combo, _IOBoard.NextQueue.Take(_botSetting.NextCnt + 1).Select(s => s.Name[0]).ToArray(), (_IOBoard.HoldMino == null ? ' ' : _IOBoard.HoldMino.Name[0]),
                    true, _IOBoard.TetrisMinoStatus.TetrisMino.Name[0], 3, 18 - _IOBoard.TetrisMinoStatus.Position.X, 0, true, false, garbage, new[] { 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, -1 }, _botSetting.NextCnt, _botSetting.Level, 0);
            string resultpath = Marshal.PtrToStringAnsi(path);
            //Console.WriteLine(resultpath);
            MoveResult moveResult = new MoveResult();
            foreach (char move in resultpath)
            {
                switch (move)
                {
                    case 'z':
                    case 'Z':
                        _IOBoard.LeftRotation();
                        moveResult.moves.Add("Ccw");
                        break;
                    case 'c':
                    case 'C':
                        _IOBoard.RightRotation();
                        moveResult.moves.Add("Cw");

                        break;
                    case 'l':
                        _IOBoard.MoveLeft();
                        moveResult.moves.Add("Left");
                        break;
                    case 'L':
                        for (int j = 0; j < 10; ++j)
                        {
                            _IOBoard.MoveLeft();
                            moveResult.moves.Add("Left");
                        }
                        break;
                    case 'r':
                        _IOBoard.MoveRight();
                        moveResult.moves.Add("Right");
                        break;
                    case 'R':
                        for (int _ = 0; _ < 10; ++_)
                        {
                            _IOBoard.MoveRight();
                            moveResult.moves.Add("Right");
                        }
                        break;
                    case 'd':
                    case 'D':

                        _IOBoard.SonicDrop();
                        moveResult.moves.Add("SonicDrop");
                        break;
                    case 'v':
                        _IOBoard.OnHold();
                        moveResult.hold = true;
                        break;
                    case 'V':
                        _IOBoard.SonicDrop();
                        moveResult.expected_cells = new int[4][];
                        var list = _IOBoard.TetrisMinoStatus.GetMinoFieldListInBoard();
                        for (int i = 0; i < 4; ++i)
                        {
                            moveResult.expected_cells[i] = new int[2];
                            moveResult.expected_cells[i][1] = list[i].X;
                            moveResult.expected_cells[i][0] = list[i].Y;
                        }
                        _IOBoard.HardDrop();
                        break;
                    default:
                        break;
                }
                if (move == 'V') break;
            }
            if (_nowIdx == 0)
            {
                _startTime = DateTime.Now;
            }
            _nowIdx++;
            TimeSpan hopeTime = TimeSpan.FromSeconds(60.0 / _botSetting.BPM * _nowIdx);
            //Console.WriteLine("延时" + (_startTime + hopeTime - DateTime.Now).TotalMilliseconds + "毫秒");
            if (DateTime.Now < _startTime + hopeTime)
            {
                Task.Delay( _startTime + hopeTime - DateTime.Now).Wait();
            }
            
            //_IOBoard.PrintBoard(WithMino:false);
            return moveResult;
        }
    }

}
