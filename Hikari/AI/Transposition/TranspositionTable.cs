using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Hikari.AI.Transposition {
    public unsafe struct TranspositionTable {
        private TTCluster* table;
        private ulong length;

        public TranspositionTable(ulong length) {
            this.length = math.ceilpow2(length);
            table = (TTCluster*) UnsafeUtility.Malloc(sizeof(TTCluster) * (int) this.length,
                UnsafeUtility.AlignOf<TTCluster>(), Allocator.Persistent);
        }

        public void Clear() {
            UnsafeUtility.MemClear(table, sizeof(TTCluster) * (int) length);
        }

        public ref TTEntry Probe(ulong key, ushort gen, out bool found) {
            Unity.Burst.Intrinsics.Common.umul128(key, length, out var high);
            ref var first = ref table[high][0];
            var key16 = unchecked((ushort) key);

            for (var i = 0; i < TTCluster.Size; i++) {
                if (first.key16 == key16 || first.gen != gen) {
                    found = first.gen != 0;
                    return ref first;
                }
            }

            found = false;
            return ref first;
        }
    }
}