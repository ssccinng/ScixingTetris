using System;
using Unity.Collections;

namespace Hikari.AI.Graph {
    public readonly struct GraphNodePtr : IEquatable<GraphNodePtr> {
        public readonly int groupIndex;
        public readonly int nodeIndex;

        public GraphNodePtr(int groupIndex, int nodeIndex) {
            this.groupIndex = groupIndex;
            this.nodeIndex = nodeIndex;
        }

        public bool Equals(GraphNodePtr other) {
            return groupIndex == other.groupIndex && nodeIndex == other.nodeIndex;
        }

        public override bool Equals(object obj) {
            return obj is GraphNodePtr other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (groupIndex * 397) ^ nodeIndex;
            }
        }

        public static bool operator ==(GraphNodePtr left, GraphNodePtr right) {
            return left.Equals(right);
        }

        public static bool operator !=(GraphNodePtr left, GraphNodePtr right) {
            return !left.Equals(right);
        }
    }

    public static class GraphNodePtrExtensions {
        public static ref GraphNode Actualize(this NativeList<GraphGroup> graph, GraphNodePtr ptr) {
            return ref graph.ElementAt(ptr.groupIndex).GetNode(ptr.nodeIndex);
        }
    }
}