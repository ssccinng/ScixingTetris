using System;
using Hikari.AI.Graph;
using Hikari.Puzzle;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct ResolveSpeculationJob : IJobParallelFor {
        [NativeDisableContainerSafetyRestriction] public NativeList<GraphGroup> graph;
        public NativeList<int>.ParallelWriter toUpdate;
        public int target;
        public PieceKind resolved;
        
        public void Execute(int index) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (index >= graph.ElementAt(target).NodeCount) {
                throw new IndexOutOfRangeException();
            }
#endif
            ref var node = ref graph.ElementAt(target).GetNode(index);
            if (node.children.HasAny) {
                if (!node.children.ResolveSpeculation(resolved)) {
                    node.death = true;
                }
                toUpdate.AddNoResize(index);
            }
        }
    }
}