using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct NativeListDistinctJob : IJob {
        public NativeArray<int> list;
        public NativeReference<int> lengthOutput;
        
        public void Execute() {
            list.Sort();
            lengthOutput.Value = list.Unique();
        }
    }
}