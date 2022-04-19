using System.Collections.Generic;
using System.Linq;
using Hikari.Utils;
using UnityEngine;

namespace Hikari.Puzzle {
    public class Board {
        public readonly Row[] row = new Row[40];
        public uint ren;
        public bool b2b;
        public PieceKind? holdPiece;
        public readonly Queue<PieceKind> nextPieces = new Queue<PieceKind>();
        public Bag bag = new Bag();
        public Bag initialBag = new Bag();

        private static readonly Vector2Int[][] FullTSpinCheckPoints = {
            new[] {new Vector2Int(0, 0), new Vector2Int(2, 0)},
            new[] {new Vector2Int(0, 2), new Vector2Int(0, 0)},
            new[] {new Vector2Int(2, 2), new Vector2Int(0, 2)},
            new[] {new Vector2Int(2, 0), new Vector2Int(2, 2)}
        };

        private static readonly Vector2Int[][] MiniTSpinCheckPoints = {
            new[] {new Vector2Int(2, 2), new Vector2Int(0, 2)},
            new[] {new Vector2Int(2, 0), new Vector2Int(2, 2)},
            new[] {new Vector2Int(0, 0), new Vector2Int(2, 0)},
            new[] {new Vector2Int(0, 2), new Vector2Int(0, 0)}
        };
        
        private Board() {}

        public Board(int nextQty) {
            for (var i = 0; i < row.Length; i++) {
                row[i] = new Row();
            }

            for (var i = 0; i < nextQty; i++) {
                nextPieces.Enqueue(bag.TakeRandomPiece());
            }
        }

        public Piece SonicDrop(Piece piece) {
            if (Collides(piece)) return piece;
            var prevDrop = piece;
            for (var i = (sbyte) (piece.Y - 1); i >= -3; i--) {
                var drop = new Piece(piece.Kind, piece.X, i, piece.Spin);
                if (Collides(drop)) return prevDrop;
                prevDrop = drop;
            }

            return prevDrop;
        }

        public bool Collides(Piece piece) {
            var shape = piece.GetShape();
            for (var i = 0; i < 4; i++) {
                var y = piece.Y + i;
                var pieceLine = shape[i].Shift(piece.X + 3);
                if (y < 0) {
                    if (pieceLine > 0) return true;
                } else {
                    var fieldLine = (row[y].ToBitFlags() << 3) | 0b111_00000_00000_111;
                    if ((pieceLine & fieldLine) != 0) return true;
                }
            }

            return false;
        }

        public LockResult Lock(in Piece piece) {
            foreach (var position in piece.GetCellPositions()) {
                row[position.y].cells[position.x] = (byte) (piece.Kind + 1);
            }

            var clearLines = new List<int>();
            var pc = true;

            for (var i = 0; i < row.Length; i++) {
                if (row[i].IsFilled) {
                    clearLines.Add(i);
                }

                if (!(row[i].IsEmpty || row[i].IsFilled)) {
                    pc = false;
                }
            }

            var placement = PlacementKindFactory.Create(clearLines.Count, piece.Tspin);

            if (placement.IsLineClear()) ren++;
            else ren = 0;

            var lockResult = new LockResult(placement, pc, ren, clearLines, b2b);
            b2b = lockResult.b2b;
            ren = lockResult.ren;

            initialBag.Take(piece.Kind);
            
            if (lockResult.clearedLines.Any()) {
                var c = 0;
                foreach (var line in lockResult.clearedLines) {
                    for (var i = line - c; i < row.Length - 1; i++) {
                        row[i] = row[i + 1];
                    }

                    c++;
                }

                for (var i = 0; i < lockResult.clearedLines.Count; i++) {
                    row[row.Length - i - 1] = new Row();
                }
            }
            return lockResult;
        }


        public TSpinStatus CheckTSpin(Piece piece, int rotation) {
            if (piece.Kind != PieceKind.T) return TSpinStatus.None;
            var tSpinCheckCount = FullTSpinCheckPoints[piece.Spin]
                .Select(t => t + new Vector2Int(piece.X, piece.Y))
                .Count(pos => pos.x < 0 || 9 < pos.x || pos.y < 0 || row[pos.y].cells[pos.x] > 0);

            var miniTSpinCheckCount = MiniTSpinCheckPoints[piece.Spin]
                .Select(t => t + new Vector2Int(piece.X, piece.Y))
                .Count(pos => pos.x < 0 || 9 < pos.x || pos.y < 0 || row[pos.y].cells[pos.x] > 0);

            if (tSpinCheckCount + miniTSpinCheckCount >= 3) {
                if (rotation == 4) return TSpinStatus.Full;
                else if (miniTSpinCheckCount == 2) return TSpinStatus.Full;
                else return TSpinStatus.Mini;
            } else return TSpinStatus.None;
        }

        public void InsertRowsAtBottom(byte[][] rows) {
            for (var i = row.Length - rows.Length - 1; i >= 0; i--) {
                row[i + rows.Length] = row[i];
            }

            for (var i = 0; i < rows.Length; i++) {
                row[i] = new Row(rows[i]);
            }
        }

        public class Row {
            public readonly byte[] cells;

            public Row() {
                cells = new byte[10];
            }

            public Row(byte[] cells) {
                this.cells = (byte[]) cells.Clone();
            }

            public ushort ToBitFlags() => (ushort) cells.Select((b, i) => b > 0 ? 1 << i : 0).Sum();

            public bool IsFilled => cells.All(b => b != 0);

            public bool IsEmpty => cells.All(b => b == 0);
        }

        public PieceKind Next() {
            nextPieces.Enqueue(bag.TakeRandomPiece());
            return nextPieces.Dequeue();
        }

        public Board Clone() {
            var clone = new Board(nextPieces.Count) {
                b2b = b2b,
                bag = bag,
                holdPiece = holdPiece,
                initialBag = initialBag,
                ren = ren
            };
            for (var i = 0; i < row.Length; i++) {
                clone.row[i] = new Row(row[i].cells);
            }

            foreach (var nextPiece in nextPieces) {
                clone.nextPieces.Enqueue(nextPiece);
            }

            return clone;
        }

        public bool[] ToBoolArray(int yRange) {
            var list = new bool[10 * yRange];
            for (var y = 0; y < row.Length; y++) {
                var r = row[y];
                for (var x = 0; x < 10; x++) {
                    list[x + y * 10] = r.cells[x] != 0;
                }
            }

            return list;
        }
    }
}