using ScixingTetrisCore.Interface;
using ScixingTetrisCore.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class TetrisBoard: ITetrisBoard
    {
        public byte[,] Field { get; private set; }
        public int Height { get => Field.GetLength(0); }
        public int Width { get => Field.GetLength(1); }
        public int ShowHeight { get; set; }

        public ITetrisRule TetrisRule => throw new NotImplementedException();
        // 对于单方块场地
        public ITetrisMinoStatus TetrisMinoStatus;
        /// <summary>
        /// 生成器 要在这里吗（
        /// </summary>
        public ITetrisMinoGenerator TetrisMinoGenerator;
        //public IFieldCheck FieldCheck => throw new NotImplementedException();

        public TetrisBoard(int Width = 10, int Height = 40, int ShowHeight = 20)
        {
            Field = new byte[Height, Width];
            // ? 也许不用现在取
            this.ShowHeight = Math.Min(ShowHeight, Height);
        }
        
        /// <summary>
        /// 输出场地
        /// </summary>
        /// <param name="printLeft"></param>
        /// <param name="printTop"></param>
        public void PrintBoard(bool WithMino = false, int printLeft = 0, int printTop = 0)
        {
            int tempTop = printTop;
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
                    if (Field[pi, j] != 0)
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

            if (WithMino)
            {
                foreach (var pos in TetrisMinoStatus?.GetMinoFieldListInBoard())
                {
                    // 肯定有问题.jpg
                    Console.SetCursorPosition(printLeft + 1 + pos.Y * 2, tempTop  + (ShowHeight - pos.X));
                    Console.Write("[]");
                }
                
            }
        }

        public bool LockMino()
        {
            //if(TetrisRule)
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {

            }
            return true;
        }

        public bool IsCellFree(int x, int y)
        {
            if (x >= 0 && x < Height && y >= 0 && y <= Width)
            {
                return Field[x, y] == 0;
            }
            return false;
        }
    }
}
