using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Hikari.AI.Graph {
    public struct GraphGroup : INativeDisposable {
        private Warehouse<GraphNode> nodeStorage;
        private Warehouse<GraphChild> childStorage;
        // private Warehouse<uint> boardBackBuffer;
        private UnsafeHashMap<StateBoard, int> lookup;
        private UnsafeMultiHashMap<int, int> parentMap;
        private int nodeCount;
        private int lockFlag;

        public static GraphGroup Create() {
            var group = new GraphGroup {
                nodeStorage = new Warehouse<GraphNode>(1 << 12, 2048, Allocator.Persistent),
                childStorage = new Warehouse<GraphChild>(1 << 12, 2048, Allocator.Persistent),
                // boardBackBuffer = new Warehouse<uint>(1 << 12, 8192, Allocator.Persistent),
                lookup = new UnsafeHashMap<StateBoard, int>(1 << 19, Allocator.Persistent),
                parentMap = new UnsafeMultiHashMap<int, int>(1 << 15, Allocator.Persistent),
            };
            return group;
        }

        public readonly int NodeCount => nodeCount;

        public int AddNode(in GraphNode node, in SimpleColBoard board, bool bypassDeduplicator = false) {
            var state = new StateBoard(board);

            // Lookup
            // none: this thread can perform add operation to nodeStorage
            //       set your entry -1, after finished, set it the node index
            // -1: other thread is writing to nodeStorage, but not complete
            //     (this thread should wait for complete)
            // -2: addition to nodeStorage failed
            //     (need to cancel the method)
            // ≥0: use existing node
            while (lockFlag == 1 || Interlocked.Exchange(ref lockFlag, 1) == 1) {
                Unity.Burst.Intrinsics.Common.Pause();
            }

            try {
                if (bypassDeduplicator || lookup.TryAdd(state, -1)) {
                    var index = nodeStorage.AddNoResize(node);
                    if (index == -1) {
                        lookup[state] = -2;
                        return -1;
                    }

                    if (!bypassDeduplicator) {
                        lookup[state] = index;
                    }

                    Interlocked.Increment(ref nodeCount);

                    return index;
                } else {
                    while (lookup[state] == -1) {
                        Unity.Burst.Intrinsics.Common.Pause();
                    } // Spin lock until the first node is written

                    var index = lookup[state];
                    if (index == -2) {
                        return -1;
                    }

                    ref var existing = ref nodeStorage[index];
                    if (existing.parent >= 0) {
                        parentMap.Add(index, existing.parent);
                        existing.parent = -1;
                    }
                    parentMap.Add(index, node.parent);
                    return index;
                }
            } finally {
                Interlocked.Exchange(ref lockFlag, 0);
            }
        }

        public int AddChildren(View<GraphChild> children) {
            return childStorage.AddRangeNoResize(children);
        }

        public readonly ref GraphNode GetNode(int index) => ref nodeStorage[index];

        public readonly View<GraphChild> GetChildren(int start, int length) {
            return childStorage.Slice(start, length);
        }

        public readonly UnsafeMultiHashMap<int, int>.Enumerator GetParentIndexesOf(int index) {
            return parentMap.GetValuesForKey(index);
        }

        public readonly bool Any() {
            return nodeStorage.Any();
        }

        public void Clear() {
            nodeStorage.Clear();
            childStorage.Clear();
            lookup.Clear();
            parentMap.Clear();
            nodeCount = 0;
        }

        public void Dispose() {
            nodeStorage.Dispose();
            childStorage.Dispose();
            lookup.Dispose();
            parentMap.Dispose();
            nodeCount = 0;
        }

        public JobHandle Dispose(JobHandle inputDeps) {
            return JobHandle.CombineDependencies(
                JobHandle.CombineDependencies(nodeStorage.Dispose(inputDeps), childStorage.Dispose(inputDeps)),
                JobHandle.CombineDependencies(lookup.Dispose(inputDeps), parentMap.Dispose(inputDeps))
            );
        }
    }
}