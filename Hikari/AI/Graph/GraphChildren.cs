using System;
using System.Runtime.CompilerServices;
using Hikari.Puzzle;
using Unity.Burst.CompilerServices;

namespace Hikari.AI.Graph {
    public struct GraphChildren : IEquatable<GraphChildren> {
        private SpeculationInfo speculation;
        public readonly int start;

        public GraphChildren(SpeculationInfo spec, int start) {
            speculation = spec;
            this.start = start;
        }

        public readonly bool HasAny => speculation.Sum != 0;
        public readonly SpeculationInfo Speculation => speculation;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly View<GraphChild> GetAll(in GraphGroup group) {
            return group.GetChildren(start, speculation.Sum);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly View<GraphChild> GetSpeculated(in GraphGroup group, [AssumeRange(0, 6)] int kind) {
            var offset = start + speculation.GetStartOf(kind);
            return group.GetChildren(offset, speculation.GetLengthOf(kind));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly View<GraphChild> GetSpeculated(in GraphGroup group, PieceKind kind) =>
            GetSpeculated(group, (int) kind);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly View<GraphChild> GetKnown(in GraphGroup group) {
            var piece = speculation.GetKnownPiece();
            var offset = start + speculation.GetStartOf(piece);
            return group.GetChildren(offset, speculation.GetLengthOf(piece));
        }

        public bool ResolveSpeculation(PieceKind resolved) {
            return speculation.Resolve(resolved);
        }

        public bool Equals(GraphChildren other) {
            return speculation.Equals(other.speculation) && start == other.start;
        }

        public override bool Equals(object obj) {
            return obj is GraphChildren other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (speculation.GetHashCode() * 397) ^ start;
            }
        }

        public static bool operator ==(GraphChildren left, GraphChildren right) {
            return left.Equals(right);
        }

        public static bool operator !=(GraphChildren left, GraphChildren right) {
            return !left.Equals(right);
        }
    }
}