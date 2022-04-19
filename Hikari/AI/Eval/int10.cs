using System;
#if HIKARI_CLIENT
using MessagePack;
#endif

namespace Hikari.AI.Eval {
#if HIKARI_CLIENT
    [MessagePackObject]
#endif
    [Serializable]
    public unsafe struct int10 {
#if HIKARI_CLIENT
        [Key(0)] public int item0;
        [Key(1)] public int item1;
        [Key(2)] public int item2;
        [Key(3)] public int item3;
        [Key(4)] public int item4;
        [Key(5)] public int item5;
        [Key(6)] public int item6;
        [Key(7)] public int item7;
        [Key(8)] public int item8;
        [Key(9)] public int item9;
#else
        public int item0;
        public int item1;
        public int item2;
        public int item3;
        public int item4;
        public int item5;
        public int item6;
        public int item7;
        public int item8;
        public int item9;
#endif

        public int10(int item0, int item1, int item2, int item3, int item4, int item5, int item6, int item7, int item8, int item9) {
            this.item0 = item0;
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
            this.item5 = item5;
            this.item6 = item6;
            this.item7 = item7;
            this.item8 = item8;
            this.item9 = item9;
        }

        public int this[int index] {
            get {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (index < 0 || index > 9) throw new ArgumentOutOfRangeException();
#endif
                fixed (int* ptr = &item0) {
                    return ptr[index];
                }
            }
            set {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (index < 0 || index > 9) throw new ArgumentOutOfRangeException();
#endif
                fixed (int* ptr = &item0) {
                    ptr[index] = value;
                }
            }
        }
    }
}