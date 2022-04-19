using System;
using System.Runtime.CompilerServices;
using Hikari.AI.Documents;
using Hikari.AI.Graph;
using Hikari.AI.Moves;
using Hikari.Puzzle;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct ExpandJob : IJobParallelFor {
        [ReadOnly] public NativeArray<Selected> selections;
        [ReadOnly] public NativeArray<int2x4> pieceCells;
        [NativeDisableContainerSafetyRestriction] public NativeList<GraphGroup> graph;
        public NativeMultiHashMap<GraphNodePtr, Expanded>.ParallelWriter outputWriter;
        public bool useHold;

        public void Execute(int index) {
            var sel = selections[index];
            if (!sel.valid) return;
            if (sel.candidates == 0) Debug.LogError("no piece");

            var board = sel.board;
            ref var node = ref graph.Actualize(sel.node);
            var childrenSpec = new SpeculationInfo();
            var mirai = new Mirai(pieceCells);

            var canHold = useHold && sel.canHold;
            var childBuffer = new NativeList<GraphChild>(
                math.select(64, 128, canHold) * math.countbits((uint) sel.candidates), Allocator.Temp);
            var death = false;
            var prevBufferLength = 0;
            mirai.Reset(board);
            for (var i = 0; i < 7; i++) {
                if (!sel.candidates.Contains(i)) continue;

                var currentPossiblePiece = (PieceKind) i;

                Expand(sel.node, board, currentPossiblePiece, false, ref mirai, ref childBuffer);

                if (canHold) {
                    if (board.hold.HasValue) {
                        if (board.hold.Value != currentPossiblePiece) {
                            var afterHold = board;
                            afterHold.HoldSelf(currentPossiblePiece);
                            Expand(sel.node, afterHold, board.hold.Value, true, ref mirai, ref childBuffer);
                        }
                    } else {
                        var clone = board;
                        var unHold = clone.HoldSelf(currentPossiblePiece);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                        if (unHold.HasValue) throw new Exception();
#endif
                        WriteBuffer(ref childBuffer, sel.node, clone, default, 3, Piece.Invalid, board.backToBack);
                    }
                }

                if (childBuffer.Length - prevBufferLength > ushort.MaxValue) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    Debug.LogWarning($"Too many children {childBuffer.Length - prevBufferLength}");
#endif
                    childBuffer.ResizeUninitialized(prevBufferLength + ushort.MaxValue);
                } else if (childBuffer.Length == prevBufferLength) {
                    death = true;
                    break;
                }

                childrenSpec.Set(currentPossiblePiece, (ushort) (childBuffer.Length - prevBufferLength));
                prevBufferLength = childBuffer.Length;
            }

            ref var group = ref graph.ElementAt(sel.node.groupIndex);
            if (death) {
                node.death = true;
            } else {
                var start = group.AddChildren(childBuffer.SliceView());
                node.children = new GraphChildren(childrenSpec, start);
            }

            childBuffer.Dispose();
            mirai.Dispose();
        }

        private void Expand(GraphNodePtr origin, in SimpleColBoard board, PieceKind pk, bool hold,
            ref Mirai mirai, ref NativeList<GraphChild> childBuffer) {
            var spawn = board.Spawn(pk);
            if (!spawn.HasValue) return;

            mirai.Generate(spawn.Value);

            using var enumerator = mirai.locked.GetEnumerator();

            while (enumerator.MoveNext()) {
                enumerator.Current.GetKeyValue(out var drop, out var stepRef);
                
                if (drop.Kind != pk) continue;
                
                var clone = board;
                var lr = clone.LockSelf(drop);
                WriteBuffer(ref childBuffer, origin, clone, lr, stepRef.cost + (hold ? 1 : 0), drop, board.backToBack);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteBuffer(ref NativeList<GraphChild> childBuffer, GraphNodePtr origin, in SimpleColBoard board,
            in SimpleLockResult lr, int time, Piece piece, bool parentIsB2B) {
            if (lr.death) return;
            ref var group = ref graph.ElementAt(origin.groupIndex + 1);
            var nodeIndex = group.AddNode(new GraphNode(default, origin.nodeIndex), board, piece.IsInvalid);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (nodeIndex < 0) throw new Exception("AddNode failed");
#endif
            outputWriter.Add(origin, new Expanded(childBuffer.Length, time, piece, board, lr, parentIsB2B));
            childBuffer.Add(new GraphChild(nodeIndex, piece));
        }
    }
}