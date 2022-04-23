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
        public KosTetrisGameBoard(int Width = 10, int Height = 25, int ShowHeight = 25, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null) :
           base(Width, Height, ShowHeight, tetrisRule, tetrisMinoGenerator)
        {
            TetrisRule = tetrisRule ?? ScixingTetrisCore.Rule.TetrisRule.KOS;
            var mlist = tetrisMinoGenerator.GetMinoList();
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
        List<byte[][]> _minoFields = new();
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
            var SpinCheck1 = TetrisRule.SpinRule.IsSpinBeforeClean(this);
            //if(TetrisRule)
            var minoList = TetrisMinoStatus.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var pos in minoList)
            {
                //Field[pos.X][pos.Y] = 1;
                Field[pos.X][pos.Y] = (byte)(TetrisMinoStatus.TetrisMino.MinoType + 1);
            }
            AttackMessage attackMessage = TryClearLines();

            ClearLineCnt += attackMessage.ClearRows;
            var spinType = TetrisRule.SpinRule.GetSpinTypeAfterClean(this, TetrisMinoStatus, attackMessage);
            // 需要思考是否用事件触发攻击事件
            SpawnNewPiece();
            //ReceiveGarbage(new List<int> { 1, 2 });
            //ReceiveGarbage(new List<int> { 1});
            //GetGarbage();
            return true;
        }
        public override void ResetGame()
        {

            ClearLineCnt = 0;
            base.ResetGame();
        }

        public void AddGarbageToField()
        {
            //List<Byte[]> Gabarge = new();
            List<byte[]> Gabarge  = TetrisRule.GarbageGenerator.GetGarbage(GarbageStack);
            GarbageStack.Clear();
            AddField(Gabarge);
            //while (GarbageStack.Count > 0)
            //{


            //}
        }

        public override AttackMessage TryClearLines()
        {
            AttackMessage message = new();
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
    }

    public class KosServerBoard: KosTetrisGameBoard
    {
        public KosClientBoard CreateClient()
        {
            KosClientBoard kosClientBoard = new();

            return kosClientBoard;
        }
    }
    public class KosClientBoard : KosTetrisGameBoard { 
    
    
    
    }

}
