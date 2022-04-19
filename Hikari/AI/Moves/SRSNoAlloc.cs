using System.Runtime.CompilerServices;
using Hikari.Puzzle;
using Unity.Collections;
using Unity.Mathematics;

namespace Hikari.AI.Moves {
    public static class SRSNoAlloc {
        private static readonly int2x4[] RotationTable = {
            new int2x4(new int2(-1, 0), new int2(-1, 1), new int2(0, -2), new int2(-1, -2)), //01
            new int2x4(new int2(1, 0), new int2(1, -1), new int2(0, 2), new int2(1, 2)), //10
            new int2x4(new int2(1, 0), new int2(1, -1), new int2(0, 2), new int2(1, 2)), //12
            new int2x4(new int2(-1, 0), new int2(-1, 1), new int2(0, -2), new int2(-1, -2)), //21
            new int2x4(new int2(1, 0), new int2(1, 1), new int2(0, -2), new int2(1, -2)), //23
            new int2x4(new int2(-1, 0), new int2(-1, -1), new int2(0, 2), new int2(-1, 2)), //32
            new int2x4(new int2(-1, 0), new int2(-1, -1), new int2(0, 2), new int2(-1, 2)), //30
            new int2x4(new int2(1, 0), new int2(1, 1), new int2(0, -2), new int2(1, -2)), //03
        };

        private static readonly int2x4[] RotationTableI = {
            new int2x4(new int2(-2, 0), new int2(1, 0), new int2(-2, -1), new int2(1, 2)), //01
            new int2x4(new int2(2, 0), new int2(-1, 0), new int2(2, 1), new int2(-1, -2)), //10
            new int2x4(new int2(-1, 0), new int2(2, 0), new int2(-1, 2), new int2(2, -1)), //12
            new int2x4(new int2(1, 0), new int2(-2, 0), new int2(1, -2), new int2(-2, 1)), //21
            new int2x4(new int2(2, 0), new int2(-1, 0), new int2(2, 1), new int2(-1, -2)), //23
            new int2x4(new int2(-2, 0), new int2(1, 0), new int2(-2, -1), new int2(1, 2)), //32
            new int2x4(new int2(1, 0), new int2(-2, 0), new int2(1, -2), new int2(-2, 1)), //30
            new int2x4(new int2(-1, 0), new int2(2, 0), new int2(-1, 2), new int2(2, -1)), //03
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRotate(Piece piece, in SimpleBoard board, bool cw,
            out int rotation, out Piece rotated, in NativeArray<uint4x4> pieceShapes) {
            if (piece.Kind == PieceKind.O) {
                rotation = 0;
                rotated = piece;
                return true;
            }

            var rotatedDirection = (sbyte) ((piece.Spin + 4 + math.select(-1, 1, cw)) & 3);

            var newPiece = new Piece(piece.Kind, piece.X, piece.Y, rotatedDirection);
            if (!board.Collides(newPiece, pieceShapes)) {
                rotation = 0;
                rotated = newPiece;
                return true;
            }

            var offsetTable = piece.Kind == PieceKind.I
                ? RotationTableI[(piece.Spin * 2 + 8 + math.select(-1, 0, cw)) & 7]
                : RotationTable[(piece.Spin * 2 + 8 + math.select(-1, 0, cw)) & 7];

            for (var i = 0; i < 4; i++) {
                newPiece = piece.WithOffset(offsetTable[i]).WithSpin(rotatedDirection);
                if (!board.Collides(newPiece, pieceShapes)) {
                    rotation = i + 1;
                    rotated = newPiece;
                    return true;
                }
            }

            rotation = default;
            rotated = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryRotate(in Piece piece, in SimpleColBoard board, bool cw, out Piece rotated) {
            if (piece.Kind == PieceKind.O) {
                rotated = piece;
                return 0;
            }

            var rotatedDirection = (sbyte) ((piece.Spin + 4 + math.select(-1, 1, cw)) & 3);

            var newPiece = new Piece(piece.Kind, piece.X, piece.Y, rotatedDirection);
            if (!board.Collides(newPiece)) {
                rotated = newPiece;
                return 0;
            }

            var offsetTable = piece.Kind == PieceKind.I
                ? RotationTableI[(piece.Spin * 2 + 8 + math.select(-1, 0, cw)) & 7]
                : RotationTable[(piece.Spin * 2 + 8 + math.select(-1, 0, cw)) & 7];

            for (var i = 0; i < 4; i++) {
                newPiece = piece.WithOffset(offsetTable[i]).WithSpin(rotatedDirection);
                if (!board.Collides(newPiece)) {
                    rotated = newPiece;
                    return i + 1;
                }
            }

            rotated = default;
            return -1;
        }
    }
}