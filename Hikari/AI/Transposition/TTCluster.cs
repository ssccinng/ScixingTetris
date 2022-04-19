using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Hikari.AI.Transposition {
    public struct TTCluster {
        private TTEntry e0;
        private TTEntry e1;
        private TTEntry e2;
        private TTEntry e3;
        public const int Size = 4;

        public unsafe ref TTEntry this[int i] {
            get {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (i < 0 || i >= Size) {
                    throw new ArgumentOutOfRangeException();
                }
#endif
                fixed (TTEntry* ptr = &e0) {
                    return ref UnsafeUtility.AsRef<TTEntry>(ptr + i);
                }
            }
        }
    }
}