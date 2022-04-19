using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Hikari.Puzzle;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Hikari.AI {
    public unsafe struct SimpleBoard : IEquatable<SimpleBoard> {
        public fixed ushort cells[Length];
        public bool backToBack;
        public PieceKind? hold;
        public Bag bag;
        public int ren;

        private const ushort FilledLine = 0b11111_11111;
        public const int Length = 24; // if you change this, you also need to edit equality members

        private static readonly int4[] TspinCheckPoints = {
            new int4(0, 0, 2, 0),
            new int4(0, 2, 0, 0),
            new int4(2, 2, 0, 2),
            new int4(2, 0, 2, 2)
        };

        public SimpleBoard(Board board) {
            fixed (ushort* ptr = cells) {
                UnsafeUtility.MemClear(ptr, sizeof(ushort) * Length);
            }

            for (var i = 0; i < Length; i++) {
                cells[i] = board.row[i].ToBitFlags();
            }

            ren = (int) board.ren;
            backToBack = board.b2b;
            bag = board.initialBag;
            hold = board.holdPiece;
        }

        public ushort Row(int y) {
            if (y < 0 || y >= Length) throw new ArgumentOutOfRangeException(nameof(y));
            return cells[y];
        }

        public NativeArray<ushort> GetCells() {
            var na = new NativeArray<ushort>(Length, Allocator.Temp);
            fixed (ushort* gridPtr = cells) {
                UnsafeUtility.MemCpy(na.GetUnsafePtr(), gridPtr, Length * sizeof(ushort));
            }

            return na;
        }

        public readonly Piece? Spawn(PieceKind pk, in NativeArray<uint4x4> pieceShapes) {
            var piece = new Piece(pk, 3, (sbyte) (pk == PieceKind.I ? 17 : 18), 0);
            if (Collides(piece, pieceShapes)) piece = piece.WithOffset(0, 1);
            return Collides(piece, pieceShapes) ? (Piece?) null : piece;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Collides(uint4 shape, int2 pos) {
            if (pos.y > Length - 3) return false;
            shape <<= pos.x + 3;
            var yVec = new int4(pos.y, pos.y + 1, pos.y + 2, pos.y + 3);
            if (math.any(yVec < 0 & (shape != 0))) return true;
            var clamped = math.max(0, yVec);
            var field = new uint4(cells[clamped.x], cells[clamped.y], cells[clamped.z], cells[clamped.w]) << 3;
            field |= 0b111_00000_00000_111;

            return math.any(field & shape);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Collides(in Piece piece, in NativeArray<uint4x4> pieceShapes) {
            return Collides(pieceShapes[(int) piece.Kind][piece.Spin], new int2(piece.X, piece.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Occupied(int2 at) {
            if (at.x < 0 || 10 <= at.x || at.y < 0) return true;
            return (cells[at.y] & (1 << at.x)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Occupied(int x, int y) {
            if (x < 0 || 10 <= x || y < 0) return true;
            return (cells[y] & (1 << x)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool OccupiedUnbounded([AssumeRange(0, 9)] int x, [AssumeRange(0, Length - 1)] int y) {
            return (cells[y] & (1 << x)) != 0;
        }

        public readonly Piece SonicDrop(in Piece piece, in NativeArray<uint4x4> pieceShapes) {
            var shape = pieceShapes[(int) piece.Kind][piece.Spin];
            if (Collides(shape, new int2(piece.X, piece.Y - 1))) return piece;

            for (var y = (sbyte) (piece.Y - 2); y >= -3; y--) {
                if (Collides(shape, new int2(piece.X, y))) {
                    return new Piece(piece.Kind, piece.X, (sbyte) (y + 1), piece.Spin);
                }
            }

            return Piece.Invalid;
        }

        public readonly void GetColumns([NoAlias] int* columns, [NoAlias] byte* cMaxHeights) {
            UnsafeUtility.MemClear(columns, sizeof(int) * 10);
            UnsafeUtility.MemClear(cMaxHeights, sizeof(byte) * 10);
            for (byte x = 0; x < 10; x++) {
                for (byte y = 0; y < Length; y++) {
                    if ((cells[y] & (1 << x)) > 0) {
                        cMaxHeights[x] = y;
                        columns[x] |= 1 << y;
                    }
                }
            }
        }

        public readonly int GetMaxHeight() {
            for (var i = Length - 1; i >= 0; i--) {
                if (cells[i] != 0) return i + 1;
            }

            return 0;
        }

        public readonly TSpinStatus CheckTSpin(Piece piece, int rotation) {
            if (piece.Kind != PieceKind.T) return TSpinStatus.None;
            var tSpinCheckCount = 0;
            var pos = new int2(piece.X, piece.Y);
            if (Occupied(TspinCheckPoints[piece.Spin].xy + pos)) {
                tSpinCheckCount++;
            }

            if (Occupied(TspinCheckPoints[piece.Spin].zw + pos)) {
                tSpinCheckCount++;
            }

            var miniTSpinCheckCount = 0;
            var invert = (piece.Spin + 2) & 0b11;
            if (Occupied(TspinCheckPoints[invert].xy + pos)) {
                miniTSpinCheckCount++;
            }

            if (Occupied(TspinCheckPoints[invert].zw + pos)) {
                miniTSpinCheckCount++;
            }


            if (tSpinCheckCount + miniTSpinCheckCount >= 3) {
                if (rotation == 4) return TSpinStatus.Full;
                else if (miniTSpinCheckCount == 2) return TSpinStatus.Full;
                else return TSpinStatus.Mini;
            } else return TSpinStatus.None;
        }

        public SimpleLockResult LockSelf(in Piece piece, in NativeArray<uint4x4> pieceShapes) {
            var shape = math.rol(pieceShapes[(int) piece.Kind][piece.Spin], piece.X);
            var clearedLines = new int4();
            var count = 0;
            var bottommostRow = true;
            var death = false;
            
            for (var i = 0; i < 4; i++) {
                if (shape[i] == 0) continue;
                
                var y = i + piece.Y;

                if (bottommostRow) {
                    if (y > 20) {
                        death = true;
                    }

                    bottommostRow = false;
                }
                
                if (y >= Length) break;

                cells[y] = (ushort) (cells[y] | shape[i]);

                if (cells[y] == FilledLine) {
                    clearedLines[count++] = y;
                }
            }

            for (var i = 0; i < count; i++) {
                for (var j = clearedLines[i] - i; j < Length - 1; j++) {
                    cells[j] = cells[j + 1];
                }

                cells[Length - 1 - i] = 0;
            }

            var pc = true;
            for (var i = 0; i < Length; i++) {
                if (cells[i] != 0) pc = false;
            }

            var placementKind = PlacementKindFactory.Create(count, piece.Tspin);

            ren = placementKind.IsLineClear() ? ren + 1 : 0;
            backToBack = placementKind.IsLineClear() ? placementKind.IsContinuous() : backToBack;

            death |= ((cells[20] | cells[21]) & 0b0001111000) != 0;

            return new SimpleLockResult(placementKind, death, backToBack, pc,
                placementKind.IsLineClear() ? ren + 1 : 0);
        }

        public PieceKind? HoldSelf(PieceKind newHold) {
            var unHold = hold;
            hold = newHold;
            return unHold;
        }

        public readonly bool Equals(SimpleBoard other) {
            // if (backToBack != other.backToBack || hold != other.hold || ren != other.ren) return false;
            // for (var i = 0; i < Length; i++) {
            //     if (cells[i] != other.cells[i]) return false;
            // }
            //
            // return true;

            fixed (SimpleBoard* ptr = &this) {
                var cmp = UnsafeUtility.MemCmp(ptr, &other, sizeof(SimpleBoard));
                return cmp == 0;
            }
        }

        public override readonly bool Equals(object obj) {
            return obj is SimpleBoard other && Equals(other);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode() {
            fixed (ushort* ptr = cells) {
                unchecked {
                    var hashCode = unchecked((int) math.hash(*(uint4x3*) ptr));
                    hashCode = (hashCode * 397) ^ backToBack.GetHashCode();
                    hashCode = (hashCode * 397) ^ UnsafeUtility.As<PieceKind?, ushort>(ref hold);
                    hashCode = (hashCode * 397) ^ bag.GetHashCode();
                    hashCode = (hashCode * 397) ^ ren;
                    return hashCode;
                }
            }
        }

        public static bool operator ==(SimpleBoard left, SimpleBoard right) {
            return left.Equals(right);
        }

        public static bool operator !=(SimpleBoard left, SimpleBoard right) {
            return !left.Equals(right);
        }

        public override string ToString() {
            var sb = new StringBuilder(@$"SimpleBoard
Hold:{hold.ToString()}
Ren: {ren.ToString()} B2B: {backToBack.ToString()}
Bag: {bag.ToString()}");
            for (var y = Length - 1; y >= 0; y--) {
                sb.Append('\n');
                for (var x = 0; x < 10; x++) {
                    sb.Append(OccupiedUnbounded(x, y) ? '#' : ' ');
                }
            }

            return sb.ToString();
        }

        public ushort[] ToArray() {
            var array = new ushort[Length];
            for (var i = 0; i < array.Length; i++) {
                array[i] = cells[i];
            }

            return array;
        }
    }
}