using Hikari.AI.Eval;
using Hikari.AI.Moves;

namespace Hikari.AI {
    public readonly struct Direction {
        public readonly Path path;
        public readonly int nodes;
        public readonly float nps;
        public readonly int depth;
        public readonly Value eval;

        public Direction(Path path, int nodes, float nps, int depth, Value eval) {
            this.path = path;
            this.nodes = nodes;
            this.nps = nps;
            this.depth = depth;
            this.eval = eval;
        }
    }
}