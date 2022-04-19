using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Hikari.AI {
    public readonly struct StateBoard : IEquatable<StateBoard> {
        private readonly uint4x3 data;

        public unsafe StateBoard(SimpleColBoard board) {
            fixed (uint4x3* ptr = &data) {
                UnsafeUtility.MemCpy(ptr, board.GetColumnsPtr(), sizeof(uint) * 10);
            }

            var hold = board.hold.HasValue ? (int) board.hold.Value : 0xFF;
            data.c2.z = (uint)(
                (hold << 24) |
                (board.bag.GetHashCode() << 16) |
                (board.ren << 8) |
                (board.backToBack.GetHashCode()));
        }

        public bool Equals(StateBoard other) {
            return data.Equals(other.data);
        }

        public override bool Equals(object obj) {
            return obj is StateBoard other && Equals(other);
        }

        public override int GetHashCode() {
            return data.GetHashCode();
        }

        public static bool operator ==(StateBoard left, StateBoard right) {
            return left.Equals(right);
        }

        public static bool operator !=(StateBoard left, StateBoard right) {
            return !left.Equals(right);
        }
    }
}