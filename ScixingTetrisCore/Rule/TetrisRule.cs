using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScixingTetrisCore.Interface;
namespace ScixingTetrisCore.Rule
{
    public abstract class TetrisRule : ITetrisRule
    {
        //public static readonly TetrisRule GuildLine = new()
        //{
        //    RotationSystem = RotationSystem.SRS,
        //    AttackTable = AttackTable.Guideline,
        //};
        /// <summary>
        /// Puyo Puyo Tetris
        /// </summary>
        public static readonly TetrisRule PPT;
        /// <summary>
        /// Tetris Online Poland
        /// </summary>
        public static readonly TetrisRule TOP;
        /// <summary>
        /// Tetr.IO
        /// </summary>
        public static readonly TetrisRule IO;
        /// <summary>
        /// C2(忘了怎么拼)
        /// </summary>
        public static readonly TetrisRule C2;
        /// <summary>
        /// KingOfStacker
        /// </summary>
        public static readonly TetrisRule KOS;
        /// <summary>
        /// Jstris
        /// </summary>
        public static readonly TetrisRule Jstris;
        /// <summary>
        /// 旋转系统
        /// </summary>
        public IRotationSystem RotationSystem { get; private set; }
        public AttackTable AttackTable { get; private set; }
        public IFieldCheck FieldCheck;
        public abstract bool CheckMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);

        public abstract List<int> GetAttack(AttackMessage attackMessage);
    }

    public sealed class GuildLineRule : TetrisRule
    {
        public static GuildLineRule GuildLine;
        public override bool CheckMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            return FieldCheck.IsMinoOk(tetrisBoard, tetrisMinoStatus);
        }

        public override List<int> GetAttack(AttackMessage attackMessage)
        {
            throw new NotImplementedException();
        }
    }
}
