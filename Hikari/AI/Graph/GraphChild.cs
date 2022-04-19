using System.Runtime.CompilerServices;
using Hikari.AI.Eval;
using Hikari.Puzzle;

namespace Hikari.AI.Graph {
    public struct GraphChild {
        public readonly int node;
        public Reward reward;
        public Piece piece;

        public GraphChild(int node, Piece piece) {
            this.node = node;
            reward = default;
            this.piece = piece;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Value Eval(in GraphGroup childGroup) {
            return childGroup.GetNode(node).value + reward;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Death(in GraphGroup childGroup) {
            return childGroup.GetNode(node).death;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref GraphNode Node(in GraphGroup childGroup) {
            return ref childGroup.GetNode(node);
        }
    }
}