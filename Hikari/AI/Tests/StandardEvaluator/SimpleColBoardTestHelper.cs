using System;
using Hikari.Puzzle;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Hikari.AI.Tests.StandardEvaluator {
    public class SimpleColBoardTestHelper : IDisposable {
        private NativeArray<uint4x4> nativeShapes;

        private SimpleColBoard board;

        public SimpleColBoard Board {
            get {
                if (disposed) throw new ObjectDisposedException(nameof(nativeShapes));
                return board;
            }
        }

        private bool disposed;

        public unsafe SimpleColBoardTestHelper(string[] rows) {
            Debug.Log(string.Join("\n", rows));
            nativeShapes = new NativeArray<uint4x4>(Piece.NativeVShapes, Allocator.Persistent);
            board = new SimpleColBoard(nativeShapes);
            var len = math.min(rows.Length, 32);
            for (var y = 0; y < len; y++) {
                var line = rows[len - y - 1];
                if (line.Length != 10) {
                    throw new ArgumentException("Length of each row must be 10.");
                }
                for (var x = 0; x < 10; x++) {
                    if (line[x] == 'x') {
                        board.columns[x + 3] |= 1U << y;
                    }
                }
            }
        }

        public void Dispose() {
            nativeShapes.Dispose();
            board = default;
            disposed = true;
        }
    }
}