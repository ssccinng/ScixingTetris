using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public abstract class GarbageRule : IGarbageRule
    {
        public static readonly GuildLineGarbageRule GuildLineGarbageRule = new();
        public int Cap { get; set ; }
        public abstract void CrossFire(List<int> garbage, List<int> atk);
    }

    public class GuildLineGarbageRule: GarbageRule
    {
        public GuildLineGarbageRule()
        {
            Cap = 0;
        }
        // 思考效率
        public override void CrossFire(List<int> garbage, List<int> atk)
        {
            while (garbage.Count > 0 && atk.Count > 0)
            {
                if (garbage[0] < atk[0])
                {
                    // 效率堪忧
                    atk[0] -= garbage[0];
                    garbage.RemoveAt(0);
                }
                else
                {
                    garbage[0] -= atk[0];
                    atk.RemoveAt(0);
                }
            }
        }
    }
}
