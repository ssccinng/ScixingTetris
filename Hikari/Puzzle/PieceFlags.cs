using System;
using Unity.Mathematics;

namespace Hikari.Puzzle {
    [Flags]
    public enum PieceFlags : byte {
        I = 1 << 0,
        O = 1 << 1,
        T = 1 << 2,
        J = 1 << 3,
        L = 1 << 4,
        S = 1 << 5,
        Z = 1 << 6,
        All = (1 << 7) - 1
    }

    public static class PieceFlagsExtensions {
        public static PieceFlags ToFlags(this PieceKind kind) {
            return (PieceFlags) (1 << (byte) kind);
        }

        public static PieceKind ToSingle(this PieceFlags flags) {
            var ui = (uint) flags;
            if (math.countbits(ui) != 1) throw new ArgumentException();
            return (PieceKind) math.tzcnt(ui);
        }

        public static bool IsSingle(this PieceFlags flags) {
            return math.countbits((uint) flags) == 1;
        }

        public static bool Contains(this PieceFlags flags, int kind) {
            return ((int) flags & (1 << kind)) > 0;
        }

        public static bool Contains(this PieceFlags flags, PieceKind kind) => Contains(flags, (int) kind);
    }
}