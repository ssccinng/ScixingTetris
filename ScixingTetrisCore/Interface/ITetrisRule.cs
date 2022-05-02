using ScixingTetrisCore.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScixingTetrisCore.Interface
{
    public interface ITetrisRule
    {
        /// <summary>
        /// 场地检查
        /// </summary>
        //IFieldCheck FieldCheck { get; }
        /// <summary>
        /// 旋转系统
        /// </summary>
        IRotationSystem RotationSystem { get; }
        IGarbageGenerator GarbageGenerator { get; }
        ITetrisMinoGenerator MinoGenerator { get; }

        SpinB2BRule SpinRule { get; }
        GarbageRule GarbageRule { get; }

        ///// <summary>
        ///// 攻击表（？ 感觉要换一下
        ///// </summary>
        //IAttackTable AttackTable { get; set; } 
        //Func<ITetrisBoard, ITetrisMinoStatus, bool> CheckMinoOk { get; }
        //Func<AttackMessage, List<int>> GetAttack { get; }


        bool CheckMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        bool CheckPostionOk(ITetrisBoard tetrisBoard, int x, int y);
        List<int> GetAttack(ClearMessage attackMessage);
    }
}
