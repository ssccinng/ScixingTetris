using Hikari.AI.Documents;
using Hikari.AI.Eval;
using Hikari.AI.Graph;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct EvaluateJob<TEvaluator> : IJobParallelFor where TEvaluator : struct, IEvaluator {
        [ReadOnly] public NativeList<Selected> selectResults;
        [ReadOnly] public NativeMultiHashMap<GraphNodePtr, Expanded> expandResults;
        [ReadOnly] public NativeReference<Weights>.ReadOnly weights;
        [NativeDisableContainerSafetyRestriction] public NativeList<GraphGroup> graph;

        public void Execute(int index) {
            var eval = default(TEvaluator);
            var sel = selectResults[index];
            if (!sel.valid) return;
            var w = weights.Value;
            ref var parent = ref graph.Actualize(sel.node);
            if (parent.death) return;
            ref var childGroup = ref graph.ElementAt(sel.node.groupIndex + 1);
            
            var children = parent.children.GetAll(in graph.ElementAt(sel.node.groupIndex));
            var enumerator = expandResults.GetValuesForKey(sel.node);
            while (enumerator.MoveNext()) {
                var ex = enumerator.Current;
                
                ref var childNode = ref children[ex.childIndex].Node(childGroup);
                if (!childNode.evaluated) {
                    childNode.value = eval.EvaluateBoard(ex.board, w);
                    childNode.evaluated = true;
                }

                children[ex.childIndex].reward =
                    eval.EvaluateMove(ex.time, ex.piece, ex.board, ex.placement, ex.parentIsB2B, w);
            }
        }
    }
}