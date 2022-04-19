using System;
using Unity.Mathematics;

namespace Hikari.AI.Eval {
    /// <summary>
    /// Value is the score of a board.
    /// <para/> This is deterministic, i.e. this is determined by only board state and evaluation weights.
    /// <para/> This is also the score of a vert of graph.
    /// </summary>
    public struct Value : IEquatable<Value> {
        public int4 value;
        public int spike;

        public Value(int4 value, int spike) {
            this.value = value;
            this.spike = spike;
        }

        public static Value operator +(Value lhs, Value rhs) {
            return new Value {value = lhs.value + rhs.value, spike = lhs.spike + rhs.spike};
        }

        public static Value operator +(Value lhs, Reward rhs) {
            lhs.value += new int4(0, 0, rhs.evaluation, 0);
            lhs.spike = rhs.attack < 0 ? 0 : lhs.spike + rhs.attack;
            return lhs;
        }

        public static Value operator *(Value lhs, int rhs) {
            return new Value {
                value = lhs.value * rhs,
                // spike = lhs.spike * rhs
            };
        }

        public static Value operator /(Value lhs, int rhs) {
            return new Value {
                value = lhs.value / rhs,
                // spike = lhs.spike / rhs
            };
        }

        public Value Improve(Value next) {
            return new Value {
                value = math.csum(next.value) > math.csum(value) ? next.value : value,
                spike = math.max(spike, next.spike)
            };
        }

        public override string ToString() {
            return $"({value.x},{value.y},{value.z},{value.w}) with {spike} spk";
        }

        public bool Equals(Value other) {
            return value.Equals(other.value) && spike == other.spike;
        }

        public override bool Equals(object obj) {
            return obj is Value other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (spike * 397) ^ value.GetHashCode();
            }
        }

        public static bool operator ==(Value left, Value right) {
            return left.Equals(right);
        }

        public static bool operator !=(Value left, Value right) {
            return !left.Equals(right);
        }
    }
}