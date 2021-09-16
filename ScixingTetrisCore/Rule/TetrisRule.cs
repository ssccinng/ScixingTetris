using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScixingTetrisCore.Interface;
using ScixingTetrisCore.Rule;
namespace ScixingTetrisCore.Rule
{
    public abstract class TetrisRule : ITetrisRule
    {
        //public static readonly TetrisRule GuildLine = new()
        //{
        //    RotationSystem = Rule.RotationSystem.SRS,
        //    AttackTable = AttackTable.Guideline,
        //    FieldCheck = Rule.FieldCheck.GuildLine,
        //    CheckMinoOk = (tetrisBoard, tetrisMinoStatus) => FieldCheck.IsMinoOk(tetrisBoard, tetrisMinoStatus),
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

        //public Func<ITetrisBoard, ITetrisMinoStatus, bool> CheckMinoOk { get; private set; }

        //public Func<AttackMessage, List<int>> GetAttack { get; private set; }

        /// <summary>
        /// 旋转系统 暂时先公开吧 后续再撕烤一下
        /// </summary>
        public IRotationSystem RotationSystem { get; set; }
        /// <summary>
        /// 攻击表
        /// </summary>
        protected AttackRule _attackRule { get; set; }

        protected IFieldCheck _fieldCheck;
        public abstract bool CheckMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus);
        public abstract bool CheckPostionOk(ITetrisBoard tetrisBoard, int x, int y);

        public abstract List<int> GetAttack(AttackMessage attackMessage);
    }

    public class GuildLineRule : TetrisRule
    {
        public static GuildLineRule Rule = new()
        {
            RotationSystem = ScixingTetrisCore.Rule.RotationSystem.SRS,
            //RotationSystem = ScixingTetrisCore.Rule.RotationSystem.Geek,
            _fieldCheck = FCGuildLine.FieldCheck,
            _attackRule = ARGuildLine.Guideline,
        };
        public override bool CheckMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            return _fieldCheck.IsMinoOk(tetrisBoard, tetrisMinoStatus);
        }

        public override bool CheckPostionOk(ITetrisBoard tetrisBoard, int x, int y)
        {
            return _fieldCheck.IsPositionOk(tetrisBoard, x, y);
        }

        public override List<int> GetAttack(AttackMessage attackMessage)
        {
            throw new NotImplementedException();
        }
    }

    public class GeekTetrisRule : TetrisRule
    {
        public static GeekTetrisRule Rule = new()
        {
            //RotationSystem = ScixingTetrisCore.Rule.RotationSystem.SRS,
            RotationSystem = ScixingTetrisCore.Rule.RotationSystem.Geek,
            _fieldCheck = FCGuildLine.FieldCheck,
            _attackRule = ARGuildLine.Guideline,
        };
        public override bool CheckMinoOk(ITetrisBoard tetrisBoard, ITetrisMinoStatus tetrisMinoStatus)
        {
            return _fieldCheck.IsMinoOk(tetrisBoard, tetrisMinoStatus);
        }

        public override bool CheckPostionOk(ITetrisBoard tetrisBoard, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetAttack(AttackMessage attackMessage)
        {
            throw new NotImplementedException();
        }
    }
}
