using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    // 暂时固定为40 * 10大小的bit数组
    public class TetrisBitBoard : ITetrisBoard
    {
        public int[] Field { get; private set; }
        public int[] ColHeight;
        public int Width { get => 10; }
        public int Height { get => 40; }
        
        public int ShowHeight { get; set; }

        public ITetrisRule TetrisRule => throw new NotImplementedException();

        int[] ITetrisBoard.ColHeight => throw new NotImplementedException();

        public TetrisBitBoard(int Width = 10, int Height = 40, int ShowHeight = 20)
        {
            Field = new int[Height];
            // ? 也许不用现在取
            this.ShowHeight = Math.Min(ShowHeight, Height);
        }
        public void PrintBoard(bool WithMino = false, int printLeft = 0, int printTop = 0)
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
        public bool LockMino(ITetrisMinoStatus tetrisMinoStatus)
        {
            var minoList = tetrisMinoStatus.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var pos in minoList)
            {
                Field[pos.X] |= 1 << pos.Y;
            }
            ClearLines();
            return true;
        }
        // 消行 是不是也该放到规则里
        public int ClearLines()
        {
            int cnt = 0;
            // 限制一下搜索高度 场地最高高度
            //List<int> clearidx = new List<int>();
            bool[] clearFlag = new bool[Height];
            for (int i = 0; i < Height; ++i)
            {
                if ((Field[i] & ((1 << Width) - 1)) == 0)
                {
                    cnt++; clearFlag[i] = true;
                }
            }
            for (int i = 0, j = 0; i < Height; ++i, ++j)
            {
                while (j < Height && clearFlag[j])
                {
                    ++j;
                }
                if (j >= Height)
                {
                    Field[i] = 0;
                }
                else
                {
                    Field[i] = Field[j];
                }

            }
            return cnt;
        }
        public bool IsCellFree(int x, int y)
        {
            if (x >= 0 && x < Height && y >= 0 && y < Width)
            {
                return ((Field[x] >> y) & 1) == 0;
            }
            return false;
        }
    }
}
