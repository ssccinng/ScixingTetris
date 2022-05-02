using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Rule
{
    public abstract class SpinB2BRule
    {
        // 需要增加b2b判定

        public static readonly TSpinRule TSpinRule = new ();
        public static readonly AllSpinMoveAbleRule AllSpinMoveAbleRule = new ();
        /// <summary>
        /// 判断锁块前是否是spin态
        /// </summary>
        /// <param name="tetrisGameBoard"></param>
        /// <returns></returns>
        public abstract bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino);
        public abstract ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, ClearMessage attackMessage);
        public abstract bool IsB2B(ClearMessage attackMessage);
    }
    /// <summary>
    /// 高效Tspin判定
    /// </summary>
    public class TSpinRule : SpinB2BRule
    {
        public override bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino)
        {
            //if (tetrisMino.TetrisMino.MinoType != MinoType.SC_T) return false;
            
            //tetrisGameBoard.
            /// 三角判定
            return true;
        }
        public override ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, ClearMessage attackMessage)
        {
            if (attackMessage.ClearRows > 0 && tetrisMino.LastRotation && tetrisMino.TetrisMino.MinoType == MinoType.SC_T)
            {
                int spinCnt = 0;
                spinCnt += tetrisGameBoard.TetrisRule.CheckPostionOk(tetrisGameBoard, tetrisMino.Position.X, tetrisMino.Position.Y) ? 0 : 1;
                spinCnt += tetrisGameBoard.TetrisRule.CheckPostionOk(tetrisGameBoard, tetrisMino.Position.X + 2, tetrisMino.Position.Y) ? 0 : 1;
                spinCnt += tetrisGameBoard.TetrisRule.CheckPostionOk(tetrisGameBoard, tetrisMino.Position.X, tetrisMino.Position.Y + 2) ? 0 : 1;
                spinCnt += tetrisGameBoard.TetrisRule.CheckPostionOk(tetrisGameBoard, tetrisMino.Position.X + 2, tetrisMino.Position.Y + 2) ? 0 : 1;
                //return true;
                if (spinCnt >= 3)
                {
                    return ClearType.Spin;
                }
            }
            return ClearType.None;
            
        }

        public override bool IsB2B(ClearMessage attackMessage)
        {
            //if (attackMessage.ClearRows == 4) return true;
            return attackMessage.ClearRows == 4;
        }
    }
    public class AllSpinMoveAbleRule : SpinB2BRule
    {
        public override ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, ClearMessage attackMessage)
        {
            
            if (attackMessage.ClearRows == 0) return ClearType.None; 
            switch (tetrisMino.TetrisMino.MinoType)
            {
                case MinoType.SC_I:
                    if (tetrisMino.Stage  == 0 || tetrisMino.Stage == 2)
                    {
                        return ClearType.Spin;
                    }
                    else
                    {
                        return attackMessage.ClearRows == 4 || tetrisMino.Kickcnt == 0 ? ClearType.Spin : ClearType.Minispin;
                    }
                    break;
                case MinoType.SC_O:
                    if (attackMessage.ClearRows == 2)
                        return ClearType.Spin;
                    else return ClearType.Minispin;
                    break;
                case MinoType.SC_T:
                case MinoType.SC_L:
                case MinoType.SC_J:
                case MinoType.SC_S:
                case MinoType.SC_Z:
                    if (tetrisMino.Stage == 0 || tetrisMino.Stage == 2)
                    {
                        return attackMessage.ClearRows == 2 || tetrisMino.Kickcnt == 0 ? ClearType.Spin : ClearType.Minispin;

                    }
                    else
                    {
                        return attackMessage.ClearRows == 3 || tetrisMino.Kickcnt == 0 ? ClearType.Spin : ClearType.Minispin;
                    }
                    break;
                default:
                    return ClearType.None;
                    break;
            }
        }

        public override bool IsB2B(ClearMessage attackMessage)
        {
            if (attackMessage.ClearRows == 4) return true;
            return false;
        }

        public override bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino)
        {
            if (tetrisGameBoard.MoveUp())
            {
                tetrisGameBoard.SoftDrop();
                return false;
            }
            if (tetrisGameBoard.SoftDrop())
            {
                tetrisGameBoard.MoveRight();
                return false;
            }
            if (tetrisGameBoard.MoveLeft())
            {
                tetrisGameBoard.MoveRight();
                return false;
            }
            if (tetrisGameBoard.MoveRight())
            {
                tetrisGameBoard.MoveLeft();
                return false;
            }
            return true;

        }
    }
    public class ZXCSpinRule : SpinB2BRule
    {

        public override ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, ClearMessage attackMessage)
        {
            return ClearType.Spin;
            //throw new NotImplementedException();
        }

        public override bool IsB2B(ClearMessage attackMessage)
        {
            throw new NotImplementedException();
        }

        public override bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino)
        {
            throw new NotImplementedException();
        }
    }
}
