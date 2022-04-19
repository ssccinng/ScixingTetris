using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hikari.AI.Documents;
using Hikari.AI.Eval;
using Hikari.AI.Graph;
using Hikari.AI.Jobs;
using Hikari.AI.Moves;
using Hikari.AI.Utils.Throws;
using Hikari.Puzzle;

using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;
using Random = Unity.Mathematics.Random;

namespace Hikari.AI {
    public sealed class HikariAI2 : IDisposable {
        private NativeList<GraphGroup> graph;
        private SimpleColBoard board;
        private int rootIndex;
        private bool dead;
        private bool disposed;
        private NativeList<PieceKind> nextPieces;
        private readonly Queue<PieceKind> nextPiecesToAdd = new Queue<PieceKind>();
        private Board resetBoard;
        private bool moveProvided;
        public List<Piece> CurrentPlan { get; } = new List<Piece>();
        private Path? pathForCurrentPlan;
        private Piece? lastPlan;
        private Value lastEval;
        private int lastNodes;
        private Stopwatch calculationTimeStopwatch = new Stopwatch();
        private readonly object moveLock = new object();

        private NativeArray<uint4x4> pieceVShapes;
        private NativeArray<int2x4> pieceCells;
        private NativeList<Random> rngs;
        private NativeList<Selected> selectBuffer;
        private NativeMultiHashMap<GraphNodePtr, Expanded> expandBuffer;
        private NativeList<Piece> selectGuidance;
        private NativeReference<Weights> weights;
        private NativeReference<int> groupLengthAfterDeduplicate;
        private readonly List<NativeList<int>> needUpdateNodeLists;

        private JobRunner runner;

        public int ParallelCount { get; private set; }
        public int Length { get; private set; }
        public int MaxDepth { get; private set; }
        public SimpleColBoard Board => board;

        private HikariConfig Config { get; }

        public HikariAI2(HikariConfig config) : this(config, Weights.Default) { }

        public HikariAI2(HikariConfig config, Weights w) {
            config.Validate();
            Config = config;
            graph = new NativeList<GraphGroup>(Config.maxDepth, Allocator.Persistent);
            needUpdateNodeLists = new List<NativeList<int>>(Config.maxDepth);

            pieceVShapes = new NativeArray<uint4x4>(Piece.NativeVShapes, Allocator.Persistent);
            pieceCells = new NativeArray<int2x4>(Piece.Cells, Allocator.Persistent);
            rngs = new NativeList<Random>(Allocator.Persistent);
            selectBuffer = new NativeList<Selected>(Allocator.Persistent);
            expandBuffer = new NativeMultiHashMap<GraphNodePtr, Expanded>(1 << 18, Allocator.Persistent);
            selectGuidance = new NativeList<Piece>(Allocator.Persistent);
            weights = new NativeReference<Weights>(Allocator.Persistent) {
                Value = w
            };
            groupLengthAfterDeduplicate = new NativeReference<int>(Allocator.Persistent);

            board = new SimpleColBoard(pieceVShapes);
            rootIndex = -1;
            dead = false;
            disposed = false;
            nextPieces = new NativeList<PieceKind>(32, Allocator.Persistent);

            ExpandGraph();
            ExpandGraph();

            ParallelCount = 128;
        }

        private void ExpandGraph() {
            if (graph.Length != needUpdateNodeLists.Count) throw new Exception("graph length is invalid");
            if (graph.Length == graph.Capacity) return;
            if (needUpdateNodeLists.Count == needUpdateNodeLists.Capacity) return;
            graph.Add(GraphGroup.Create());
            needUpdateNodeLists.Add(new NativeList<int>(1 << 16, Allocator.Persistent));
        }

        public void Update() {
            if (disposed) {
                Debug.LogWarning("AI is already disposed, do not call Update()");
                return;
            }

            if (runner.Scheduled) {
                var sw = Stopwatch.StartNew();
                runner.Complete();
                sw.Stop();

                var dT = (double) sw.ElapsedTicks / Stopwatch.Frequency;
                if (dT < Time.fixedDeltaTime / 4) ParallelCount += 1;
                else if (dT > Time.fixedDeltaTime / 3) ParallelCount = math.max(ParallelCount - 4, 1);

                var md = 0;
                for (; md < graph.Length; md++) {
                    if (!graph.ElementAt(md).Any()) {
                        break;
                    }
                }

                MaxDepth = md;

                var l = 0;
                for (var i = 0; i < md; i++) {
                    l += graph.ElementAt(i).NodeCount;
                }

                Length = l;

                ref var rootNode = ref graph.ElementAt(0).GetNode(rootIndex);

                if (nextPieces.Length > 0
                    && rootNode.children.HasAny
                    && !rootNode.children.Speculation.Has(nextPieces[0])) {
                    Debug.LogWarning("Speculation corrupted. Rebooting Hikari...");
                    for (var i = 0; i < graph.Length; i++) {
                        graph.ElementAt(i).Clear();
                    }

                    rootIndex = graph.ElementAt(0).AddNode(new GraphNode(default, -2), board);
                    MaxDepth = 0;
                    Length = 1;
                } else if (rootNode.death) {
                    dead = true;
                }

                if (selectGuidance.Length <= MaxDepth) {
                    selectGuidance.Clear();
                }

                ClearJobBuffers();
            }

            if (dead) return;

            if (resetBoard != null) {
                ResetInternal();
            } else if (CurrentPlan.Any() && moveProvided) {
                // the order of root children may be changed
                // so, we need to find where the child with the provided move went
                ref var rootGroup = ref graph.ElementAt(0);
                var children = rootGroup.GetNode(rootIndex).children.GetKnown(rootGroup);
                for (var i = 0; i < children.length; i++) {
                    if (children[i].piece == CurrentPlan[0]) {
                        // Debug.Log(children[i].piece + " " + children[i].Eval(graph.ElementAt(1)));
                        AdvanceGraph(children[i]);
                        moveProvided = false;
                        break;
                    }

                    if (i == children.length - 1) {
                        throw new UnreachableException();
                    }
                }
            }

            if (rootIndex < 0) return;

            UpdatePlan();

            runner.ScheduleNext((ref JobHandle handle) => {
                lock (nextPiecesToAdd) {
                    while (nextPiecesToAdd.Any() && nextPieces.Length < Config.previews) {
                        nextPieces.Add(nextPiecesToAdd.Dequeue());
                        ResolveSpeculation(ref handle);
                    }
                }

                if (nextPieces.IsEmpty) return;

                var loops = math.max(1, math.floorlog2(ParallelCount * 2 / math.min(ParallelCount, Length)));
                if (loops > 1) {
                    loops += 1;
                    if (selectGuidance.Length > 0) {
                        loops = math.max(selectGuidance.Length, loops);
                    }

                    Debug.Log($"QUICK EXPAND MODE: Loops {loops}");
                }

                while (MaxDepth + loops > graph.Length - 1 && graph.Length < graph.Capacity) {
                    ExpandGraph();
                }

                for (var i = 0; i < loops - 1; i++) {
                    ScheduleJobs(ref handle, math.max(1, ParallelCount / 8));
                    handle.Complete();
                    ClearJobBuffers();
                    handle = default;
                }

                ScheduleJobs(ref handle, ParallelCount);
            });
        }

        private void ResetInternal() {
            Debug.Log("Resetting");

            selectGuidance.Clear();
            var prevMaxHeight = board.MaxHeight();
            var bag = board.bag;
            var nextBoard = new SimpleColBoard(resetBoard, pieceVShapes);
            var currentMaxHeight = nextBoard.MaxHeight();
            if (nextPieces.Length > 0 && !board.Spawn(nextPieces[0]).HasValue) {
                Debug.LogWarning("Reset board resulted in death");
                dead = true;
            } else {
                CheckGarbage();

                void CheckGarbage() {
                    if (currentMaxHeight <= prevMaxHeight) return;

                    var diff = currentMaxHeight - prevMaxHeight;
                    for (var i = 0; i < diff; i++) {
                        if (math.countbits((uint) nextBoard.Row(i)) != 9) return;
                    }

                    for (var i = 0; i < prevMaxHeight; i++) {
                        if (board.Row(i) != nextBoard.Row(i + diff)) return;
                    }

                    foreach (var piece in CurrentPlan) {
                        selectGuidance.Add(piece.IsInvalid ? piece : piece.WithOffset(0, diff));
                    }
                }
            }

            Profiler.BeginSample("Clear Graph");
            for (var i = 0; i < graph.Length; i++) {
                graph.ElementAt(i).Clear();
            }

            Profiler.EndSample();

            board = nextBoard;
            rootIndex = graph.ElementAt(0).AddNode(new GraphNode(default, -2), board);
            MaxDepth = 0;
            Length = 1;
            lastNodes = 1;
            resetBoard = null;
            calculationTimeStopwatch.Restart();
        }

        private void UpdatePlan() {
            lock (moveLock) {
                CurrentPlan.Clear();
                ref var group = ref graph.ElementAt(0);
                ref var node = ref group.GetNode(rootIndex);
                var firstPiece = node.children.Speculation.GetKnownPiece();
                for (var i = 0; i < MaxDepth; i++) {
                    if (node.IsLeaf) break;
                    if (!node.children.Speculation.IsKnown()) break;
                    var best = node.children.GetKnown(group)[0];
                    if (i == 0) {
                        lastEval = best.Eval(graph.ElementAt(1));
                    }

                    group = ref graph.ElementAt(i + 1);
                    node = ref best.Node(group);
                    CurrentPlan.Add(best.piece);
                }

                if (CurrentPlan.Any()) {
                    if (CurrentPlan[0] != lastPlan) {
                        pathForCurrentPlan = CurrentPlan[0].IsInvalid
                            ? new Path {
                                result = CurrentPlan[0], holdOnly = true,
                                instructions = new List<Instruction>().AsReadOnly()
                            }
                            : PathFinder.FindPath(board, CurrentPlan[0], CurrentPlan[0].Kind != firstPiece, pieceCells);
                    }

                    lastPlan = CurrentPlan[0];
                } else {
                    pathForCurrentPlan = null;
                    lastPlan = null;
                }
            }
        }

        private void AdvanceGraph(GraphChild pickedChild) {
            // new StandardEvaluator().EvaluateBoard(board, Weights.Default);
            
            board.bag.Take(nextPieces[0]);

            if (pickedChild.piece.IsInvalid) {
                var unHold = board.HoldSelf(nextPieces[0]);
                if (unHold != null) Debug.LogError("hold only move invalid");
            } else if (pickedChild.piece.Kind == nextPieces[0]) {
                board.LockSelf(pickedChild.piece);
            } else {
                var unHold = board.HoldSelf(nextPieces[0]);
                if (unHold != pickedChild.piece.Kind) Debug.LogError("hold move invalid");
                board.LockSelf(pickedChild.piece);
            }

            nextPieces.RemoveAt(0);

            var first = graph[0];
            lastNodes = Length - first.NodeCount;
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var l = 0;
            for (var i = 1; i < graph.Length; i++) {
                l += graph[i].NodeCount;
            }

            if (lastNodes != l) {
                Debug.LogWarning("errrrr");
            }
#endif
            
            graph.RemoveAt(0);
            first.Clear();
            graph.Add(first);
            rootIndex = pickedChild.node;
            calculationTimeStopwatch.Restart();
        }

        private void ClearJobBuffers() {
            foreach (var list in needUpdateNodeLists) {
                list.Clear();
            }

            expandBuffer.Clear();
        }

        private void ResolveSpeculation(ref JobHandle handle) {
            var depth = nextPieces.Length - 1;
            if (depth >= graph.Length) return;

            var nodeCount = graph.ElementAt(depth).NodeCount;
            if (nodeCount == 0) return;

            var resolveJob = new ResolveSpeculationJob {
                graph = graph,
                target = depth,
                resolved = nextPieces[depth],
                toUpdate = needUpdateNodeLists[depth].AsParallelWriter()
            };
            handle = resolveJob.Schedule(nodeCount, 16, handle);
        }

        private void ScheduleJobs(ref JobHandle handle, int parallelCount) {
            var batchCount = Config.singleThread ? int.MaxValue : 1;
            
            rngs.ResizeUninitialized(parallelCount);
            var createRngJob = new CreateRandomsJob {
                outputs = rngs,
                rng = new Random((uint) UnityEngine.Random.Range(0, int.MaxValue))
            };
            handle = createRngJob.Schedule(handle);

            selectBuffer.ResizeUninitialized(parallelCount);
            var selectJob = new SelectJob {
                graph = graph,
                queue = nextPieces,
                rands = rngs,
                rootBoard = board,
                rootIndex = rootIndex,
                selectedList = selectBuffer,
                selectGuidance = selectGuidance
            };
            handle = selectJob.Schedule(parallelCount, batchCount, handle);


            var validateJob = new SelectionValidateJob {
                selections = selectBuffer
            };
            handle = validateJob.Schedule(handle);

            JobHandle.ScheduleBatchedJobs();

            var expandJob = new ExpandJob {
                graph = graph,
                selections = selectBuffer,
                outputWriter = expandBuffer.AsParallelWriter(),
                pieceCells = pieceCells,
                useHold = Config.useHold
            };
            handle = expandJob.Schedule(parallelCount, batchCount, handle);

            var evaluateJob = new EvaluateJob<StandardEvaluator> {
                graph = graph,
                selectResults = selectBuffer,
                expandResults = expandBuffer,
                weights = weights.AsReadOnly()
            };
            handle = evaluateJob.Schedule(parallelCount, batchCount, handle);

            var pHandle = default(JobHandle);
            for (var i = 0; i < graph.Length; i++) {
                var prepareJob = new PrepareBackpropagationJob {
                    selectResults = selectBuffer,
                    toUpdates = needUpdateNodeLists[i],
                    target = i
                };
                pHandle = JobHandle.CombineDependencies(prepareJob.Schedule(handle), pHandle);
            }

            handle = pHandle;

            for (var i = graph.Length - 1; i >= 0; i--) {
                var distinctJob = new NativeListDistinctJob {
                    list = needUpdateNodeLists[i].AsDeferredJobArray(),
                    lengthOutput = groupLengthAfterDeduplicate
                };
                handle = distinctJob.Schedule(handle);

                var backpropagateJob = new BackpropagateJob {
                    graph = graph,
                    toUpdate = needUpdateNodeLists[i].AsDeferredJobArray(),
                    nextUpdate =
                        (i == 0 ? needUpdateNodeLists[needUpdateNodeLists.Count - 1] : needUpdateNodeLists[i - 1])
                        .AsParallelWriter(),
                    depth = i
                };
                unsafe {
                    handle = backpropagateJob.Schedule(
                        (int*) NativeReferenceUnsafeUtility.GetUnsafePtrWithoutChecks(groupLengthAfterDeduplicate),
                        batchCount, handle);
                }
            }
        }

        public void AddNextPiece(PieceKind pieceKind) {
            lock (nextPiecesToAdd) {
                nextPiecesToAdd.Enqueue(pieceKind);
            }
        }

        public Direction? GetNextMove() {
            lock (moveLock) {
                if (dead || CurrentPlan.Count == 0 || pathForCurrentPlan is null) {
                    return null;
                }

                if (moveProvided) {
                    Debug.LogWarning("move has already provided");
                }

                moveProvided = true;
                var path = pathForCurrentPlan.Value;
                pathForCurrentPlan = null;
                lastPlan = null;
                return new Direction(
                    path, Length, 
                    (Length - lastNodes) * 1000f / calculationTimeStopwatch.ElapsedMilliseconds,
                    MaxDepth, lastEval);
            }
        }

        public void Reset(Board board) {
            resetBoard = board;
            pathForCurrentPlan = null;
            lastPlan = null;
        }

        public void Dispose() {
            if (runner.Scheduled) {
                runner.Complete();
            }

            for (var i = 0; i < graph.Length; i++) {
                graph.ElementAt(i).Dispose();
            }

            graph.Dispose();
            nextPieces.Dispose();
            pieceVShapes.Dispose();
            pieceCells.Dispose();
            rngs.Dispose();
            selectBuffer.Dispose();
            expandBuffer.Dispose();
            selectGuidance.Dispose();
            weights.Dispose();
            groupLengthAfterDeduplicate.Dispose();
            foreach (var list in needUpdateNodeLists) {
                list.Dispose();
            }

            disposed = true;
        }
    }
}