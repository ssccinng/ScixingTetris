using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class TetrisPosition
    {
        public int X, Y;
        public static TetrisPosition operator+(TetrisPosition a, TetrisPosition b)
        {
            return new TetrisPosition { X = a.X + b.X, Y = a.Y + b.Y };
        }
        public static TetrisPosition operator-(TetrisPosition a, TetrisPosition b)
        {
            return new TetrisPosition { X = a.X - b.X, Y = a.Y - b.Y };
        }
    }
}
