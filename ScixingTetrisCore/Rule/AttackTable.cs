using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Rule
{
    public class AttackTable
    {
        public static readonly AttackTable Guideline = new AttackTable
        {
            ComboTable = new[] { 1, 1, 2, 2, 3, 3, 4, 4, 4, 5, 5, -1 },
            TspinAttack = new[] { 0, 2, 4, 6 },
            MiniTspinAttack = new[] { 0, 0, 2, 6 },
            ClearRowAttack = new[] { 0, 1, 2, 4 },
            PerfectClearAttack = 6,
            //DamageCale
        };
        // 比如minipc 怎么办
        public int[] ComboTable { get; private set; }
        public int[] TspinAttack { get; private set; }
        public int[] MiniTspinAttack { get; private set; }
        public int[] ClearRowAttack { get; private set; }
        public int PerfectClearAttack { get; private set; }

        public Func<int, int> DamageCalc;
    }
}
