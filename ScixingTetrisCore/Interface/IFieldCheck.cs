using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface IFieldCheck
    {
        //Func<ITetrisBoard, ITetrisMinoStatus, bool> IsMinoOk { get; }
        //Func<ITetrisBoard, int, int, bool> IsPositionOk { get; }

        bool IsMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        bool IsPositionOk(ITetrisBoard tetrisBoard, int x, int y);
    }
}
