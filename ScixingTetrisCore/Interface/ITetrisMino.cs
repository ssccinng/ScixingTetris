using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface ITetrisMino
    {
        string Name { get; }

        (int X, int Y)[] GetMinoField(int Stage);
        //void 
    }
}
