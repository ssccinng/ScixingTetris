using System;
using System.IO;
using System.Threading.Tasks;
using ScixingTetrisCore;
using ScixingTetrisCore.Tools;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace ScixingTetris
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //TetrisGameBoard tetrisBoard = new(ShowHeight: 22, tetrisMinoGenerator : new Bag7Generator<TetrisMino>());
            GeekTetrisGameBoard tetrisBoard = new(ShowHeight: 22);
            //TetrisGameBoard tetrisBoard = new(ShowHeight: 22, tetrisMinoGenerator : new Bag7Generator<GeekTetrisMino>());
            //TetrisBitBoard tetrisBoard1 = new(ShowHeight: 20);
            //Bag7Generator bag7Generator = new Bag7Generator();
            GeekTetrisMino bag7Generator1 = new GeekTetrisMino();
            Random rnd = new();
            Console.CursorVisible = false;
            if (!Directory.Exists("save")) Directory.CreateDirectory("save");
            if (Directory.Exists("save"))
            {
                var paths = Directory.GetFiles("save");
                for (int i = 0; i < paths.Length; ++i)
                {
                    Console.WriteLine($"{i + 1}. {paths[i]}");
                }
                //Console.WriteLine("要选择哪个存档？如需新游戏，则输入0");
                Console.WriteLine("choose what save？if need new game，input 0");
                int choose = int.Parse(Console.ReadLine());
                if (choose != 0)
                {
                    GeekTetrisGameBoard save = JsonSerializer.Deserialize<GeekTetrisGameBoard>(File.ReadAllText(paths[choose - 1]));

                    tetrisBoard.Field = save.Field;
                    tetrisBoard.stageidx = save.stageidx - 1;
                    tetrisBoard.UpdataIdx(save.idx);
                    tetrisBoard.Score = save.Score;
                    while(save.res[save.res.Length - 1] != 'N')
                    {
                        save.res.Remove(save.res.Length - 1, 1);
                    }
                    tetrisBoard.res = save.res;
                    Console.Clear();

                }
            }
            //tetrisBoard.TetrisMinoStatus = new TetrisMinoStatus { TetrisMino = TetrisMino.I, Stage = 0, Position = (15, 3) };
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            for (int i = 0; i != -1; ++i)
            {
                
                //tetrisBoard1.Field[rnd.Next(20)] |= (1 << rnd.Next(10));
                //tetrisBoard.Field[rnd.Next(20), rnd.Next(10)] = 1;
                //if (tetrisBoard.TetrisMinoStatus == null || key.Key == ConsoleKey.W)
                //    tetrisBoard.TetrisMinoStatus = new TetrisMinoStatus { TetrisMino = bag7Generator.GetNextMino(), Stage = 0, Position = (15, 3) };
                //tetrisBoard.TetrisMinoStatus.LeftRoll();
                tetrisBoard.PrintBoard(true, 0, 0);
                key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.A:
                        tetrisBoard.MoveLeft();
                        break;
                    case ConsoleKey.D:
                        tetrisBoard.MoveRight();
                        break;
                    case ConsoleKey.S:
                        tetrisBoard.SoftDrop();
                        break;
                    case ConsoleKey.L:
                        tetrisBoard.RightRotation();
                        break;
                        //case ConsoleKey.K:
                        //    tetrisBoard.LeftRotation();
                        break;
                    case ConsoleKey.W:
                        tetrisBoard.HardDrop();
                        break;
                    case ConsoleKey.P:
                        tetrisBoard.ReturnPrev();
                        break;
                    //case ConsoleKey.J:
                    //    tetrisBoard.OnHold();
                    //break;
                    case ConsoleKey.R:
                        Console.Clear();
                        //File aa = new File("geekres");
                        //File.WriteAllText($"save/{tetrisBoard.idx}块 {tetrisBoard.Score}分.jks", tetrisBoard.GetRes);
                        if (!Directory.Exists("save")) Directory.CreateDirectory("save");
                        File.WriteAllText($"save/{DateTime.Now:yyyy-MM-dd HH-mm-ss} {tetrisBoard.idx}块 {tetrisBoard.Score}分.json", JsonSerializer.Serialize(tetrisBoard));
                        Console.WriteLine(tetrisBoard.GetRes);
                        Console.WriteLine("save success!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        break;
                }
                //switch (key.Key)
                //{
                //    case ConsoleKey.LeftArrow:
                //        tetrisBoard.MoveLeft();
                //        break;
                //    case ConsoleKey.RightArrow:
                //        tetrisBoard.MoveRight();
                //        break;
                //    case ConsoleKey.DownArrow:
                //        tetrisBoard.SoftDrop();
                //        break;
                //    case ConsoleKey.X:
                //        tetrisBoard.RightRotation();
                //        break;
                //        //case ConsoleKey.K:
                //        //    tetrisBoard.LeftRotation();
                //        break;
                //    case ConsoleKey.Spacebar:
                //        tetrisBoard.HardDrop();
                //        break;
                //    case ConsoleKey.P:
                //        tetrisBoard.ReturnPrev();
                //        break;
                //    //case ConsoleKey.J:
                //    //    tetrisBoard.OnHold();
                //    //break;
                //    case ConsoleKey.R:
                //        Console.Clear();
                //        //File aa = new File("geekres");
                //        //File.WriteAllText($"save/{tetrisBoard.idx}块 {tetrisBoard.Score}分.jks", tetrisBoard.GetRes);
                //        if (!Directory.Exists("save")) Directory.CreateDirectory("save");
                //        File.WriteAllText($"save/{DateTime.Now:yyyy-MM-dd HH-mm-ss} {tetrisBoard.idx}Pieces {tetrisBoard.Score}Score.json", JsonSerializer.Serialize(tetrisBoard));
                //        Console.WriteLine(tetrisBoard.GetRes);
                //        Console.WriteLine("save success!");
                //        Console.ReadKey();
                //        Console.Clear();
                //        break;
                //    default:
                //        break;
                //}
                //tetrisBoard1.PrintBoard();

                //await Task.Delay(500);
            }

            

            //for (int i = 0; i < 200; ++i)
            //{
            //    if (i % 7 == 0) Console.Write('\n');
            //    Console.Write(bag7Generator.GetNextMino().Name);
            //    await Task.Delay(500);
            //}
        }
    }
}
