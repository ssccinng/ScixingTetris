using Hikari.AI.Graph;
using Hikari.Puzzle;

namespace Hikari.AI.Documents {
    public readonly struct Selected {
        public readonly bool valid;
        public readonly bool canHold;
        public readonly PieceFlags candidates;
        public readonly GraphNodePtr node;
        public readonly SimpleColBoard board;

        public Selected(bool canHold, PieceFlags candidates, GraphNodePtr node, SimpleColBoard board) {
            valid = true;
            this.canHold = canHold;
            this.candidates = candidates;
            this.node = node;
            this.board = board;
        }
    }
}