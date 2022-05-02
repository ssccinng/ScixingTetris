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
    // 生成位置确定一下
    public class TetrisGameBoard : ITetrisGameBoard
    {
        public byte[][] Field { get; protected set; }
        public int Height { get => Field.Length; }
        public int Width { get => Field[0].Length; }
        public int ShowHeight { get; set; }
        public int[] ColHeight { get; set; }
        public ITetrisRule TetrisRule { get; protected set; }

        //public Queue<ITetrisMino> NextQueue => throw new NotImplementedException(); 
        public Queue<ITetrisMino> NextQueue { get; } = new();

        public bool IsDead => false;

        // 对于单方块场地
        public ITetrisMinoStatus TetrisMinoStatus;

        // hold
        public ITetrisMino HoldMino;
        /// <summary>
        /// 生成器 要在这里吗（
        /// </summary>
        public ITetrisMinoGenerator TetrisMinoGenerator;
        /// <summary>
        /// 垃圾行储存
        /// </summary>
        public List<int> GarbageStack { get; } = new();

        //public IFieldCheck FieldCheck => throw new NotImplementedException();


        // 此处还欠考虑
        public int B2B { get; set; } = -1;
        public int Combo { get; set; }

        public (int X, int Y) DefaultPos = (20, 3);
        // 此处还欠考虑
        public TetrisGameBoard(int Width = 10, int Height = 40, int ShowHeight = 20, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null)
        {
            // 赋予规则
            TetrisRule = tetrisRule ?? Rule.TetrisRule.GuildLine;
            TetrisMinoGenerator = tetrisMinoGenerator ?? new Bag7Generator<TetrisMino>();
            Field = new byte[Height][];
            for (int i = 0; i < Height; ++i)
            {
                Field[i] = new byte[Width];
            }
            // ? 也许不用现在取
            this.ShowHeight = Math.Min(ShowHeight, Height);
            ColHeight = new int[Width];
            // 别spawn
            //SpawnNewPiece();
        }

        // 加入接口
        public virtual void GameStart()
        {
            NextQueue.Clear();
            for (int i = 0; i < 7; i++)
            {
                NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            }
            SpawnNewPiece();
        }
        protected ClearMessage CountClearLines()
        {
            ClearMessage count = new ClearMessage();
            bool[] clearFlag = new bool[Height];
            count.ClearFlag = clearFlag;
            //List<byte> clearFlag = new();
            for (int i = 0; i < Height; ++i)
            {

                for (int j = 0; j < Width; ++j)
                {
                    if (Field[i][j] == 0)
                    {
                        clearFlag[i] = true;
                        ++count.ClearRows;
                        break;
                    }
                }
            }
            return count;
        }
        //private List<byte> CountClearLines()
        //{
        //    //bool[] clearFlag = new bool[Height];
        //    List<byte> clearFlag = new();
        //    for (int i = 0; i < Height; ++i)
        //    {
  
        //        for (int j = 0; j < Width; ++j)
        //        {
        //            if (Field[i][j] == 0)
        //            {
        //                //clearFlag[i] = true;
        //                clearFlag.Add(i);
        //                break;
        //            }
        //        }
        //    }
        //    return clearFlag;
        //}
        //private void ClearLine(List<bool> clearFlag)
        //{
        //    int idx = 0;
        //    for (int i = 0, j = 0; i < Height; ++i, ++j)
        //    {
        //        while (j < Height && clearFlag[idx] == j)
        //        {
        //            ++j;
        //            idx++;
        //        }
        //        if (j >= Height)
        //        {
        //            Field[i] = new byte[Width];
        //        }
        //        else
        //        {
        //            Field[i] = Field[j];
        //        }

        //    }
        //}
        protected void ClearLine(bool[] clearFlag)
        {
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
        }

        public virtual ClearMessage TryClearLines()
        {
            ClearMessage message = new ();
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
                        break;
                    }
                }
                if (flag) { cnt++; clearFlag[i] = true; }
            }
            
            if (cnt > 0) Combo++;
            else Combo = 0;
            // 以上位消行检测

            bool isTspin = false;
            // 可能要改一下
            if (cnt > 0 && TetrisMinoStatus.LastRotation && TetrisMinoStatus.TetrisMino.MinoType == MinoType.SC_T)
            {
                int spinCnt = 0;
                spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X, TetrisMinoStatus.Position.Y) ? 0 : 1;
                spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X + 2, TetrisMinoStatus.Position.Y) ? 0 : 1;
                spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X, TetrisMinoStatus.Position.Y + 2) ? 0 : 1;
                spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X + 2, TetrisMinoStatus.Position.Y + 2) ? 0 : 1;
                if (spinCnt >= 3) isTspin = true;
                if (spinCnt >= 3) Console.WriteLine("Tspin");
            }
            if (cnt == 4 || isTspin) B2B++;


            // 以下为消行行为
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

            message.ClearRows = cnt;
            message.B2B = B2B;
            message.ClearType = isTspin ? ClearType.Tspin : ClearType.None;
            message.Combo = Combo;
            message.IsPerfectClear = IsPrefect();
            return message;
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

        public virtual bool LockMino()
        {
            //if(TetrisRule)

            var minoList = TetrisMinoStatus.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var (X, Y) in minoList)
            {
                //Field[pos.X][pos.Y] = 1;
                Field[X][Y] = (byte)(TetrisMinoStatus.TetrisMino.MinoType + 1);
            }
            TryClearLines();
            SpawnNewPiece();
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
        public bool IsCellFreeWithMino(int x, int y)
        {
            bool flag = true;
            if (TetrisMinoStatus != null)
            {
                foreach (var pos in TetrisMinoStatus?.GetMinoFieldListInBoard())
                {
                    if (pos.X == x && pos.Y == y)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag && IsCellFree(x, y);
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
            return TetrisRule.RotationSystem._180Rotation(this, TetrisMinoStatus).isSuccess;

            //throw new NotImplementedException();
        }

        public bool MoveLeft()
        {
            TetrisMinoStatus.MoveLeft();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                TetrisMinoStatus.LastRotation = false;
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
                TetrisMinoStatus.LastRotation = false;
                return true;
            }
            TetrisMinoStatus.MoveLeft();
            return false;
        }

        public bool MoveUp()
        {
            TetrisMinoStatus.MoveTop();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                TetrisMinoStatus.LastRotation = false;
                return true;
            }
            TetrisMinoStatus.MoveBottom();
            return false;
        }
        public bool SoftDrop()
        {
            TetrisMinoStatus.MoveBottom();
            if (TetrisRule.CheckMinoOk(this, TetrisMinoStatus))
            {
                TetrisMinoStatus.LastRotation = false;
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
                //TetrisMinoStatus.Position = (19, 3);
                TetrisMinoStatus.Position = DefaultPos;
                TetrisMinoStatus.Stage = 0;
                SoftDrop();
            }
            return true;
        }

        public virtual bool SpawnNewPiece()
        {
            // 先简略来一个（ 后续要改 要考虑方块用什么 需不需要接口 要看看成不成功
            //TetrisMinoStatus = new TetrisMinoStatus { Position = (19, 3), Stage = 0, TetrisMino = TetrisMinoGenerator.GetNextMino() };
            //TetrisMinoStatus = new TetrisMinoStatus { Position = (19, 3), Stage = 0, TetrisMino = NextQueue.Dequeue() };
            TetrisMinoStatus = new TetrisMinoStatus { Position = DefaultPos, Stage = 0, TetrisMino = NextQueue.Dequeue() };
            // 这个撕烤 根据不同的规则生成
            NextQueue.Enqueue(TetrisMinoGenerator.GetNextMino());
            // 针对io 立即下降一格
            //SoftDrop();
            return true;
        }

        public virtual void ResetGame()
        {
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; j++)
                {
                    Field[i][j] = 0;
                }
            }
            for (int i = 0; i < Width; i++)
            {
                ColHeight[i] = 0;
            }
            HoldMino = null;
            //for (int i = 0; i < ColHeight.Length; i++)
            //{
            //    ColHeight[i] = 0;
            //}
            TetrisMinoGenerator.Reset();
            GarbageStack.Clear();
            B2B = -1;
            Combo = 0;
            GameStart();
            //ColHeight = new int[Width];
        }

        public virtual void ReceiveGarbage(List<int> garbages)
        {
            for (int i = 0; i < garbages.Count; i++)
            {
                GarbageStack.Add(garbages[i]);
            }
        }

        public virtual void AddField(List<byte[]> field)
        {
            for (int i = Height - field.Count - 1; i >= 0 ; --i)
            {
                Field[i + field.Count] = Field[i];
            }
            for (int i = 0; i < field.Count && i < Height; i++)
            {
                Field[i] = field[i];    
            }
            //for (int i = 0; i < ColHeight.Length; i++)
            //{
            //    ColHeight[i] += field.Count;
            //}
            //throw new NotImplementedException();
        }
        public virtual bool IsPrefect()
        {
            return !Field[0].Any(s => s != 0);
        }
    }
}
