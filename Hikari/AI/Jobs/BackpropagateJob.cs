using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hikari.AI.Eval;
using Hikari.AI.Graph;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct BackpropagateJob : IJobParallelForDefer {
        [NativeDisableContainerSafetyRestriction] public NativeList<GraphGroup> graph;
        [ReadOnly] public NativeArray<int> toUpdate;
        [WriteOnly] public NativeList<int>.ParallelWriter nextUpdate;
        public int depth; // of toUpdate nodes

        public void Execute(int index) {
            ref var group = ref graph.ElementAt(depth);
            ref var childGroup = ref graph.ElementAt(depth + 1);
            ref var node = ref group.GetNode(toUpdate[index]);
            if (!node.death) {
                var comparer = new ChildComparer(ref childGroup);
                if (node.children.Speculation.IsKnown()) {
                    var val = ProcessChildren(node.children.GetKnown(group), childGroup, comparer);

                    if (val.HasValue) {
                        node.value = (Value) val;
                    } else {
                        node.death = true;
                    }
                } else {
                    var start = node.children.start;
                    var count = 0;
                    var sum = int4.zero;
                    var spike = 0;
                    for (var i = 0; i < 7; i++) {
                        if (node.children.Speculation.Has(i)) {
                            var sLength = node.children.Speculation.GetLengthOf(i);
                            // use manual children index tracking for efficiency
                            var val = ProcessChildren(group.GetChildren(start, sLength), childGroup, comparer);
                            if (val.HasValue) {
                                count++;
                                sum += val.Value.value;
                                spike = math.max(val.Value.spike, spike);
                            } else {
                                node.death = true;
                            }

                            start += sLength;
                        }
                    }

                    if (!node.death && count != 0) {
                        var avg = sum / count;
                        node.value = new Value(avg, spike);
                    }
                }
            }

            if (depth != 0) {
                if (node.parent >= 0) {
                    nextUpdate.AddNoResize(node.parent);
                } else if (node.parent == -1) {
                    // Append all the parents to next updates
                    var parents = group.GetParentIndexesOf(toUpdate[index]);
                    while (parents.MoveNext()) {
                        nextUpdate.AddNoResize(parents.Current);
                    }
                }
            }
        }

        private static Value? ProcessChildren(View<GraphChild> children, in GraphGroup childGroup, ChildComparer comparer) {
            Value? eval = null;
            children.Sort(comparer);
            for (var i = 0; i < children.length; i++) {
                if (children[i].Death(childGroup)) continue;
                eval = eval?.Improve(children[i].Eval(childGroup))
                       ?? children[i].Eval(childGroup);
            }
            
            return eval;
        }

        private readonly unsafe struct ChildComparer : IComparer<GraphChild> {
            private readonly GraphGroup* ptr;

            public ChildComparer(ref GraphGroup group) {
                ptr = (GraphGroup*) UnsafeUtility.AddressOf(ref group);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Compare(GraphChild x, GraphChild y) {
                ref var group = ref UnsafeUtility.AsRef<GraphGroup>(ptr);
                ref var node1 = ref x.Node(group);
                ref var node2 = ref y.Node(group);
                return (node1.death, node2.death) switch {
                    (true, true) => 0,
                    (true, false) => 1,
                    (false, true) => -1,
                    (false, false) =>
                        -math.csum((node1.value + x.reward).value).CompareTo(math.csum((node2.value + y.reward).value))
                };
            }
        }
    }
}