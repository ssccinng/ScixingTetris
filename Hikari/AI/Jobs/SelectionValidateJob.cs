using Hikari.AI.Documents;
using Hikari.AI.Graph;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct SelectionValidateJob : IJob {
        public NativeArray<Selected> selections;
        
        public void Execute() {
            using var deduplicator = new JacksonDunstan.NativeCollections.NativeHashSet<GraphNodePtr>(selections.Length, Allocator.Temp);
            for (var i = 0; i < selections.Length; i++) {
                if (!deduplicator.TryAdd(selections[i].node)) {
                    selections[i] = default;
                }
            }
        }
    }
}