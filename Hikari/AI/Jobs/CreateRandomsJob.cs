using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Hikari.AI.Jobs {
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct CreateRandomsJob : IJob {
        public Random rng;
        [WriteOnly] public NativeArray<Random> outputs;

        public void Execute() {
            for (var i = 0; i < outputs.Length; i++) {
                outputs[i] = new Random(rng.NextUInt(1, uint.MaxValue));
            }
        }
    }
}