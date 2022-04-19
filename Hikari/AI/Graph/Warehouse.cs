using System;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Hikari.AI.Graph {
    public struct Warehouse<T> : INativeDisposable where T : unmanaged {
        private UnsafeList<Container> containers;
        private readonly int containerSize;
        private int length;
        private int allocatedContainers;
        private readonly Allocator allocator;

        public int Capacity => containerSize * containers.capacity;

        /// <summary>
        /// Create a new Warehouse instance.
        /// <para/>The capacity of this warehouse will be <c>containerSize * maxContainers</c>.
        /// </summary>
        /// <param name="containerSize">The size of each container.</param>
        /// <param name="maxContainers">The number of containers.</param>
        /// <param name="allocator">Allocator.</param>
        public Warehouse(int containerSize, int maxContainers, Allocator allocator) {
            this.containerSize = containerSize;
            containers = new UnsafeList<Container>(maxContainers, allocator);
            length = 0;
            allocatedContainers = 0;
            this.allocator = allocator;
        }

        private unsafe bool CreateContainer(bool noResize = false) {
            var index = Interlocked.Increment(ref containers.length) - 1;
            if (index >= containers.capacity) {
                if (noResize) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    Debug.LogWarning("containers exceed capacity");
#endif
                    Interlocked.Decrement(ref containers.length);
                    return false;
                } else {
                    containers.SetCapacity(containers.Capacity * 2);
                }
            }

            containers[index] = new Container {
                data = (T*) UnsafeUtility.Malloc(sizeof(T) * containerSize, UnsafeUtility.AlignOf<T>(), allocator)
            };
            Interlocked.Increment(ref allocatedContainers);

            return true;
        }

        public readonly ref T this[int index] {
            get {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if (index < 0 || index >= length) {
                    throw new IndexOutOfRangeException($"{index} is out of range of {length} long warehouse");
                }
#endif
                return ref containers[Math.DivRem(index, containerSize, out var mod)][mod];
            }
        }

        public readonly bool Any() => length > 0;

        public readonly unsafe View<T> Slice(int start, int length) {
            return new View<T>((T*) UnsafeUtility.AddressOf(ref this[start]), length);
        }

        public int AddNoResize(in T item) {
            var index = Interlocked.Increment(ref length) - 1;
            if (index == allocatedContainers * containerSize) {
                if (!CreateContainer(true)) {
                    return -1;
                }
            } else {
                while (index > allocatedContainers * containerSize) {
                    Unity.Burst.Intrinsics.Common.Pause(); // Spin lock
                }
            }

            this[index] = item;
            return index;
        }

        public int AddRangeNoResize(View<T> items) {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (items.length == 0) throw new ArgumentException("length of view is 0");
            if (items.length > containerSize) throw new InvalidOperationException();
#endif

            int copiedLength;
            int index;
            do {
                copiedLength = length;
                if (copiedLength % containerSize + items.length > containerSize) {
                    index = (copiedLength / containerSize + 1) * containerSize;
                } else {
                    index = copiedLength;
                }
            } while (Interlocked.CompareExchange(ref length, index + items.length, copiedLength) != copiedLength);

            if (index == allocatedContainers * containerSize) {
                if (!CreateContainer(true)) {
                    return -1;
                }
            } else {
                while (index > allocatedContainers * containerSize) {
                    Unity.Burst.Intrinsics.Common.Pause(); // Spin lock
                }
            }

            unsafe {
                UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref this[index]), items.buffer,
                    sizeof(T) * items.length);
            }

            return index;
        }

        public void Clear() {
            length = 0;
        }

        public unsafe void Dispose() {
            for (var i = 0; i < containers.length; i++) {
                UnsafeUtility.Free(containers[i].data, allocator);
            }

            containers.Dispose();
        }

        public JobHandle Dispose(JobHandle inputDeps) {
            return new DisposeJob(this).Schedule(inputDeps);
        }

        [BurstCompile]
        private struct DisposeJob : IJob {
            private Warehouse<T> warehouse;

            public DisposeJob(Warehouse<T> warehouse) {
                this.warehouse = warehouse;
            }

            public void Execute() {
                warehouse.Dispose();
            }
        }

        private unsafe struct Container {
            public T* data;

            public ref T this[int index] => ref data[index];
        }
    }
}