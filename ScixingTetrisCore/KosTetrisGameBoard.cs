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
        public KosTetrisGameBoard(int Width = 10, int Height = 40, int ShowHeight = 20, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null) :
           base(Width, Height, ShowHeight, tetrisRule, tetrisMinoGenerator)
        {
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
            //if(TetrisRule)
            var minoList = TetrisMinoStatus.GetMinoFieldListInBoard();
            // 要不不检查了（？
            // 断言此时的场地和方块是ok的
            foreach (var pos in minoList)
            {
                //Field[pos.X][pos.Y] = 1;
                Field[pos.X][pos.Y] = (byte)(TetrisMinoStatus.TetrisMino.MinoType + 1);
            }
            ClearLineCnt += TryClearLines();
            SpawnNewPiece();
            return true;
        }
        public override void ResetGame()
        {

            ClearLineCnt = 0;
            base.ResetGame();
        }
    }
}
