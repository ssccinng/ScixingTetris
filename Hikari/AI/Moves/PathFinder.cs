using Hikari.Puzzle;
using Unity.Collections;
using Unity.Mathematics;

namespace Hikari.AI.Moves {
    public static class PathFinder {
        public static Path? FindPath(in SimpleColBoard board, Piece piece, bool useHold, in NativeArray<int2x4> pieceCells) {
            var spawned = board.Spawn(piece.Kind);
            if (!spawned.HasValue) return null;

            using var mirai = new Mirai(board, pieceCells);
            mirai.Generate(spawned.Value);

            return mirai.RebuildPath(piece, useHold);
        }
    }
}