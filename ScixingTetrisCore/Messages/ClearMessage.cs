using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Messages
{
    internal class ClearMessage
    {
        public int ClearRows { get; set; }
        public ITetrisMinoStatus tetrisMinoStatus { get; set; }
        public bool IsPrefectClear { get; set; }
    }
}
