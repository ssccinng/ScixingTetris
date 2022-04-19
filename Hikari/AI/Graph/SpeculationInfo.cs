using System;
using Hikari.Puzzle;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Hikari.AI.Graph {
    public struct SpeculationInfo : IEquatable<SpeculationInfo> {
        private ushort I;
        private ushort O;
        private ushort T;
        private ushort J;
        private ushort L;
        private ushort S;
        private ushort Z;
        private byte possibilities;

        public readonly int Sum => I + O + T + J + L + S + Z;

        public readonly bool IsKnown() => math.countbits((uint) possibilities) == 1;
        public readonly PieceKind GetKnownPiece() => (PieceKind) math.tzcnt((uint) possibilities);

        public readonly bool Has(int kind) {
            return (possibilities & (1U << kind)) != 0U;
        }

        public readonly bool Has(PieceKind kind) => Has((int) kind);

        public readonly int CountPossiblePieces() => math.countbits((uint) possibilities);

        public readonly unsafe ushort GetLengthOf([AssumeRange(0,6)] int kind) {
            fixed (SpeculationInfo* ptr = &this) {
                return ((ushort*) ptr)[kind];
            }
        }

        public readonly ushort GetLengthOf(PieceKind kind) => GetLengthOf((int) kind);

        public readonly int GetStartOf([AssumeRange(0, 6)] int kind) {
            var start = 0;
            for (var i = 0; i < kind; i++) {
                start += GetLengthOf(i);
            }

            return start;
        }

        public readonly int GetStartOf(PieceKind kind) => GetStartOf((int) kind);

        public unsafe void Set(PieceKind kind, ushort length) {
            // if ((possibilities & ~((1 << (int) kind) - 1)) != 0) {
            //     Debug.LogError("aaa");
            // }
            fixed (SpeculationInfo* ptr = &this) {
                ((ushort*) ptr)[(int) kind] = length;
            }

            possibilities |= (byte) (1 << (int) kind);
        }

        public bool Resolve(PieceKind resolved) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (possibilities == 0) {
                throw new Exception("Speculation is invalid");
            }
#endif

            if (!Has(resolved)) {
                return false;
            }
            
            possibilities = (byte) (1 << (int) resolved);
            return true;
        }

        public unsafe bool Equals(SpeculationInfo other) {
            fixed (SpeculationInfo* ptr = &this) {
                return UnsafeUtility.MemCmp(ptr, &other, sizeof(SpeculationInfo)) == 0;
            }
        }

        public override bool Equals(object obj) {
            return obj is SpeculationInfo other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = I.GetHashCode();
                hashCode = (hashCode * 397) ^ O.GetHashCode();
                hashCode = (hashCode * 397) ^ T.GetHashCode();
                hashCode = (hashCode * 397) ^ J.GetHashCode();
                hashCode = (hashCode * 397) ^ L.GetHashCode();
                hashCode = (hashCode * 397) ^ S.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ possibilities.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(SpeculationInfo left, SpeculationInfo right) {
            return left.Equals(right);
        }

        public static bool operator !=(SpeculationInfo left, SpeculationInfo right) {
            return !left.Equals(right);
        }
    }
}