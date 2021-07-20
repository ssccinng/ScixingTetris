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
        ///// <summary>
        ///// 旋转系统
        ///// </summary>
        //IRotationSystem RotationSystem { get; }
        ///// <summary>
        ///// 攻击表（？ 感觉要换一下
        ///// </summary>
        //IAttackTable AttackTable { get; set; }

        bool CheckMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        List<int> GetAttack(AttackMessage attackMessage);
    }
}
