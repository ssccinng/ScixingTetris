using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScixingTetrisCore.Interface;
namespace ScixingTetrisCore.Rule
{
    public enum AttackType
    {
        Normal,
        PerfectClear,
        Tspin,
        Combo,
        Tetris,
    }
    public abstract class AttackRule : IAttackRule
    {
        //public static readonly AttackRule Guideline = new AttackRule
        //{
        //    ComboTable = new[] { 1, 1, 2, 2, 3, 3, 4, 4, 4, 5, 5, -1 },
        //    TspinAttack = new[] { 0, 2, 4, 6 },
        //    MiniTspinAttack = new[] { 0, 0, 2, 6 },
        //    ClearRowAttack = new[] { 0, 1, 2, 4 },
        //    PerfectClearAttack = 6,
        //    //DamageCale
        //};
        // 比如minipc 怎么办
        public int[] ComboTable { get; protected set; }
        public int[] TspinAttack { get; protected set; }
        public int[] MiniTspinAttack { get; protected set; }
        public int[] ClearRowAttack { get; protected set; }
        public int PerfectClearAttack { get; protected set; }

        public abstract List<int> DamageCalc(ClearMessage attackMessage);
        public abstract int DamageCalcSimple(ClearMessage attackMessage);

        //public Func<int, int> DamageCalc;
    }

    public class ARGuildLine : AttackRule
    {
        public static readonly AttackRule Guideline = new ARGuildLine
        {
            ComboTable = new[] { 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5, 5, -1 },
            TspinAttack = new[] { 0, 2, 4, 6 },
            MiniTspinAttack = new[] { 0, 0, 2, 6 },
            ClearRowAttack = new[] { 0, 0, 1, 2, 4 },
            PerfectClearAttack = 6,
            //DamageCale
        };
        // 要不要传出总伤害 因为有时候并不需要
        public override List<int> DamageCalc(ClearMessage attackMessage)
        {
            List<int> res = new List<int>();
            if (attackMessage.IsPerfectClear) res.Add(PerfectClearAttack);
            int atk = 0;
            atk += ComboTable[attackMessage.Combo];
            switch (attackMessage.ClearType)
            {
                case ClearType.None:
                    atk += ClearRowAttack[attackMessage.ClearRows];
                    break;
                case ClearType.Spin:
                    atk += TspinAttack[attackMessage.ClearRows];
                    break;
                case ClearType.Minispin:
                    atk += MiniTspinAttack[attackMessage.ClearRows];
                    break;
                default:
                    break;
            }
            if (attackMessage.B2B > 0) atk++;
            res.Add(atk);
            return res;
        }

        public override int DamageCalcSimple(ClearMessage attackMessage)
        {
            int atk = 0;
            if (attackMessage.IsPerfectClear) atk += PerfectClearAttack;
            atk += ComboTable[attackMessage.Combo];
            switch (attackMessage.ClearType)
            {
                case ClearType.None:
                    atk += ClearRowAttack[attackMessage.ClearRows];
                    break;
                case ClearType.Tspin:
                    atk += TspinAttack[attackMessage.ClearRows];
                    break;
                case ClearType.Minispin:
                    atk += ClearRowAttack[attackMessage.ClearRows];
                    break;
                default:
                    break;
            }
            if (attackMessage.B2B > 0) atk++;
            return atk;
        }
    }


    public class EPlusAttackRule : ARGuildLine
    {
        public override List<int> DamageCalc(ClearMessage attackMessage)
        {
            return base.DamageCalc(attackMessage);
        }

        public override int DamageCalcSimple(ClearMessage attackMessage)
        {
            throw new NotImplementedException();
        }
    }
}
