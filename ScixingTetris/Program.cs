using System;
using System.Threading.Tasks;
using ScixingTetrisCore;
using ScixingTetrisCore.Tools;


namespace ScixingTetris
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            TetrisBoard tetrisBoard = new(ShowHeight: 20);
            //TetrisBitBoard tetrisBoard1 = new(ShowHeight: 20);
            Random rnd = new();
            Console.CursorVisible = false;
            tetrisBoard.TetrisMinoStatus = new TetrisMinoStatus { TetrisMino = TetrisMino.S, Stage = 0, Position = (15, 3) };
            for (int i = 0; i < 200; ++i)
            {
                //tetrisBoard1.Field[rnd.Next(20)] |= (1 << rnd.Next(10));
                tetrisBoard.Field[rnd.Next(20), rnd.Next(10)] = 1;
                tetrisBoard.TetrisMinoStatus.LeftRoll();
                //tetrisBoard1.PrintBoard();
                tetrisBoard.PrintBoard(true, 0, 0);
                await Task.Delay(500);
            }

            Bag7Generator bag7Generator = new Bag7Generator();

            for (int i = 0; i < 200; ++i)
            {
                if (i % 7 == 0) Console.Write('\n');
                Console.Write(bag7Generator.GetNextMino().Name);
                await Task.Delay(500);
            }
        }
    }
}
