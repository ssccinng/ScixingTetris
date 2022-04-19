using Hikari.AI.Documents;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct PrepareBackpropagationJob : IJob {
        [ReadOnly] public NativeArray<Selected> selectResults;
        public NativeList<int> toUpdates;
        public int target;
        
        public void Execute() {
            for (var i = 0; i < selectResults.Length; i++) {
                if (selectResults[i].valid && selectResults[i].node.groupIndex == target) {
                    toUpdates.Add(selectResults[i].node.nodeIndex);
                }
            }
        }
    }
}