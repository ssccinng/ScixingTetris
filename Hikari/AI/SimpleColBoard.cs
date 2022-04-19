using System;
using System.Runtime.CompilerServices;
using Hikari.Puzzle;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Hikari.AI {
    public unsafe struct SimpleColBoard {
        internal fixed uint columns[16];
        public bool backToBack;
        public PieceKind? hold;
        public Bag bag;
        public int ren;

        [NativeDisableUnsafePtrRestriction] private uint4* pieceColShapes;

        public SimpleColBoard(NativeArray<uint4x4> pieceVShapes) {
            pieceColShapes = (uint4*)pieceVShapes.GetUnsafeReadOnlyPtr();
            backToBack = false;
            hold = null;
            ren = 0;
            bag = new Bag();

            FillSideWalls();
        }

        public SimpleColBoard(in Board rowBoard, NativeArray<uint4x4> pieceVShapes) {
            pieceColShapes = (uint4*)pieceVShapes.GetUnsafeReadOnlyPtr();
            backToBack = rowBoard.b2b;
            hold = rowBoard.holdPiece;
            ren = (int)rowBoard.ren;
            bag = rowBoard.bag;

            FillSideWalls();

            for (var x = 0; x < 10; x++) {
                for (var y = 0; y < 32; y++) {
                    if (rowBoard.row[y].cells[x] != 0) {
                        columns[x + 3] |= 1U << y;
                    }
                }
            }
        }

        private void FillSideWalls() {
            columns[0] = uint.MaxValue;
            columns[1] = uint.MaxValue;
            columns[2] = uint.MaxValue;

            for (var x = 0; x < 10; x++) {
                columns[x + 3] = 0;
            }

            columns[13] = uint.MaxValue;
            columns[14] = uint.MaxValue;
            columns[15] = uint.MaxValue;
        }

        public SimpleBoard ToRowBoard() {
            var rowBoard = new SimpleBoard {
                ren = ren,
                backToBack = backToBack,
                bag = bag,
                hold = hold,
            };

            for (var x = 0; x < 10; x++) {
                for (var y = 0; y < SimpleBoard.Length; y++) {
                    if (OccupiedUnbounded(x, y)) {
                        rowBoard.cells[y] |= (ushort)(1U << x);
                    }
                }
            }

            return rowBoard;
        }

        public readonly ref uint4 Slice4(int x) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (x < -3 || 10 <= x) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }
#endif
            fixed (uint* ptr = columns) {
                return ref UnsafeUtility.AsRef<uint4>((uint4*)(ptr + x + 3));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int MaxHeight() {
            var c = 0U;
            for (var x = 0; x < 10; x++) {
                c |= columns[x + 3];
            }

            return 32 - math.lzcnt(c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int Height(int x) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (x < 0 || 10 <= x) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }
#endif

            return 32 - math.lzcnt(columns[x + 3]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint Row(int y) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (y < 0 || y >= 32) {
                throw new ArgumentOutOfRangeException();
            }
#endif
            var l = 0U;
            for (var i = 0; i < 10; i++) {
                l |= ((columns[i + 3] >> y) & 1U) << i;
            }

            return l;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint Column(int x) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (x < 0 || x >= 10) {
                throw new ArgumentOutOfRangeException();
            }
#endif
            return columns[x + 3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint ColumnWide(int x) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (x < -3 || x >= 13) {
                throw new ArgumentOutOfRangeException();
            }
#endif
            return columns[x + 3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint4 Mask(in uint4 vShape, int x, int y) {
            var moved = math.rol(vShape, y);
            return Slice4(x) & moved;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool CollidesAny(in uint4 vShape, int x, int y) {
            var moved = math.rol(vShape, y);
            ref var slice = ref Slice4(x);
            return math.any((moved & (0b1111U << 28)) | (slice & moved));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool CollidesAll(in uint4 vShape, int x, int y) {
            var moved = math.rol(vShape, y);
            return math.all((moved & (0b1111U << 28)) | (Slice4(x) & moved));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Occupied(int2 at) {
            if (at.x < 0 || 10 <= at.x || at.y < 0) return true;
            return (columns[at.x + 3] & (1 << at.y)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Occupied(int x, int y) {
            if (x < 0 || 10 <= x || y < 0) return true;
            return (columns[x + 3] & (1 << y)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool OccupiedUnbounded(int x, int y) {
            return (columns[x + 3] & (1 << y)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Collides(in Piece p) {
            return CollidesAny(GetPieceShape(p), p.X, p.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Piece SonicDrop(in Piece p) {
            ref readonly var vShape = ref GetPieceShape(p);
            ref var cols = ref Slice4(p.X);

            var belowPiece = math.tzcnt(vShape) + p.Y;
            var v1 = (1U << math.cmax(belowPiece % 32)) - 1;
            var v2 = math.rol(vShape, p.Y);
            var terrainMask = v1 | v2;
            var masked = cols & terrainMask;
            var aboveTerrain = math.lzcnt(masked);
            var drop = math.cmin(belowPiece + aboveTerrain) - 32;

            return new Piece(p.Kind, p.X, p.Y - drop, p.Spin, drop == 0 ? p.Tspin : TSpinStatus.None);

            // var v = new int4();
            // var belowPiece = math.tzcnt(vShape) + p.Y;
            // for (var i = 0; i < 4; i++) {
            //     if (vShape[i] != 0) {
            //         v[i] = math.lzcnt(~(~cols[i] << (32 - belowPiece[i])));
            //     } else {
            //         v[i] = 32;
            //     }
            // }
            //
            // var drop = math.cmin(v);
            // return new Piece(p.Kind, p.X, p.Y - drop, p.Spin, drop == 0 ? p.Tspin : TSpinStatus.None);
        }

        private static readonly int4[] TspinCheckPoints = {
            new int4(0, 0, 2, 0),
            new int4(0, 2, 0, 0),
            new int4(2, 2, 0, 2),
            new int4(2, 0, 2, 2)
        };

        public readonly TSpinStatus CheckTSpin(in Piece piece, int rotation) {
            if (piece.Kind != PieceKind.T) return TSpinStatus.None;
            var tSpinCheckCount = 0;
            var pos = new int2(piece.X, piece.Y);

            if (Occupied(TspinCheckPoints[piece.Spin].xy + pos)) tSpinCheckCount++;
            if (Occupied(TspinCheckPoints[piece.Spin].zw + pos)) tSpinCheckCount++;

            var miniTSpinCheckCount = 0;
            var invert = (piece.Spin + 2) & 0b11;
            if (Occupied(TspinCheckPoints[invert].xy + pos)) miniTSpinCheckCount++;
            if (Occupied(TspinCheckPoints[invert].zw + pos)) miniTSpinCheckCount++;


            if (tSpinCheckCount + miniTSpinCheckCount >= 3) {
                if (rotation == 4) return TSpinStatus.Full;
                else if (miniTSpinCheckCount == 2) return TSpinStatus.Full;
                else return TSpinStatus.Mini;
            } else return TSpinStatus.None;
        }

        public readonly Piece? Spawn(PieceKind pk) {
            var piece = new Piece(pk, 3, (sbyte)(pk == PieceKind.I ? 17 : 18), 0);
            if (Collides(piece)) piece = piece.WithOffset(0, 1);
            return Collides(piece) ? (Piece?)null : piece;
        }

        public SimpleLockResult LockSelf(in Piece p) {
            ref var targetCols = ref Slice4(p.X);
            var shape = math.rol(GetPieceShape(p), p.Y);

#if UNITY_EDITOR
            if (math.any(targetCols & shape)) {
                Debug.LogWarning("Piece is already locked by another piece.");
            }
#endif

            targetCols |= shape;

            var filled = uint.MaxValue;
            for (var x = 0; x < 10; x++) {
                filled &= columns[x + 3];
            }

            // reflect line clear result to board state
            if (filled != 0) {
                for (var x = 0; x < 10; x++) {
                    columns[x + 3] = X86.Bmi2.pext_u32(columns[x + 3], ~filled);
                }
            }

            var placementKind = PlacementKindFactory.Create(math.countbits(filled), p.Tspin);

            var center = columns[6] | columns[7] | columns[8] | columns[9];
            var death = (center & (0b11U << 20)) != 0;

            ren = placementKind.IsLineClear() ? ren + 1 : 0;

            var maxHeight = MaxHeight();

            return new SimpleLockResult(placementKind, death, backToBack, maxHeight == 0, ren);
        }

        public PieceKind? HoldSelf(PieceKind newHold) {
            var unHold = hold;
            hold = newHold;
            return unHold;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly ref readonly uint4 GetPieceShape(in Piece p) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (pieceColShapes == null) {
                throw new InvalidOperationException();
            }
#endif
            return ref pieceColShapes[(int)p.Kind * 4 + p.Spin];
        }

        public uint* GetColumnsPtr() {
            fixed (uint* ptr = columns) {
                return ptr + 3;
            }
        }
    }
}