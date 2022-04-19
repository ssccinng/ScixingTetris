using Hikari.Puzzle;

namespace Hikari.AI.Documents {
    public readonly struct Expanded {
        public readonly int childIndex;
        public readonly int time;
        public readonly Piece piece;
        public readonly SimpleColBoard board;
        public readonly SimpleLockResult placement;
        public readonly bool parentIsB2B;

        public Expanded(int childIndex, int time, Piece piece, SimpleColBoard board, SimpleLockResult placement, bool parentIsB2B) {
            this.childIndex = childIndex;
            this.time = time;
            this.piece = piece;
            this.board = board;
            this.placement = placement;
            this.parentIsB2B = parentIsB2B;
        }
    }
}