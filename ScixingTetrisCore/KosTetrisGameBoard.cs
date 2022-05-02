using ScixingTetrisCore.Tools;
using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class KosTetrisGameBoard: TetrisGameViewBoard
    {

        public string GameMessage = "";
        public event Action<List<int>> OnAtk;
        public KosTetrisGameBoard(int Width = 10, int Height = 25, int ShowHeight = 25, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null) :
           base(Width, Height, ShowHeight, tetrisRule, tetrisMinoGenerator)
        {
            DefaultPos = (22, 3);
            TetrisRule = tetrisRule ?? ScixingTetrisCore.Rule.TetrisRule.KOS;
            //TetrisMinoGenerator = tetrisMinoGenerator ?? new Bag7Generator<TetrisMino>();
            var mlist = TetrisMinoGenerator.GetMinoList();
            foreach (var m in mlist)
            {
                var f = new byte[4][];
                for (int i = 0; i < f.Length; i++)
                {
                    f[i] = new byte[4];
                }
                foreach (var item in m.GetMinoField(0))
                {
                    f[item.X][item.Y] = (byte)(m.MinoType + 1) ;
                }
                _minoFields.Add(f);
            }
            var f1 = new byte[4][];
            for (int i = 0; i < f1.Length; i++)
            {
                f1[i] = new byte[4];
            }
            _minoFields.Add(f1);
        }
        protected List<byte[][]> _minoFields = new();
        public override byte[][] GetHoldField()
        {
            if (HoldMino == null) return _minoFields.Last();
            return _minoFields[(int)HoldMino.MinoType];
        }
        /// <summary>
        /// 最好有优化
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override List<byte[][]> GetNextQueueField()
        {
            List<byte[][]> list = new List<byte[][]>();
            foreach (var item in NextQueue.Take(6))
            {
                list.Add(_minoFields[(int)item.MinoType]);
            }
            return list;
            //throw new NotImplementedException();
        }
        public int ClearLineCnt { get; private set; } = 0;
        public override bool LockMino()
        {
            var SpinCheck1 = TetrisRule.SpinRule.IsSpinBeforeClean(this, TetrisMinoStatus);


            //if(TetrisRule)
            var minoList = TetrisMinoStatus.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var (X, Y) in minoList)
            {
                //Field[pos.X][pos.Y] = 1;
                Field[X][Y] = (byte)(TetrisMinoStatus.TetrisMino.MinoType + 1);
            }
            ClearMessage attackMessage = TryClearLines();

            ClearLineCnt += attackMessage.ClearRows;
            
            if (SpinCheck1 && attackMessage.ClearRows > 0)
            {
                var spinType = TetrisRule.SpinRule.GetSpinTypeAfterClean(this, TetrisMinoStatus, attackMessage);
                attackMessage.ClearType = spinType;
                if (spinType == ClearType.Spin || spinType == ClearType.Minispin || TetrisRule.SpinRule.IsB2B(attackMessage)) attackMessage.B2B = ++B2B;
                else B2B = -1;
            }
            else
            {
                if (TetrisRule.SpinRule.IsB2B(attackMessage))
                {
                    attackMessage.B2B = ++B2B;
                }
                else if (attackMessage.ClearRows != 0)
                {
                    attackMessage.B2B = B2B = -1;
                }

                //if (attackMessage.ClearRows == 4)
                //{
                //    attackMessage.B2B = ++B2B;
                //}
                //else if (attackMessage.ClearRows != 0)
                //{
                //    attackMessage.B2B = B2B = -1;
                //}
            }
            
            GameMessage = $"{TetrisMinoStatus.TetrisMino.MinoType} {attackMessage.ClearType} {attackMessage.ClearRows}";
            // 需要思考是否用事件触发攻击事件
            
            // atk 攻击抵消 
            if (attackMessage.ClearRows > 0)
            {
                // 涨行抵消规则待定
                //此处需抵消

                var atk = TetrisRule.GetAttack(attackMessage);
                TetrisRule.GarbageRule.CrossFire(GarbageStack, atk);
                OnAtk?.Invoke(atk);

            }
            else
            {
                // 涨行抵消规则待定
                AddGarbageToField();

            }
            //ReceiveGarbage(new List<int> { 1, 2 });
            SpawnNewPiece();
            //ReceiveGarbage(new List<int> { 1});
            //GetGarbage();
            return true;
        }
        public override void ResetGame()
        {

            ClearLineCnt = 0;
            base.ResetGame();
        }

        public virtual void AddGarbageToField()
        {
            //List<Byte[]> Gabarge = new();
            List<byte[]> Gabarge  = TetrisRule.GarbageGenerator.GetGarbage(GarbageStack);
            GarbageStack.Clear();
            AddField(Gabarge);
        }

        public override ClearMessage TryClearLines()
        {
            ClearMessage message = new();
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
            //bool isTspin = false;
            //// 可能要改一下
            //if (cnt > 0 && TetrisMinoStatus.LastRotation && TetrisMinoStatus.TetrisMino.MinoType == MinoType.SC_T)
            //{
            //    int spinCnt = 0;
            //    spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X, TetrisMinoStatus.Position.Y) ? 0 : 1;
            //    spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X + 2, TetrisMinoStatus.Position.Y) ? 0 : 1;
            //    spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X, TetrisMinoStatus.Position.Y + 2) ? 0 : 1;
            //    spinCnt += TetrisRule.CheckPostionOk(this, TetrisMinoStatus.Position.X + 2, TetrisMinoStatus.Position.Y + 2) ? 0 : 1;
            //    if (spinCnt >= 3) isTspin = true;
            //    if (spinCnt >= 3) Console.WriteLine("Tspin");
            //}


            //if (cnt == 4 || isTspin) B2B++;
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
            //message.Kickcnt =
            message.ClearFlag = clearFlag;
            message.ClearRows = cnt;
            message.B2B = B2B;
            message.Combo = Combo;
            message.IsPerfectClear = IsPrefect();
            return message;
        }
    }

    public class KosServerBoard: KosTetrisGameBoard
    {
        public KosClientBoard CreateClient()
        {
            // 这里要拷贝一份ServerBoard
            KosClientBoard kosClientBoard = new();
            kosClientBoard.SetField(Field.Clone() as byte[][]);
            kosClientBoard.HoldMino = HoldMino;
            kosClientBoard.NextList = NextQueue.ToArray(); 
            return kosClientBoard;
        }
        public object CreateMessage(int remainMove)
        {
            // 这里要拷贝一份ServerBoard
            return new
            {
                Field = Field,
                HoldMino = HoldMino.MinoType,
                NextList = NextQueue.Select(s => s.MinoType),
                RemainMove = remainMove
            };
            //KosClientBoard kosClientBoard = new();
            //kosClientBoard.SetField(Field.Clone() as byte[][]);
            //kosClientBoard.HoldMino = HoldMino;
            //kosClientBoard.NextList = NextQueue.ToArray();
            //return kosClientBoard;
        }

        public override void GameStart()
        {
            HoldMino = TetrisMinoGenerator.GetNextMino();
            base.GameStart();
        }

        public void NextTurn()
        {

        }
    }
    public class KosClientBoard : KosTetrisGameBoard {

        private int _moveIdx { get; set; } = 0;
        public ITetrisMino[] NextList { get; set; }
        public Stack<byte[][]> PrevField = new();
        public List<List<MoveType>> MoveList { get; set; } = new();
        public override bool SpawnNewPiece()
        {
            _moveIdx++;
            if (_moveIdx >= NextList.Length) return false;
            TetrisMinoStatus = new TetrisMinoStatus { Position = DefaultPos, Stage = 0, TetrisMino = NextList[_moveIdx] };
            return true;
            //return base.SpawnNewPiece();
        }

        public override List<byte[][]> GetNextQueueField()
        {
            List<byte[][]> list = new List<byte[][]>();
            for (int i = _moveIdx + 1; i < NextList.Length; i++)
            {
                list.Add(_minoFields[(int)NextList[i].MinoType]);
            }

            return list;
        }

        public void SetField(byte[][] field)
        {
            Field = field;
        }

        public override void AddGarbageToField()
        {
            List<byte[]> Gabarge = new();
            int sum = GarbageStack.Sum();
            for (int i = 0; i < sum; i++)
            {
                var nl = new byte[Width];
                for (int j = 0; j < Width; j++)
                {
                    nl[j] = 255;
                }
                Gabarge.Add(nl);
            }
            //GarbageStack.Clear();
            AddField(Gabarge);
        }

        public override ClearMessage TryClearLines()
        {
            ClearMessage message = new();
            int cnt = 0;
            bool[] clearFlag = new bool[Height];
            for (int i = 0; i < Height; ++i)
            {
                bool flag = true;
                for (int j = 0; j < Width; ++j)
                {
                    if (Field[i][j] == 0 || Field[i][j] == 255)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag) { cnt++; clearFlag[i] = true; }
            }

            if (cnt > 0) Combo++;
            else Combo = 0;
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
            //message.Kickcnt =
            message.ClearFlag = clearFlag;
            message.ClearRows = cnt;
            message.B2B = B2B;
            message.Combo = Combo;
            message.IsPerfectClear = IsPrefect();
            return message;
        }
        // 撤销功能 如何实现

    }

}
