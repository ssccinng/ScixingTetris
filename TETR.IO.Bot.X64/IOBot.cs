using Carter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carter.ModelBinding;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TETR.IO.Bot.X64
{
    public class MoveResult
    {
        public bool hold { get; set; }
        public List<string> moves { get; set; } = new List<string>();
        public int[][] expected_cells { get; set; }

    }
    public enum WeightType
    {
        None,
        Default,
        Fast,
    }
    public class BotSetting
    {
        public int NextCnt { get; set; } = 6;
        public int BPM { get; set; } = 200;
        [JsonInclude]
        public CCWeights CCWeights;
        [JsonInclude]
        public CCOptions CCOptions;
        public WeightType WeightType { get; set; } = WeightType.None;
        //public 
        //public bool AutoLevel { get; set; } = true;

    }
    public class IOBot : CarterModule
    {
        static Queue<CCPiece> _nextQueue = new();
        static IntPtr _bot = IntPtr.Zero;
        static int _pieceCnt = 0;
        static object _lockQueue = new();
        static object _lockBot = new();
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
                Console.WriteLine("新的一局开始了！");
                Console.WriteLine($"序列为！{string.Join(",", nextQueue[..20])}...");

            });

            Post("/endGame", async (req, res) =>
            {
                if (_bot == IntPtr.Zero) return;
                lock (_lockBot)
                {
                    ColdClearCore.cc_destroy_async(_bot);
                    _bot = IntPtr.Zero;

                }

                Console.WriteLine("游戏结束！");
            });

            Post("/newPieces", async (req, res) =>
            {
                if (_bot == IntPtr.Zero) return;
                var nextQueue = await req.Bind<string[]>();
                AddNext(nextQueue);
                Console.WriteLine("添加新序列");
            });
            Get("/GetPieces", async (req, res) =>
            {
                //await res.WriteAsJsonAsync(_IOBoard.NextQueue);
            });
            Post("/nextMove", async (req, res) =>
            {
                //Console.WriteLine("请求收到了");
                if (_bot == IntPtr.Zero) return;
                var garbage = await req.Bind<int>();
                // 重置
                await res.WriteAsJsonAsync(GetMove(garbage));

            });

            Post("/resetBoard", async (req, res) =>
            {
                if (_bot == IntPtr.Zero) return;
                var result = await req.Bind<JsonDocument>();
                resetBoard(result);


                //resetBorad(board);
                Console.WriteLine("重置地图");
            });
            Post("/pendingGarbage", async (req, res) =>
            {
                if (_bot == IntPtr.Zero) return;
                var nextQueue = req.BindAndValidate<string[]>();
                Console.WriteLine("重置游戏红条");
            });
        }

        private void Init()
        {
            _nextQueue = new();

            Console.WriteLine(1);


            _pieceCnt = 0;
            _nowIdx = 0;
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                };
                _botSetting = JsonSerializer.Deserialize<BotSetting>(System.IO.File.ReadAllText("TetrSetting.json"), options);
            }
            catch (Exception)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                ColdClearCore.cc_default_options(ref _botSetting.CCOptions);
                ColdClearCore.cc_default_weights(ref _botSetting.CCWeights);
                System.IO.File.WriteAllTextAsync("TetrSetting.json", JsonSerializer.Serialize(_botSetting, options));
            }

            if (_botSetting.WeightType == WeightType.Default)
            {

                ColdClearCore.cc_default_weights(ref _botSetting.CCWeights);
            }
            else if (_botSetting.WeightType == WeightType.Fast)
            {
                ColdClearCore.cc_fast_weights(ref _botSetting.CCWeights);
            }
            //Console.WriteLine("数据读取完成");
            _bot = ColdClearCore.cc_launch_async(_botSetting.CCOptions, _botSetting.CCWeights, new CCBook(), null, 0);
            //Console.WriteLine($"初始化完成: {_bot}");
            // 读取配置文件
        }

        private void AddNext(string[] nextQueue)
        {

            foreach (string mino in nextQueue)
            {
                _nextQueue.Enqueue((CCPiece)Enum.Parse(typeof(CCPiece), "CC_" + mino));
            }


            //Console.WriteLine("Next加入队列...");
            UpdateNext();

        }

        private void UpdateNext()
        {
            lock (_lockBot)
            {
                //Console.WriteLine(_nextQueue.Count);
                while (_pieceCnt < _botSetting.NextCnt && _nextQueue.Count > 0)
                {
                    ColdClearCore.cc_add_next_piece_async(_bot, _nextQueue.Dequeue());
                    _pieceCnt++;
                }
                //Console.WriteLine("Next更新完成...");
            }
        }

        private void resetBoard(JsonDocument board)
        {

            if (board is null) return;
            lock (_lockBot)
            {
                JsonElement data = board.RootElement.GetProperty("board");
                byte[] ff = new byte[400];
                for (int i = 39; i >= 0; --i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        ff[(39 - i) * 10 + j] = (byte)(data[i][j].GetString() == null ? 0 : 1);
                    }
                }


                ColdClearCore.cc_reset_async(_bot, ff, 0, 0);
            }

        }


        private MoveResult GetMove(int garbage)
        {
            MoveResult moveResult = new MoveResult();

            lock (_lockBot)
            {
                //Console.WriteLine("开始请求调用");
                ColdClearCore.cc_request_next_move(_bot, (uint)garbage);
                //Console.WriteLine("请求调用成功");
                CCMove cCMove = new CCMove();
                CCBotPollStatus aa;
                while ((aa = ColdClearCore.cc_poll_next_move(_bot, ref cCMove, null, IntPtr.Zero)) != CCBotPollStatus.CC_MOVE_PROVIDED)
                {
                    if (aa == CCBotPollStatus.CC_BOT_DEAD) return null;
                    //Console.WriteLine(aa);
                    //Console.WriteLine(_pieceCnt);
                    Task.Delay(50).Wait();
                }
                //Console.WriteLine(aa);
                //Console.WriteLine("开始写入操作");
                for (int i = 0; i < cCMove.movement_count; ++i)
                {
                    switch (cCMove.movements[i])
                    {
                        case CCMovement.CC_LEFT:
                            moveResult.moves.Add("Left");
                            break;
                        case CCMovement.CC_RIGHT:
                            moveResult.moves.Add("Right");
                            break;
                        case CCMovement.CC_CW:
                            moveResult.moves.Add("Cw");
                            break;
                        case CCMovement.CC_CCW:
                            moveResult.moves.Add("Ccw");
                            break;
                        case CCMovement.CC_DROP:
                            moveResult.moves.Add("SonicDrop");
                            break;
                        default:
                            break;
                    }

                }
                if (cCMove.hold == 1) moveResult.hold = true;
                moveResult.expected_cells = new int[4][];
                for (int j = 0; j < 4; ++j)
                {
                    moveResult.expected_cells[j] = new int[2];
                    moveResult.expected_cells[j][0] = cCMove.expected_x[j];
                    moveResult.expected_cells[j][1] = cCMove.expected_y[j];
                }
                if (_nowIdx == 0)
                {
                    _startTime = DateTime.Now;
                }
                _nowIdx++;
                TimeSpan hopeTime = TimeSpan.FromSeconds(60.0 / _botSetting.BPM * _nowIdx);
                if (DateTime.Now < _startTime + hopeTime)
                {
                    Task.Delay(_startTime + hopeTime - DateTime.Now).Wait();
                }
            }
            _pieceCnt--;
            UpdateNext();
            return moveResult;
        }
    }
}
