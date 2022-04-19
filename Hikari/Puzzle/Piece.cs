using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Hikari.Puzzle {
    public struct Piece : IEquatable<Piece> {
        public PieceKind Kind;
        public int X;
        public int Y;
        public int Spin;
        public TSpinStatus Tspin;
        public bool IsInvalid;

        public static Piece Invalid => new Piece(default, default, default, default, default, true);

        public Piece(PieceKind kind, int x, int y, int spin, TSpinStatus tspin = TSpinStatus.None,
            bool isInvalid = false) {
            Kind = kind;
            X = x;
            Y = y;
            Spin = spin;
            Tspin = tspin;
            IsInvalid = isInvalid;
        }

        public static readonly uint4x4[] NativeShapes = {
            math.transpose(new uint4x4(
                0, 0, 15, 0,
                4, 4, 4, 4,
                0, 15, 0, 0,
                2, 2, 2, 2)),
            math.transpose(new uint4x4(
                0, 6, 6, 0,
                0, 6, 6, 0,
                0, 6, 6, 0,
                0, 6, 6, 0)),
            math.transpose(new uint4x4(
                0, 7, 2, 0,
                2, 6, 2, 0,
                2, 7, 0, 0,
                2, 3, 2, 0)),
            math.transpose(new uint4x4(
                0, 7, 1, 0,
                2, 2, 6, 0,
                4, 7, 0, 0,
                3, 2, 2, 0)),
            math.transpose(new uint4x4(
                0, 7, 4, 0,
                6, 2, 2, 0,
                1, 7, 0, 0,
                2, 2, 3, 0)),
            math.transpose(new uint4x4(
                0, 3, 6, 0,
                4, 6, 2, 0,
                3, 6, 0, 0,
                2, 3, 1, 0)),
            math.transpose(new uint4x4(
                0, 6, 3, 0,
                2, 6, 4, 0,
                6, 3, 0, 0,
                1, 3, 2, 0))
        };


        public static readonly uint4x4[] NativeVShapes = {
            math.transpose(new uint4x4(
                4, 4, 4, 4,
                0, 0, 15, 0,
                2, 2, 2, 2,
                0, 15, 0, 0)),
            math.transpose(new uint4x4(
                0, 6, 6, 0,
                0, 6, 6, 0,
                0, 6, 6, 0,
                0, 6, 6, 0)),
            math.transpose(new uint4x4(
                2, 6, 2, 0,
                0, 7, 2, 0,
                2, 3, 2, 0,
                2, 7, 0, 0)),
            math.transpose(new uint4x4(
                6, 2, 2, 0,
                0, 7, 4, 0,
                2, 2, 3, 0,
                1, 7, 0, 0)),
            math.transpose(new uint4x4(
                2, 2, 6, 0,
                0, 7, 1, 0,
                3, 2, 2, 0,
                4, 7, 0, 0)),
            math.transpose(new uint4x4(
                2, 6, 4, 0,
                0, 6, 3, 0,
                1, 3, 2, 0,
                6, 3, 0, 0)),
            math.transpose(new uint4x4(
                4, 6, 2, 0,
                0, 3, 6, 0,
                2, 3, 1, 0,
                3, 6, 0, 0))
        };

        public static readonly int2x4[] Cells = {
            new int2x4(0, 1, 2, 3, 2, 2, 2, 2),
            new int2x4(2, 2, 2, 2, 0, 1, 2, 3),
            new int2x4(0, 1, 2, 3, 1, 1, 1, 1),
            new int2x4(1, 1, 1, 1, 0, 1, 2, 3),

            new int2x4(1, 1, 2, 2, 1, 2, 1, 2),
            new int2x4(1, 1, 2, 2, 1, 2, 1, 2),
            new int2x4(1, 1, 2, 2, 1, 2, 1, 2),
            new int2x4(1, 1, 2, 2, 1, 2, 1, 2),

            new int2x4(0, 1, 2, 1, 1, 1, 1, 2),
            new int2x4(1, 1, 2, 1, 0, 1, 1, 2),
            new int2x4(1, 0, 1, 2, 0, 1, 1, 1),
            new int2x4(1, 0, 1, 1, 0, 1, 1, 2),

            new int2x4(0, 1, 2, 0, 1, 1, 1, 2),
            new int2x4(1, 1, 1, 2, 0, 1, 2, 2),
            new int2x4(2, 0, 1, 2, 0, 1, 1, 1),
            new int2x4(0, 1, 1, 1, 0, 0, 1, 2),

            new int2x4(0, 1, 2, 2, 1, 1, 1, 2),
            new int2x4(1, 2, 1, 1, 0, 0, 1, 2),
            new int2x4(0, 0, 1, 2, 0, 1, 1, 1),
            new int2x4(1, 1, 0, 1, 0, 1, 2, 2),

            new int2x4(0, 1, 1, 2, 1, 1, 2, 2),
            new int2x4(2, 1, 2, 1, 0, 1, 1, 2),
            new int2x4(0, 1, 1, 2, 0, 0, 1, 1),
            new int2x4(1, 0, 1, 0, 0, 1, 1, 2),

            new int2x4(1, 2, 0, 1, 1, 1, 2, 2),
            new int2x4(1, 1, 2, 2, 0, 1, 1, 2),
            new int2x4(1, 2, 0, 1, 0, 0, 1, 1),
            new int2x4(0, 0, 1, 1, 0, 1, 1, 2)
        };


        public ushort[] GetShape() {
            var shape = NativeShapes[(int) Kind][Spin];
            return new[] {(ushort) shape.x, (ushort) shape.y, (ushort) shape.z, (ushort) shape.w};
        }

        public int2x4 GetCells() {
            return Cells[(int) Kind * 4 + Spin] + new int2x4(X, X, X, X, Y, Y, Y, Y);
        }

        public int2x4 GetCells(in NativeArray<int2x4> cells) {
            return cells[(int) Kind * 4 + Spin] + new int2x4(X, X, X, X, Y, Y, Y, Y);
        }

        public IEnumerable<Vector2Int> GetCellPositions() {
            var shape = GetShape();
            for (var i = 0; i < 4; i++) {
                for (var j = 0; j < 4; j++) {
                    if ((shape[i] & (1 << j)) != 0) {
                        yield return new Vector2Int(X + j, Y + i);
                    }
                }
            }
        }

        public unsafe bool Equals(Piece other) {
            fixed (Piece* ptr = &this) {
                return UnsafeUtility.MemCmp(ptr, &other, sizeof(Piece)) == 0;
            }
        }

        public override bool Equals(object obj) {
            return obj is Piece other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode =
                    ((uint) Kind << 13) |
                    ((uint) (X + 3) << 9) |
                    ((uint) (Y + 3) << 4) |
                    ((uint) Spin << 2) |
                    ((uint) Tspin);
                return unchecked((int) hashCode);
            }
        }

        public override string ToString() {
            return IsInvalid ? "Invalid" : $"{Kind}{(Kind == PieceKind.T ? $"_{Tspin}" : "")} ({X},{Y}) {Spin}";
        }

        public static bool operator ==(Piece left, Piece right) {
            return left.Equals(right);
        }

        public static bool operator !=(Piece left, Piece right) {
            return !left.Equals(right);
        }

        public readonly Piece WithOffset(Vector2Int offset) {
            return new Piece(Kind, X + offset.x, Y + offset.y, Spin, Tspin);
        }

        public readonly Piece WithOffset(int x, int y) {
            return WithOffset(new Vector2Int(x, y));
        }

        public readonly Piece WithOffset(int2 v) {
            return WithOffset(new Vector2Int(v.x, v.y));
        }

        public readonly Piece WithTSpinStatus(TSpinStatus ts) {
            return new Piece(Kind, X, Y, Spin, ts);
        }

        public readonly Piece WithSpin(sbyte s) {
            return new Piece(Kind, X, Y, s);
        }
    }
}