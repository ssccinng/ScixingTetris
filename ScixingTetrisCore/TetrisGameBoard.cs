using ScixingTetrisCore.Interface;
using ScixingTetrisCore.Rule;
using ScixingTetrisCore.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class TetrisGameBoard : ITetrisGameBoard
    {
        public byte[][] Field { get; private set; }
        public int Height { get => Field.Length; }
        public int Width { get => Field[0].Length; }
        public int ShowHeight { get; set; }
        public int[] ColHeight { get; set; }
        public ITetrisRule TetrisRule { get; private set; }

        public Queue<ITetrisMino> NextQueue => throw new NotImplementedException();

        public bool IsDead => throw new NotImplementedException();

        // 对于单方块场地
        public ITetrisMinoStatus TetrisMinoStatus;

        // hold
        public ITetrisMino HoldMino;
        /// <summary>
        /// 生成器 要在这里吗（
        /// </summary>
        public ITetrisMinoGenerator TetrisMinoGenerator;
        //public IFieldCheck FieldCheck => throw new NotImplementedException();

        public TetrisGameBoard(int Width = 10, int Height = 40, int ShowHeight = 20, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null)
        {
            // 赋予规则
            TetrisRule = tetrisRule ?? GuildLineRule.Rule;
            TetrisMinoGenerator = tetrisMinoGenerator ?? new Bag7Generator<TetrisMino>();
            Field = new byte[Height][];
            for (int i = 0; i < Height; ++i)
            {
                Field[i] = new byte[Width];
            }
            // ? 也许不用现在取
            this.ShowHeight = Math.Min(ShowHeight, Height);
            ColHeight = new int[Width];
            SpawnNewPiece();
        }

        public int TryClearLines()
        {
            int cnt = 0;
            // 限制一下搜索高度
            //List<int> clearidx = new List<int>();
            bool[] clearFlag = new bool[Height]; 
            for (int i = 0; i < Height; ++i)
            {
                bool flag = true;
                for (int j = 0; j < Width; ++j)
                {
                    if (Field[i][j] == 0)
                    {
                        flag = false;
                    }
                }
                if (flag) { cnt++; clearFlag[i] = true; }
            }
            for (int i = 0, j = 0; i < Height; ++i, ++j)
            {
                while (j < Height && clearFlag[j])
                {
                    ++j;
                }
                if (j >= Height)
                {
                    Field[i] = new byte[Width];
                }
                else
                {
                    Field[i] = Field[j];
                }
                
            }
            return cnt;
        }

        /// <summary>
        /// 输出场地
        /// </summary>
        /// <param name="printLeft"></param>
        /// <param name="printTop"></param>
        public void PrintBoard(bool WithMino = false, int printLeft = 0, int printTop = 0)
        {
            int tempTop = printTop;
            Console.SetCursorPosition(printLeft, printTop); ;
            for (int i = 0; i < Width + 1; ++i) Console.Write("--");
            Console.Write('\n');
            for (int i = 0; i < ShowHeight; ++i)
            {
                Console.SetCursorPosition(printLeft, ++printTop);
                int pi = ShowHeight - 1 - i;
                Console.Write('|');
                for (int j = 0; j < Width; ++j)
                {
                    if (Field[pi][j] != 0)
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

            if (WithMino && TetrisMinoStatus != null)
            {
                foreach (var pos in TetrisMinoStatus?.GetMinoFieldListInBoard())
                {
                    // pos 要在显示区域内
                    // 肯定有问题.jpg
                    Console.SetCursorPosition(printLeft + 1 + pos.Y * 2, tempTop + (ShowHeight - pos.X));
                    Console.Write("[]");
                }

            }
        }

        public bool LockMino()
        {
            //if(TetrisRule)
            var minoList = TetrisMinoStatus.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var pos in minoList)
            {
                Field[pos.X][pos.Y] = 1;
            }
            SpawnNewPiece();
            TryClearLines();
            return true;
        }

        public bool IsCellFree(int x, int y)
        {
            if (x >= 0 && x < Height && y >= 0 && y < Width)
            {
                return Field[x][y] == 0;
            }
            return false;
        }

        public bool LeftRotation()
        {
            return TetrisRule.RotationSystem.LeftRotation(this, TetrisMinoStatus).isSuccess;
        }

        public bool RightRotation()
        {
            return TetrisRule.RotationSystem.RightRotation(this, TetrisMinoStatus).isSuccess;
        }

        public bool _180Rotation()
        {
            throw new NotImplementedException();
        }

        public bool MoveLeft()
        {
            TetrisMinoStatus.MoveLeft();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                return true;
            }
            TetrisMinoStatus.MoveRight();
            return false;
        }

        public bool MoveRight()
        {
            TetrisMinoStatus.MoveRight();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                return true;
            }
            TetrisMinoStatus.MoveLeft();
            return false;
        }

        public bool SoftDrop()
        {
            TetrisMinoStatus.MoveBottom();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                return true;
            }
            TetrisMinoStatus.MoveTop();
            return false;
        }

        public bool SonicDrop()
        {
            while (SoftDrop()) ;
            return true;
        }
        /// <summary>
        /// 硬降 需要确认方块位置没问题吗
        /// </summary>
        /// <returns></returns>
        public bool HardDrop()
        {
            SonicDrop();
            LockMino();
            return true;
        }

        public bool OnHold()
        {
            // if 允许hold

            if (HoldMino == null)
            {
                HoldMino = TetrisMinoStatus.TetrisMino;
                SpawnNewPiece();
            }
            else
            {
                (HoldMino, TetrisMinoStatus.TetrisMino) = (TetrisMinoStatus.TetrisMino, HoldMino);
                TetrisMinoStatus.Position = (19, 3);
                TetrisMinoStatus.Stage = 0;
            }
            return true;
        }

        public bool SpawnNewPiece()
        {
            // 先简略来一个（ 后续要改 要考虑方块用什么 需不需要接口 要看看成不成功
            TetrisMinoStatus = new TetrisMinoStatus { Position = (19, 3), Stage = 0, TetrisMino = TetrisMinoGenerator.GetNextMino() };
            return true;
        }
    }
}
