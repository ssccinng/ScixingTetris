using System;
using Hikari.AI.Eval;
using Unity.Collections.LowLevel.Unsafe;

namespace Hikari.AI.Graph {
    public struct GraphNode : IEquatable<GraphNode> {
        public bool death;
        public bool evaluated;
        public int parent;
        public GraphChildren children;
        public Value value;

        public GraphNode(Value value, int parent) {
            death = false;
            evaluated = false;
            this.parent = parent;
            children = default;
            this.value = value;
        }

        public bool IsLeaf => !children.HasAny;
        public override string ToString() {
            return $"D:{death} C:{children.Speculation.Sum} / {value}";
        }

        public unsafe bool Equals(GraphNode other) {
            fixed (GraphNode* ptr = &this) {
                return UnsafeUtility.MemCmp(ptr, &other, sizeof(GraphNode)) == 0;
            }
        }

        public override bool Equals(object obj) {
            return obj is GraphNode other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = death.GetHashCode();
                hashCode = (hashCode * 397) ^ children.GetHashCode();
                hashCode = (hashCode * 397) ^ value.GetHashCode();
                return hashCode;
            }
        }


        public static bool operator ==(GraphNode left, GraphNode right) {
            return left.Equals(right);
        }

        public static bool operator !=(GraphNode left, GraphNode right) {
            return !left.Equals(right);
        }
    }
}