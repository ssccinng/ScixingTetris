using System;
using Hikari.Puzzle;

namespace Hikari.AI.Moves {
    public readonly struct Step : IComparable<Step> {
        public readonly Piece parent;
        public readonly int cost;
        public readonly int depth;
        public readonly Piece piece;
        public readonly Instruction inst;

        public Step(Piece parent, int cost, int depth, Piece piece, Instruction inst) {
            this.parent = parent;
            this.cost = cost;
            this.depth = depth;
            this.piece = piece;
            this.inst = inst;
        }

        public int CompareTo(Step other) {
            var costComparison = cost.CompareTo(other.cost);
            if (costComparison != 0) return costComparison;
            return depth.CompareTo(other.depth);
        }
    }
}