using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class MinoMessage
    {
        public bool IsPerfectClear;
        public bool IsSelfClear;
        public bool IsLastRotation;
        // 这个可能要通过外部判断
        public bool IsSpin;
        public int ClearRows;

    }
}
