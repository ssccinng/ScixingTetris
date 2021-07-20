using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface IBoardStatus
    {
        int Combo { get; set; }
        int B2B { get; set; }
        
    }
}
