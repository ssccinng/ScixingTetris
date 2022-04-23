using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Rule
{
    public abstract class SpinRule
    {
        public static readonly TSpinRule TSpinRule = new ();
        public static readonly AllSpinMoveAbleRule AllSpinMoveAbleRule = new ();
        public abstract bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard);
        public abstract ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, AttackMessage attackMessage);
    }
    public class TSpinRule : SpinRule
    {
        public override bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard)
        {
            return true;
        }
        public override ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, AttackMessage attackMessage)
        {
            if (tetrisMino.TetrisMino.MinoType == MinoType.SC_T)
            {
                return ClearType.Spin;
            }
            else
            {
                return ClearType.None;
            }
        }
    }
    public class AllSpinMoveAbleRule : SpinRule
    {
        public override ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, AttackMessage attackMessage)
        {
            if (attackMessage.ClearRows == 0) return ClearType.None; 
            switch (tetrisMino.TetrisMino.MinoType)
            {
                case MinoType.SC_I:
                    if (tetrisMino.Stage % 2 == 0)
                    {
                        return ClearType.Spin;
                    }
                    else
                    {
                        return attackMessage.ClearRows == 4 ? ClearType.Spin : ClearType.Minispin;
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
                    if (tetrisMino.Stage % 2 == 0)
                    {
                        return attackMessage.ClearRows == 2 ? ClearType.Spin : ClearType.Minispin;

                    }
                    else
                    {
                        return attackMessage.ClearRows == 3 ? ClearType.Spin : ClearType.Minispin;

                    }
                    break;
                default:
                    return ClearType.None;
                    break;
            }
        }

        public override bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard)
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
    public class ZXCSpinRule : SpinRule
    {

        public override ClearType GetSpinTypeAfterClean(ITetrisGameBoard tetrisGameBoard, ITetrisMinoStatus tetrisMino, AttackMessage attackMessage)
        {
            return ClearType.Spin;
            //throw new NotImplementedException();
        }

        public override bool IsSpinBeforeClean(ITetrisGameBoard tetrisGameBoard)
        {
            throw new NotImplementedException();
        }
    }
}
