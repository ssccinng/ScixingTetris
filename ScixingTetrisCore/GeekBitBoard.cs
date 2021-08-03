using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class GeekBitBoard : ITetrisBoard
    {
        public int[] Field { get; private set; }
        public int Width { get => 10; }
        public int Height { get => 40; }

        public int ShowHeight { get; set; }

        public ITetrisRule TetrisRule => throw new NotImplementedException();

        public int[] ColHeight => throw new NotImplementedException();

        //public IFieldCheck FieldCheck => throw new NotImplementedException();

        public GeekBitBoard(int Width = 10, int Height = 40, int ShowHeight = 20)
        {
            Field = new int[Height];
            // ? 也许不用现在取
            this.ShowHeight = Math.Min(ShowHeight, Height);
        }
        /// <summary>
        /// 输出场地
        /// </summary>
        /// <param name="WithMino"></param>
        /// <param name="printLeft"></param>
        /// <param name="printTop"></param>
        public void PrintBoard(bool WithMino, int printLeft = 0, int printTop = 0)
        {
            Console.SetCursorPosition(printLeft, printTop);
            for (int i = 0; i < Width + 1; ++i) Console.Write("--");
            Console.Write('\n');
            for (int i = 0; i < ShowHeight; ++i)
            {
                Console.SetCursorPosition(printLeft, ++printTop);
                int pi = ShowHeight - 1 - i;
                Console.Write('|');
                for (int j = 0; j < Width; ++j)
                {
                    if (((Field[pi] >> j) & 1) != 0)
                    {
                        Console.Write("[]");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.Write('|');
                Console.Write('\n');
            }
            Console.SetCursorPosition(printLeft, ++printTop);
            for (int i = 0; i < Width + 1; ++i) Console.Write("--");
            Console.Write('\n');
        }

        public bool LockMino()
        {
            throw new NotImplementedException();
        }

        public bool IsCellFree(int x, int y)
        {
            if (x >= 0 && x < Height && y >= 0 && y < Width)
            {
                return ((Field[x] >> y) & 1) == 1;
            }
            return false;
        }
    }
}
