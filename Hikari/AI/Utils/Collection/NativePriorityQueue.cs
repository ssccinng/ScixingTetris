using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace Hikari.AI.Utils.Collection {
    
    public struct NativePriorityQueue<T> : IDisposable where T : struct, IComparable<T> {
        private NativeList<T> list;
        private readonly bool greater;

        public NativePriorityQueue(bool greater, int initLength, Allocator alloc) {
            list = new NativeList<T>(initLength, alloc);
            this.greater = greater;
        }

        public NativePriorityQueue(bool greater, Allocator alloc) : this(greater, 0, alloc) { }

        public void Enqueue(T value) {
            list.Add(value);
            var i = list.Length - 1;
            while (i != 0) {
                var parent = (i - 1) / 2;
                if (Compare(list[i], list[parent]) > 0) {
                    Swap(i, parent);
                    i = parent;
                } else break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Swap(int i1, int i2) {
            var tmp = list[i1];
            list[i1] = list[i2];
            list[i2] = tmp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Compare(T a, T b) {
            var c = a.CompareTo(b);
            if (!greater) {
                c = -c;
            }

            return c;
        }

        public bool TryDequeue(out T value) {
            if (list.Length == 0) {
                value = default;
                return false;
            }

            value = list[0];
            list.RemoveAtSwapBack(0);
            var parent = 0;
            while (true) {
                var child = 2 * parent + 1;
                if (child > list.Length - 1) break;
                if (child < list.Length - 1 && Compare(list[child], list[child + 1]) < 0) {
                    child += 1;
                }

                if (Compare(list[parent], list[child]) < 0) {
                    Swap(parent, child);
                    parent = child;
                } else break;
            }

            return true;
        }

        public T Dequeue() {
            if (!TryDequeue(out var value)) throw new Exception();
            return value;
        }

        public T Peek() {
            return list[0];
        }

        public void Dispose() {
            list.Dispose();
        }

        public void Clear() {
            list.Clear();
        }
    }
}