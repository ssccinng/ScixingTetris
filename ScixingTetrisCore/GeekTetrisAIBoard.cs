using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScixingTetrisCore.Interface;
using ScixingTetrisCore.Rule;

namespace ScixingTetrisCore
{
    public class GeekTetrisAIBoard : ITetrisAIBoard
    {
        public int Width => 10;

        public int Height => 22;

        public int[] Field { get; private set; }
        // 需要被clone
        public int[] ColHeight { get; private set; }
        // 需要被clone
        public ITetrisRule TetrisRule { get; private set; }

        public TetrisMinoStatus TetrisMinoStatus;
        // 这个要怎么做呢？
        public bool IsDead => !TetrisRule.CheckMinoOk(this, TetrisMinoStatus) || (Field[19] & ((1 << Width) - 1)) != 0;

        public GeekTetrisAIBoard(ITetrisRule tetrisRule = null)
        {
            TetrisRule = tetrisRule ?? GeekTetrisRule.Rule;
            Field = new int[Height];
            ColHeight = new int[Width];

        }
        public bool HardDrop(ITetrisMinoStatus tetrisMino)
        {
            SonicDrop(tetrisMino);
            return LockMino(tetrisMino);
        }

        public bool IsCellFree(int x, int y)
        {
            if (x >= 0 && x < Height && y >= 0 && y < Width)
            {
                return ((Field[x] >> y) & 1) == 0;
            }
            return false;
        }

        public bool LeftRotation(ITetrisMinoStatus tetrisMino)
        {
            return TetrisRule.RotationSystem.LeftRotation(this, tetrisMino).isSuccess;

        }
        public int ClearLines()
        {
            int cnt = 0;
            // 限制一下搜索高度 场地最高高度
            //List<int> clearidx = new List<int>();
            bool[] clearFlag = new bool[Height];
            for (int i = 0; i < Height; ++i)
            {

                if (((~Field[i]) & ((1 << Width) - 1)) == 0)
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
            if (cnt > 0)
            {
                for (int i = 0; i < Width; ++i)
                {
                    ColHeight[i] -= cnt;
                    while (IsCellFree(ColHeight[i] - 1, i)) ColHeight[i]--;
                }
            }
            
            
            return cnt;
        }


        // 产生动作信息
        public bool LockMino(ITetrisMinoStatus tetrisMino)
        {
            var minoList = tetrisMino.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var pos in minoList)
            {
                Field[pos.X] |= 1 << pos.Y;
                if (pos.X + 1 > ColHeight[pos.Y])
                {
                    ColHeight[pos.Y] = pos.X + 1;
                }
            }
            // 怎么返回
            ClearLines();
            return true;
            
        }

        public bool LockMino()
        {
            throw new NotImplementedException();
        }

        public bool MoveLeft(ITetrisMinoStatus tetrisMino)
        {
            tetrisMino.MoveLeft();
            if (TetrisRule.CheckMinoOk(this, tetrisMino))
            {
                return true;
            }
            tetrisMino.MoveRight();
            return false;
        }

        public bool MoveRight(ITetrisMinoStatus tetrisMino)
        {
            tetrisMino.MoveRight();
            if (TetrisRule.CheckMinoOk(this, tetrisMino))
            {
                return true;
            }
            tetrisMino.MoveLeft();
            return false;
        }

        public void PrintBoard(bool WithMino = false, int printLeft = 0, int printTop = 0)
        {
            Console.SetCursorPosition(printLeft, printTop);
            for (int i = 0; i < Width + 1; ++i) Console.Write("--");
            Console.Write('\n');
            for (int i = 0; i < 22; ++i)
            {
                Console.SetCursorPosition(printLeft, ++printTop);
                int pi = 22 - 1 - i;
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
            for (int i = 0; i < Width; ++i) Console.Write(ColHeight[i] + " ");
        }

        public bool RightRotation(ITetrisMinoStatus tetrisMino)
        {
            return TetrisRule.RotationSystem.RightRotation(this, tetrisMino).isSuccess;
        }

        public bool SoftDrop(ITetrisMinoStatus tetrisMino)
        {
            tetrisMino.MoveBottom();
            if (TetrisRule.CheckMinoOk(this, tetrisMino))
            {
                return true;
            }
            tetrisMino.MoveTop();
            return false;
        }

        public bool SonicDrop(ITetrisMinoStatus tetrisMino)
        {
            // 修改布尔判定
            //while (SoftDrop(tetrisMino)) ;
            var res = tetrisMino.GetMinoFieldListInBoard();
            int dis = 999;
            for (int i = 0; i < res.Length; ++i)
            {
                dis = Math.Min(dis, res[i].X - ColHeight[res[i].Y]);
            }
            if (dis != 999 && dis >= 0)
            {
                tetrisMino.MoveBottom(dis);
                return true;
            }
                
            return false;
        }

        public bool _180Rotation(ITetrisMinoStatus tetrisMino)
        {
            throw new NotImplementedException();
        }

        public bool ResetBoard(byte[][] field)
        {
            if (field.Length != Height && field[0].Length != Width) return false;
            for (int i = 0; i < Height;++i)
            {
                Field[i] = 0;
                for (int j = Width - 1; j >= 0; --j)
                {
                    Field[i] <<= 1;
                    Field[i] |= field[i][j];
                }
            }
            // 重新计算一下colheight
            return true;
        }

        public GeekTetrisAIBoard Clone()
        {
            var res = new GeekTetrisAIBoard(TetrisRule);
            res.Field = (int[])Field.Clone();
            res.ColHeight = (int[])ColHeight.Clone();
            //res.Field
            return res;
        }
    }
}
