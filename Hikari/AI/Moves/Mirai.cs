using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hikari.AI.Utils.Collection;
using Hikari.Puzzle;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Hikari.AI.Moves {
    // Light-weight movement iterator
    public struct Mirai : IDisposable {
        public NativeHashMap<Piece, Step> tree;
        public NativePriorityQueue<StepRef> next;
        public NativeArray<int2x4> pieceCells;
        public UnsafeHashMap<Piece, StepRef> locked;
        public JacksonDunstan.NativeCollections.NativeHashSet<uint> deduplicator;
        public SimpleColBoard board;

        public Mirai(in SimpleColBoard board, in NativeArray<int2x4> pieceCells) {
            this.pieceCells = pieceCells;
            tree = new NativeHashMap<Piece, Step>(500, Allocator.Temp);
            next = new NativePriorityQueue<StepRef>(false, 500, Allocator.Temp);
            locked = new UnsafeHashMap<Piece, StepRef>(100, Allocator.Temp);
            deduplicator = new JacksonDunstan.NativeCollections.NativeHashSet<uint>(100, Allocator.Temp);
            this.board = board;
        }

        public Mirai(in NativeArray<int2x4> pieceCells) : this(default, pieceCells) { }

        public void Reset(in SimpleColBoard board) {
            this.board = board;
            tree.Clear();
            next.Clear();
            locked.Clear();
            deduplicator.Clear();
        }

        public void Generate(Piece spawned) {
            var num = 0;

            // spawned = new Piece(spawned.Kind, spawned.X, (sbyte) math.min(spawned.Y, boardMaxHeight + 3),
            //     spawned.Spin, spawned.Tspin);

            if (tree.ContainsKey(spawned)) return;

            deduplicator.Clear();

            var root = new Step(Piece.Invalid, 0, 0, spawned, Instruction.None);
            tree.Add(spawned, root);
            next.Enqueue(new StepRef(root));

            while (next.TryDequeue(out var ci)) {
                var piece = ci.piece;
                var origin = tree[piece];

                var dropped = board.SonicDrop(piece);

                if (origin.depth < Path.MaxInstructions) {
                    Append(origin, piece.WithOffset(-1, 0), Instruction.Left);
                    Append(origin, piece.WithOffset(1, 0), Instruction.Right);
                    if (spawned.Kind != PieceKind.O) {
                        Append(origin, Rotate(piece, true), Instruction.Cw, true);
                        Append(origin, Rotate(piece, false), Instruction.Ccw, true);
                    }

                    if (dropped.Y != piece.Y) {
                        Append(origin, dropped, Instruction.SonicDrop);
                    }
                }

                if (deduplicator.TryAdd(math.hash(dropped.GetCells(pieceCells)) + (uint) piece.Tspin)) {
                    locked.TryAdd(dropped, ci);
                }
            }
        }

        private void Append(in Step origin, Piece result, Instruction inst, bool skipCheck = false) {
            if (result.IsInvalid || !skipCheck && board.Collides(result)) {
                // tree.TryAdd(result, default);
                return;
            }

            int t;

            if (inst == Instruction.SonicDrop) {
                t = 2 * (origin.piece.Y - result.Y);
                // if (result.Kind != PieceKind.T && origin.cost + t >= 20) return;
            } else {
                t = 1;
            }

            if (origin.inst == inst) {
                t += 1;
            }

            var step = new Step(origin.piece, origin.cost + t, origin.depth + 1, result, inst);

            if (tree.TryAdd(result, step) && step.depth < Path.MaxInstructions) {
                // if (result.Kind == PieceKind.T || step.cost < 20)
                    next.Enqueue(new StepRef(step));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Piece Rotate(in Piece piece, bool cw) {
            var r = SRSNoAlloc.TryRotate(piece, board, cw, out var result);
            if (r >= 0) {
                return result.WithTSpinStatus(board.CheckTSpin(result, r));
            } else {
                return Piece.Invalid;
            }
        }

        [NotBurstCompatible]
        public Path? RebuildPath(Piece to, bool holdUsed) {
            var instructions = new List<Instruction>();
            if (!locked.TryGetValue(to, out var ci)) return null;
            var leaf = ci.piece;
            while (!tree[leaf].parent.IsInvalid) {
                instructions.Add(tree[leaf].inst);
                leaf = tree[leaf].parent;
            }

            instructions.Reverse();

            var path = new Path {
                hold = holdUsed,
                holdOnly = false,
                instructions = instructions.AsReadOnly(),
                result = to,
                time = locked[to].cost
            };

            return path;
        }

        public void Dispose() {
            tree.Dispose();
            next.Dispose();
            locked.Dispose();
        }
    }
}