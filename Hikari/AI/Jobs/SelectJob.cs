using System;
using System.Runtime.CompilerServices;
using Hikari.AI.Documents;
using Hikari.AI.Graph;
using Hikari.AI.Utils.Throws;
using Hikari.Puzzle;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Hikari.AI.Jobs {
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast, OptimizeFor = OptimizeFor.Performance)]
    public struct SelectJob : IJobParallelFor {
        [NativeDisableContainerSafetyRestriction] public NativeList<GraphGroup> graph;
        [ReadOnly] public NativeArray<PieceKind> queue;
        [ReadOnly] public NativeArray<Random> rands;
        [ReadOnly] public NativeArray<Piece> selectGuidance;
        [WriteOnly] public NativeArray<Selected> selectedList;
        public SimpleColBoard rootBoard;
        public int rootIndex;

        public void Execute(int index) {
            selectedList[index] = Select(index);
        }

        private Selected Select(int index) {
            var board = rootBoard;
            var rng = rands[index];

            var sel = rootIndex;
            var canHold = true;
            ref var group = ref graph.ElementAt(0);
            ref var current = ref group.GetNode(rootIndex);
            if (current.death) return default;

            // using var debug = new NativeList<Piece>(16, Allocator.Temp);
            for (var depth = 0; depth < graph.Length; depth++) {
                if (!graph[depth].Any()) break;

                if (current.IsLeaf) {
                    var piece = depth < queue.Length ? queue[depth].ToFlags() : board.bag;
                    if (piece == 0) piece = PieceFlags.All;
                    return new Selected(canHold, piece, new GraphNodePtr(depth, sel), board);
                }

                var picked = -1;
                PieceKind pieceUsed = default;
                if (index == 0 && depth < selectGuidance.Length) {
                    picked = Guided(current, depth, out pieceUsed);
                }

                if (picked < 0) {
                    picked = Choose(current, depth, ref rng, out pieceUsed);
                }

#if !HIKARI_DANGER
                if (depth < queue.Length && pieceUsed != queue[depth]) {
                    throw new Exception("Piece inconsistency detected");
                }
#endif

                var child = current.children.GetSpeculated(group, pieceUsed)[picked];
                sel = child.node;
                // debug.Add(child.piece);
                canHold = !child.piece.IsInvalid;
                UpdateBoard(ref board, child, pieceUsed);
                group = ref graph.ElementAt(depth + 1);
                current = ref group.GetNode(sel);
            }

            return default;
        }

        /// <summary>
        /// Choose one of the child nodes of current node.
        /// </summary>
        /// <param name="node">Current node.</param>
        /// <param name="depth"></param>
        /// <param name="rng">Random number generator.</param>
        /// <param name="picked">The piece used.</param>
        /// <returns>The chosen child index.</returns>
        private unsafe int Choose([NoAlias] in GraphNode node, int depth, [NoAlias] ref Random rng,
            [NoAlias] out PieceKind picked) {
            ref var currentGroup = ref graph.ElementAt(depth);
            // ref var node = ref currentGroup.GetNode(nodeIndex);
            ref var childGroup = ref graph.ElementAt(depth + 1);
            var spec = node.children.Speculation;
            picked = PickPossiblePieceKind(spec, ref rng);

            var weights = stackalloc float[spec.GetLengthOf(picked)];
            var children = node.children.GetSpeculated(currentGroup, picked);

            var multiplier = new int4(1, 1, 1, 1);
            var min = (float) long.MaxValue;
            for (var i = 0; i < children.length; i++) {
                var a = (float) math.csum(children[i].Eval(childGroup).value * multiplier);
                weights[i] = a;
                if (a < min) min = a;
            }

            var sum = 0f;
            for (var i = 0; i < children.length; i++) {
                if (children[i].Death(childGroup)) {
                    weights[i] = 0;
                } else {
                    weights[i] = weights[i] - min + 1;
                    weights[i] = weights[i] * weights[i] / ((i + 1) * (i + 1));
                    sum += weights[i];
                }
            }

            if (sum <= math.EPSILON) {
                return 0;
            }

            var rand = rng.NextFloat(0f, sum);
            var val = 0f;

            for (var i = 0; i < children.length; i++) {
                val += weights[i];
                if (val > rand) {
                    return i;
                }
            }

            // Fallback
            // This will likely never happen, but may happen due to float precision loss
            return 0;
        }

        private int Guided([NoAlias] in GraphNode node, int depth, [NoAlias] out PieceKind picked) {
            ref var currentGroup = ref graph.ElementAt(depth);
            picked = node.children.Speculation.GetKnownPiece();

            var children = node.children.GetSpeculated(currentGroup, picked);

            for (var i = 0; i < children.length; i++) {
                if (children[i].piece == selectGuidance[depth]) return i;
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PieceKind PickPossiblePieceKind(SpeculationInfo spec,
            [NoAlias] ref Random rng) {
            if (spec.IsKnown()) {
                // Known
                return spec.GetKnownPiece();
            } else {
                // Speculated
                // first, pick one of the next possible pieces randomly
                // we can use simple rand since the possibilities of each pieces are same
                var s = rng.NextInt(0, spec.CountPossiblePieces());
                for (var i = 0; i < 7; i++) {
                    if (spec.Has(i)) {
                        if (s-- == 0) {
                            return (PieceKind) i;
                        }
                    }
                }

                throw new UnreachableException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateBoard([NoAlias] ref SimpleColBoard board, [NoAlias] in GraphChild child,
            PieceKind currentPiece) {
            board.bag.Take(currentPiece);
            if (child.piece.IsInvalid) {
                var unHold = board.HoldSelf(currentPiece);
                if (unHold != null) throw new SpeculationBrokeException();
            } else if (child.piece.Kind == currentPiece) {
                board.LockSelf(child.piece);
            } else {
                var unHold = board.HoldSelf(currentPiece);
                if (unHold != child.piece.Kind) throw new SpeculationBrokeException();
                board.LockSelf(child.piece);
            }
        }
    }
}