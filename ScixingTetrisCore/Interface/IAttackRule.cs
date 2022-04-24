using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface IAttackRule
    {
        //public int[] ComboTable { get; }
        //public int[] TspinAttack { get;}
        //public int[] MiniTspinAttack { get; }
        //public int PerfectClearAttack { get; }
        //public int[] ClearRowAttack { get; }
        List<int> DamageCalc(ClearMessage attackMessage);
        int DamageCalcSimple(ClearMessage attackMessage);
    }
}
