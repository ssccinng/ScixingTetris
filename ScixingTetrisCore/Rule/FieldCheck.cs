using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Rule
{
    // 还是说要传入Field(?
    // 需要在这里check吗 能不能在board里check
    //public class FieldCheck<TTetrisMino, TTetrisBoard>
    //    where TTetrisMino  : ITetrisMino
    //    where TTetrisBoard : ITetrisBoard
    //{

    //    public static readonly FieldCheck<TTetrisMino, TTetrisBoard> Guideline = new()
    //    {
    //        IsMinoOK = (TTetrisMino, TTetrisBoard) =>
    //        {
    //            return true;
    //        }
    //    };
    //    public static readonly FieldCheck<TTetrisMino, TTetrisBoard> Jstris = new()
    //    {
    //        IsMinoOK = (TTetrisMino, TTetrisBoard) =>
    //        {
    //            return true;
    //        }
    //    };
    //    public Func<TTetrisMino, TTetrisBoard, bool> IsMinoOK;
    //    // 比如js20层以上可以重叠
    //}

    public abstract class FieldCheck : IFieldCheck
    {

        public abstract bool IsMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        
        // 比如js20层以上可以重叠
    }

    public class FCGuildLine : FieldCheck
    {
        public override bool IsMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            var minoPos = tetrisMinoStatus.GetMinoFieldListInBoard();
            foreach (var pos in minoPos)
            {
                if (!tetrisBoard.IsCellFree(pos.X, pos.Y)) return false;
            }
            return true;
        }
    }
}

