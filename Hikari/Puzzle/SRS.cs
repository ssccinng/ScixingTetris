using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hikari.Puzzle {
    public static class SRS {
        private static readonly Dictionary<int, Vector2Int[]> RotationTable = new Dictionary<int, (int, int)[]> {
            {01, new[] {(0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2)}},
            {10, new[] {(0, 0), (1, 0), (1, -1), (0, 2), (1, 2)}},
            {12, new[] {(0, 0), (1, 0), (1, -1), (0, 2), (1, 2)}},
            {21, new[] {(0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2)}},
            {23, new[] {(0, 0), (1, 0), (1, 1), (0, -2), (1, -2)}},
            {32, new[] {(0, 0), (-1, 0), (-1, -1), (0, 2), (-1, 2)}},
            {30, new[] {(0, 0), (-1, 0), (-1, -1), (0, 2), (-1, 2)}},
            {03, new[] {(0, 0), (1, 0), (1, 1), (0, -2), (1, -2)}}
        }.ToDictionary(kv => kv.Key,
            kv => kv.Value.Select(tuple => new Vector2Int(tuple.Item1, tuple.Item2)).ToArray());

        private static readonly Dictionary<int, Vector2Int[]> RotationTableI = new Dictionary<int, (int, int)[]> {
            {01, new[] {(0, 0), (-2, 0), (1, 0), (-2, -1), (1, 2)}},
            {10, new[] {(0, 0), (2, 0), (-1, 0), (2, 1), (-1, -2)}},
            {12, new[] {(0, 0), (-1, 0), (2, 0), (-1, 2), (2, -1)}},
            {21, new[] {(0, 0), (1, 0), (-2, 0), (1, -2), (-2, 1)}},
            {23, new[] {(0, 0), (2, 0), (-1, 0), (2, 1), (-1, -2)}},
            {32, new[] {(0, 0), (-2, 0), (1, 0), (-2, -1), (1, 2)}},
            {30, new[] {(0, 0), (1, 0), (-2, 0), (1, -2), (-2, 1)}},
            {03, new[] {(0, 0), (-1, 0), (2, 0), (-1, 2), (2, -1)}}
        }.ToDictionary(kv => kv.Key,
            kv => kv.Value.Select(tuple => new Vector2Int(tuple.Item1, tuple.Item2)).ToArray());

        public static bool TryRotate(Piece piece, Board board, bool turnRight, out (int, Piece) result) {
            Dictionary<int, Vector2Int[]> rotationRule;

            switch (piece.Kind) {
                case PieceKind.O:
                    result = (0, piece);
                    return true;
                case PieceKind.I:
                    rotationRule = RotationTableI;
                    break;
                case PieceKind.L:
                case PieceKind.J:
                case PieceKind.T:
                case PieceKind.Z:
                case PieceKind.S:
                    rotationRule = RotationTable;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var spin = GetRotatedDirection(piece.Spin, turnRight);
            var rotationKey = piece.Spin * 10 + spin;

            for (var i = 0; i < rotationRule[rotationKey].Length; i++) {
                var candidate = rotationRule[rotationKey][i];
                var movedPiece = piece.WithSpin((sbyte) spin).WithOffset(candidate);
                if (board.Collides(movedPiece)) continue;

                result = (i, movedPiece);
                return true;
            }

            result = (-1, default);
            return false;
        }

        private static int GetRotatedDirection(int from, bool turnRight) {
            if (turnRight) return (from + 1) % 4;
            else return from == 0 ? 3 : from - 1;
        }
    }
}