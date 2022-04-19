using System;
using Hikari.Puzzle;

namespace Hikari.AI.Moves {
    public readonly struct StepRef : IComparable<StepRef> {
        public readonly Piece piece;
        public readonly int cost;

        public StepRef(in Step step) {
            piece = step.piece;
            cost = step.cost;
        }

        public int CompareTo(StepRef other) {
            return cost.CompareTo(other.cost);
        }
    }
}