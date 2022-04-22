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

    public abstract class FieldCheck : IFieldCheck
    {

        //public static readonly FieldCheck GuildLine = new FieldCheck
        //{
        //    IsMinoOk = (ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus) =>
        //    {
        //        var minoPos = tetrisMinoStatus.GetMinoFieldListInBoard();
        //        foreach (var pos in minoPos)
        //        {
        //            if (!tetrisBoard.IsCellFree(pos.X, pos.Y)) return false;
        //        }
        //        return true;
        //    },
        //    IsPositionOk = (ITetrisBoard tetrisBoard, int x, int y) =>
        //    {
        //        return tetrisBoard.IsCellFree(x, y);
        //    }
        //};

        public abstract bool IsMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        public abstract bool IsPositionOk(ITetrisBoard tetrisBoard, int x, int y);

        // 比如js20层以上可以重叠
        //public Func<ITetrisBoard, ITetrisMinoStatus, bool> IsMinoOk { get; private set; }
        //public Func<ITetrisBoard, int, int, bool> IsPositionOk { get; private set; }
    }

    public class FCGuildLine : FieldCheck
    {
        public static readonly FCGuildLine FieldCheck = new FCGuildLine { };
        private FCGuildLine() { }
        public override bool IsMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            var minoPos = tetrisMinoStatus.GetMinoFieldListInBoard();
            foreach (var pos in minoPos)
            {
                if (!tetrisBoard.IsCellFree(pos.X, pos.Y)) return false;
            }
            return true;
        }

        public override bool IsPositionOk(ITetrisBoard tetrisBoard, int x, int y)
        {
            return tetrisBoard.IsCellFree(x, y);
        }
    }

    
}

