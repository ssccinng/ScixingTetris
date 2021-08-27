using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface IRotationSystem
    {
        (bool isSuccess, int kickCnt) LeftRotation(ITetrisRuleBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        (bool isSuccess, int kickCnt) RightRotation(ITetrisRuleBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        (bool isSuccess, int kickCnt) _180Rotation(ITetrisRuleBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
    }
}
