using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Hikari.AI.Graph {
    public readonly unsafe struct View<T> where T : unmanaged {
        public readonly T* buffer;
        public readonly int length;

        public View(T* buffer, int length) {
            this.buffer = buffer;
            this.length = length;
        }

        public ref T this[int index] {
            get {
                CheckRange(index);
                return ref buffer[index];
            }
        }
        
        public void Sort<TComp>(TComp comp) where TComp : IComparer<T> {
            NativeSortExtension.Sort(buffer, length, comp);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void CheckRange(int index) {
            if (index < 0 || index >= length) {
                throw new ArgumentOutOfRangeException($"View: index {index} is out of length {length}");
            }
        }

        public T[] ToArray() {
            var array = new T[length];
            for (var i = 0; i < length; i++) {
                array[i] = buffer[i];
            }

            return array;
        }
    }

    public static unsafe class ViewExtensions {
        public static View<T> SliceView<T>(this NativeList<T> buffer, int start, int length) where T : unmanaged {
            return new View<T>((T*) buffer.GetUnsafeReadOnlyPtr() + start, length);
        }

        public static View<T> SliceView<T>(this NativeList<T> buffer) where T : unmanaged =>
            SliceView(buffer, 0, buffer.Length);
    }
}