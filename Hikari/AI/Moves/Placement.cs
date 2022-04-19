using System;
using Hikari.Puzzle;

namespace Hikari.AI.Moves {
    public struct Placement : IComparable<Placement> {
        public sbyte x;
        public sbyte y;
        public sbyte r;
        public TSpinStatus t;
        public Instruction prev;
        public byte depth;
        public byte cost;

        public int CompareTo(Placement other) {
            return cost.CompareTo(other.cost);
        }
    }
}