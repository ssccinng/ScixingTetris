using System;
using System.Collections.Generic;
using Hikari.AI.Graph;
using NUnit.Framework;
using Unity.Collections;

namespace Hikari.AI.Tests {
    public class WarehouseTests {
        // A Test behaves as an ordinary method
        [Test]
        public void AddSingleAndRead() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            storage.AddNoResize(4);
            Assert.AreEqual(4, storage[0]);
        }

        [Test]
        public void AddManyAndRead() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            for (var i = 0; i < 8 * 4; i++) {
                storage.AddNoResize(i);
            }

            for (var i = 0; i < 8 * 4; i++) {
                Assert.AreEqual(i, storage[i]);
            }
        }

        [Test]
        public void NegativeIndex() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            storage.AddNoResize(0);
            Assert.Throws<IndexOutOfRangeException>(() => {
                var i = storage[-1];
            });
        }

        [Test]
        public void OutOfRange() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            storage.AddNoResize(0);
            Assert.Throws<IndexOutOfRangeException>(() => {
                var i = storage[1];
            });
        }

        [Test]
        public unsafe void AddRange() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            var e = stackalloc int[8];
            for (var i = 0; i < 8; i++) {
                e[i] = i;
            }

            for (var i = 0; i < 4; i++) {
                storage.AddRangeNoResize(new View<int>(e, 8));
            }

            // Assert.AreEqual(8 * 4, storage.Length);

            for (var i = 0; i < 8 * 4; i++) {
                Assert.AreEqual(i % 8, storage[i]);
            }
        }

        [Test]
        public unsafe void AddRangeMasonry() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            var e = stackalloc int[8];
            for (var i = 0; i < 8; i++) {
                e[i] = i;
            }

            storage.AddRangeNoResize(new View<int>(e, 5));
            // Assert.AreEqual(5, storage.Length);
            storage.AddRangeNoResize(new View<int>(e, 3));
            // Assert.AreEqual(8, storage.Length);
            storage.AddRangeNoResize(new View<int>(e, 4));
            // Assert.AreEqual(12, storage.Length);
            storage.AddRangeNoResize(new View<int>(e, 5));
            // Assert.AreEqual(21, storage.Length);
            storage.AddRangeNoResize(new View<int>(e, 5));
            // Assert.AreEqual(29, storage.Length);

            var exp = new[] {
                0, 1, 2, 3, 4, 0, 1, 2,
                0, 1, 2, 3, -1, -1, -1, -1,
                0, 1, 2, 3, 4, -1, -1, -1,
                0, 1, 2, 3, 4, -1, -1, -1
            };

            for (var i = 0; i < exp.Length; i++) {
                if (exp[i] >= 0) Assert.AreEqual(exp[i], storage[i]);
            }
        }

        [Test]
        public void ExcessCapacityAdd() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            var results = new List<int>(storage.Capacity + 1);
            for (var i = 0; i < storage.Capacity + 1; i++) {
                results.Add(storage.AddNoResize(i));
            }

            for (var i = 0; i < storage.Capacity; i++) {
                Assert.AreEqual(i, results[i]);
            }

            Assert.AreEqual(-1, results[storage.Capacity + 1 - 1]);
        }

        [Test]
        public unsafe void ExcessCapacityAddRange() {
            using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
            var e = stackalloc int[8];
            for (var i = 0; i < 8; i++) {
                e[i] = 1;
            }
            var results = new List<int>(storage.Capacity + 1);
            for (var i = 0; i < storage.Capacity / 8 + 1; i++) {
                results.Add(storage.AddRangeNoResize(new View<int>(e, 6)));
            }

            for (var i = 0; i < storage.Capacity / 8; i++) {
                Assert.AreEqual(i * 8, results[i]);
            }

            Assert.AreEqual(-1, results[storage.Capacity / 8 + 1 - 1]);
        }

        [Test]
        public unsafe void OverSizeAddRange() {
            Assert.Throws<InvalidOperationException>(() => {
                using var storage = new Warehouse<int>(8, 4, Allocator.Persistent);
                var e = stackalloc int[16];
                for (var i = 0; i < 16; i++) {
                    e[i] = 1;
                }

                storage.AddRangeNoResize(new View<int>(e, 16));
            });
        }
    }
}