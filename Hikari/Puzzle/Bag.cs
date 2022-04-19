using System;
using System.Text;
using Unity.Mathematics;

namespace Hikari.Puzzle {
    public struct Bag : IEquatable<Bag> {
        private byte value;
        public int Count => math.countbits((uint) value);

        private byte TakeRandom() {
            if (value == 0) RestoreAll();
            var n = UnityEngine.Random.Range(0, Count);
            var a = 0;
            for (byte i = 0; i < 7; i++) {
                if ((value & (1 << i)) <= 0) continue;
                if (a++ != n) continue;

                value -= unchecked((byte) (1u << i));
                return i;
            }

            throw new Exception();
        }

        public bool Take(PieceKind pk) {
            if (value == 0) RestoreAll();
            if (!Contains(pk)) return false;
            value &= (byte) ~(1u << (int) pk);
            return true;
        }

        public PieceKind TakeRandomPiece() => (PieceKind) TakeRandom();

        public readonly bool Contains(PieceKind pk) => (value & (1 << (int) pk)) != 0;

        private void RestoreAll() {
            value = 0b1111111;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            for (var i = 0; i < 7; i++) {
                if (Contains((PieceKind) i)) sb.Append(((PieceKind) i).ToString()).Append(' ');
            }

            sb.Append(Count);
            return sb.ToString().Trim();
        }

        public static implicit operator PieceFlags(Bag bag) {
            return bag.value == 0 ? (PieceFlags) ((1 << 7) - 1) : (PieceFlags) bag.value;
        }

        public bool Equals(Bag other) {
            return value == other.value;
        }

        public override bool Equals(object obj) {
            return obj is Bag other && Equals(other);
        }

        public override int GetHashCode() {
            return value;
        }

        public static bool operator ==(Bag left, Bag right) {
            return left.Equals(right);
        }

        public static bool operator !=(Bag left, Bag right) {
            return !left.Equals(right);
        }
    }
}