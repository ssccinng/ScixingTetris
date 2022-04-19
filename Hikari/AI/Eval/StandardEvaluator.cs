using System.Runtime.CompilerServices;
using Hikari.Puzzle;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Hikari.AI.Eval {
    public struct StandardEvaluator : IEvaluator {
        public Value EvaluateBoard([NoAlias] in SimpleColBoard board, [NoAlias] in Weights w) {
            var clone = board;
            var fieldSafety = 0;
            var fieldPower = 0;
            var maxHeight = clone.MaxHeight();

            var prevWell = LocateWell(clone).x;

            if (!w.noTspin && maxHeight < 15) {
                var tPieceCount = math.select(0, 1, clone.bag.Contains(PieceKind.T)) +
                                  math.select(0, 1, clone.hold == PieceKind.T) +
                                  math.select(0, 1, clone.bag.Count <= 3);
                for (var i = 0; i < tPieceCount; i++) {
                    var tHole = ScanTspin(clone);
                    if (tHole != null) {
                        fieldPower += tHole.Value.line * w.tHole;
                        AfterSpin(ref clone, tHole.Value);
                        maxHeight -= tHole.Value.line;
                        continue;
                    }

                    var tst = ScanTST(clone);
                    if (tst != null) {
                        fieldPower += tst.Value.line * w.tstHole;
                        AfterSpin(ref clone, tst.Value);
                        maxHeight -= tst.Value.line;
                        continue;
                    }

                    break;
                }
            }

            // board.GetColumns(columns, maxHeights);

            var wellInfo = LocateWell(clone);
            var wellX = wellInfo.x;
            var wellDepth = wellInfo.y;

            fieldSafety += math.csum(Bumpiness(clone, wellX) * new int2(w.bumpSum, w.bumpSumSq));

            fieldPower += math.min(wellDepth, 6) * w.wellDepth;
            if (wellDepth >= 2) {
                fieldPower += w.wellX[wellX];
            }

            if (0 < wellX && wellX < 9) {
                fieldPower += math.abs(clone.Height(wellX - 1) - clone.Height(wellX + 1)) switch {
                    0 => w.holeEdgeDiff0,
                    1 => w.holeEdgeDiff1,
                    2 => w.holeEdgeDiff2,
                    _ => w.holeEdgeDiffMany
                };
            }

            if (prevWell != wellX) {
                fieldPower += w.donate;
            }

            fieldSafety += maxHeight * w.maxHeight;
            fieldSafety += math.max(maxHeight - 10, 0) * w.top50;
            fieldSafety += math.max(maxHeight - 15, 0) * w.top75;

            var holes = Holes(clone);
            fieldSafety += holes * w.holes;
            fieldSafety += holes * holes * w.holesSq;

            var cover = Cover(clone);
            fieldSafety += cover * w.coveredCells;
            fieldSafety += cover * cover * w.coveredCellsSq;

            fieldSafety += RowTransitions(clone) * w.rowTransitions;
            fieldSafety += ColTransitions(clone) * w.colTransitions;

            return new Value {
                value = new int4(fieldSafety, fieldPower, 0, 0),
                spike = 0
            };
        }

        public Reward EvaluateMove(int time, Piece piece,
            [NoAlias] in SimpleColBoard board, [NoAlias] in SimpleLockResult lr, bool parentIsB2B,
            [NoAlias] in Weights w) {
            var moveScore = 0;
            if (!piece.IsInvalid) {
                if (!lr.perfectClear && lr.placementKind.IsLineClear()) time += 40;
                moveScore += time * w.moveTime;

                var maxDangerHeight = math.cmax(32 - math.lzcnt(board.Slice4(3)));
                moveScore += math.max(maxDangerHeight - 10, 0) * time * w.danger;

                if (lr.placementKind.IsLineClear() && lr.backToBack) moveScore += w.b2bContinue;
                if (parentIsB2B && !lr.backToBack) moveScore += w.b2bDestroy;

                moveScore += piece.Y * w.placementHeight;

                if (lr.perfectClear) {
                    moveScore += w.perfect;
                } else {
                    var renAttack = Game.GetRenAttack(lr.ren - 1);
                    moveScore += w.ren * renAttack * renAttack;
                    moveScore += lr.placementKind switch {
                        PlacementKind.Clear1 => w.clear1,
                        PlacementKind.Clear2 => w.clear2,
                        PlacementKind.Clear3 => w.clear3,
                        PlacementKind.Clear4 => w.clear4,
                        PlacementKind.Mini1 => w.tMini1,
                        PlacementKind.Mini2 => w.tMini2,
                        PlacementKind.TSpin1 => w.tSpin1,
                        PlacementKind.TSpin2 => w.tSpin2,
                        PlacementKind.TSpin3 => w.tSpin3,
                        _ => 0
                    };
                }

                if (piece.Kind == PieceKind.T) {
                    if (!lr.placementKind.IsFullTspinClear()) {
                        moveScore += w.wastedT;
                        // } else {
                        //     Debug.Log("TS");
                    }
                }
            }

            if (board.hold == PieceKind.T && !lr.placementKind.IsContinuous()) {
                moveScore += w.holdT;
            }

            return new Reward {
                evaluation = moveScore,
                attack = lr.GetAttack()
            };
        }

        private static int2 LocateWell(in SimpleColBoard board) {
            var well = 0;
            var max = board.Height(well);
            for (var x = 1; x < 10; x++) {
                var height = board.Height(x);
                if (height < max) {
                    well = x;
                    max = height;
                }
            }

            var combined = uint.MaxValue;
            for (var x = 0; x < 10; x++) {
                if (x != well) {
                    combined &= board.Column(x);
                }
            }

            var start = 32 - math.lzcnt(combined);
            var last = 32 - math.lzcnt(~combined & ((1U << start) - 1));
            var depth = last == max ? start - last : 0;

            return new int2(well, depth);
        }

        private static int2 Bumpiness(in SimpleColBoard board, int well) {
            var bumpiness = -1;
            var bumpinessSq = -1;

            var prev = board.Height(well == 0 ? 1 : 0);

            for (var x = 0; x < 10; x++) {
                if (x == well) continue;

                var height = board.Height(x);
                if (prev >= 0) {
                    var dh = math.abs(prev - height);
                    bumpiness += dh;
                    bumpinessSq += dh * dh;
                }

                prev = height;
            }

            return new int2(math.abs(bumpiness), math.abs(bumpinessSq));
        }

        private static int Holes(in SimpleColBoard board) {
            var count = 0;
            for (var x = 0; x < 10; x++) {
                var c = board.Column(x);
                var height = 32 - math.lzcnt(c);
                var mask = (1U << height) - 1U;
                var holes = ~c & mask;
                count += math.countbits(holes);
            }

            return count;
        }

        private static int Cover(in SimpleColBoard board) {
            var count = 0;
            for (var x = 0; x < 10; x++) {
                var c = board.Column(x);
                var height = 32 - math.lzcnt(c);
                var mask = (1U << height) - 1U;
                var holes = ~c & mask;

                while (holes != 0) {
                    var len = math.tzcnt(holes);
                    count += math.min(height - len, 6);
                    holes &= ~(1U << len);
                }
            }

            return count;
        }

        private static int RowTransitions(in SimpleColBoard board) {
            var count = 0;
            var prev = board.Column(0);
            for (var x = 1; x < 10; x++) {
                var c = board.Column(x);
                var trans = c ^ prev;
                count += math.countbits(trans);

                prev = c;
            }

            return count;
        }

        private static int ColTransitions(in SimpleColBoard board) {
            var count = 0;
            for (var x = 0; x < 10; x++) {
                var c = board.Column(x);
                var trans = c ^ ((c << 1) | 1U);
                count += math.countbits(trans) - 1;
            }

            return count;
        }

        internal static TspinHole? ScanTspin(in SimpleColBoard board) {
            static void Check(in SimpleColBoard b, int x, int y, ref TspinHole? spinHole) {
                var l = 0U;
                for (var i = 0; i < 10; i++) {
                    l |= ((b.Column(i) >> y) & 0b11U) << (2 * i);
                }

                var lines = 0;
                if (math.countbits(l & 0b_01_01_01_01_01_01_01_01_01_01) == 9) lines++;
                if (math.countbits(l & 0b_10_10_10_10_10_10_10_10_10_10) == 7) lines++;

                if (lines != 0 && (spinHole == null || spinHole.Value.line < lines)) {
                    spinHole = new TspinHole(lines, x, y, 2);
                }
            }

            var tShapeV = new uint4(0b010, 0b011, 0b010, 0);

            TspinHole? best = null;

            for (var x = 0; x < 10; x++) {
                var y = board.Height(x);

                if (y == 0) continue;

                if (x <= 7
                    && board.Height(x + 1) < y
                    && !board.CollidesAny(tShapeV, x, y - 1)
                    && ((board.Column(x + 2) >> (y - 1)) & 0b111) == 0b101) {
                    Check(board, x, y - 1, ref best);
                    continue;
                }

                if (x >= 2
                    && board.Height(x - 1) < y
                    && !board.CollidesAny(tShapeV, x - 2, y - 1)
                    && ((board.Column(x - 2) >> (y - 1)) & 0b111) == 0b101) {
                    Check(board, x - 2, y - 1, ref best);
                }
            }

            return best;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TspinHole? ScanTST(in SimpleColBoard board) {
            static void Check(in SimpleColBoard b, int x, int y, int r, ref TspinHole? spinHole) {
                var l = 0U;
                for (var i = 0; i < 10; i++) {
                    l |= ((b.Column(i) >> y) & 0b111U) << (3 * i);
                }

                var filled = math.csum(math.countbits(b.Mask(new uint4(0b101, 0, 0b101, 0), x, y)));
                if (filled < 3) return;

                var lines = 0;
                if (math.countbits(l & 0b_001_001_001_001_001_001_001_001_001_001U) == 9) lines++;
                if (math.countbits(l & 0b_010_010_010_010_010_010_010_010_010_010U) == 8) lines++;
                if (math.countbits(l & 0b_100_100_100_100_100_100_100_100_100_100U) == 9) lines++;

                if (lines == 0) return;

                if (spinHole == null || spinHole.Value.line < lines) {
                    spinHole = new TspinHole(lines, x, y, r);
                }
            }

            // []    {}
            // ......{}
            // ..[]
            // ....
            // ..
            var slotL = new uint4(0b1111U, 0b1010U, 0b1000U, 0U);
            // {}    []
            // {}......
            //     []..    
            //     ....
            //       ..
            var slotR = new uint4(0b1000U, 0b1010U, 0b1111U, 0U);

            // scan for tst shape
            TspinHole? best = null;

            for (var x = 1; x < 9; x++) {
                var y = board.Height(x) - 3;

                if (y < 0) continue;

                if (board.Height(x + 1) <= y + 3
                    && !board.CollidesAny(slotL, x - 1, y) // slot
                    && board.OccupiedUnbounded(x - 1, y + 4) // anchor
                    && board.OccupiedUnbounded(x + 2, y + 3) == board.OccupiedUnbounded(x + 2, y + 4) // guide
                ) {
                    Check(board, x - 2, y, 1, ref best);
                    continue;
                }

                if (board.Height(x - 1) <= y + 3
                    && !board.CollidesAny(slotR, x - 1, y) // slot
                    && board.OccupiedUnbounded(x + 1, y + 4) // anchor
                    && board.OccupiedUnbounded(x - 2, y + 3) == board.OccupiedUnbounded(x - 2, y + 4) // guide
                ) {
                    Check(board, x, y, 3, ref best);
                    continue;
                }
            }

            return best;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AfterSpin(ref SimpleColBoard board, TspinHole ts) {
            var piece = new Piece(PieceKind.T, (sbyte)ts.x, (sbyte)ts.y, (sbyte)ts.r, TSpinStatus.Full);
            // arbitrary place T
            var lr = board.LockSelf(piece);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (((int)lr.placementKind & 0b111) != ts.line) {
                Debug.LogWarning("!?");
            }
#endif
        }
    }
}