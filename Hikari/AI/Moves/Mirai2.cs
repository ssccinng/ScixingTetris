// using System.Runtime.CompilerServices;
// using Hikari.AI.Utils.Collection;
// using Hikari.Puzzle;
// using Unity.Collections;
// using Unity.Mathematics;
//
// namespace Hikari.AI.Moves {
//     public struct Mirai2 {
//         public NativePriorityQueue<Placement> next;
//         public NativeList<Placement> locked;
//         public SimpleColBoard board;
//         
//         public void Generate(Piece spawned) {
//             var root = new Step(Piece.Invalid, 0, 0, spawned, Instruction.None);
//             next.Enqueue(new Placement(root));
//
//             while (next.TryDequeue(out var ci)) {
//                 var piece = new Piece(spawned.Kind, ci.x, ci.y, ci.r, ci.t);
//
//                 var dropped = board.SonicDrop(piece);
//
//                 if (ci.depth < Path.MaxInstructions) {
//                     Append(origin, piece.WithOffset(-1, 0), Instruction.Left);
//                     Append(origin, piece.WithOffset(1, 0), Instruction.Right);
//                     if (spawned.Kind != PieceKind.O) {
//                         Append(origin, Rotate(piece, true), Instruction.Cw, true);
//                         Append(origin, Rotate(piece, false), Instruction.Ccw, true);
//                     }
//
//                     if (dropped.Y != piece.Y) {
//                         Append(origin, dropped, Instruction.SonicDrop);
//                     }
//                 }
//
//                 if (deduplicator.TryAdd(math.hash(dropped.GetCells(pieceCells)) + (uint) piece.Tspin)) {
//                     locked.TryAdd(dropped, ci);
//                 }
//             }
//         }
//
//         private void Append(in Step origin, Piece result, Instruction inst, bool skipCheck = false) {
//             if (result.IsInvalid || !skipCheck && board.Collides(result)) {
//                 // tree.TryAdd(result, default);
//                 return;
//             }
//
//             int t;
//
//             if (inst == Instruction.SonicDrop) {
//                 t = 2 * (origin.piece.Y - result.Y);
//                 // if (result.Kind != PieceKind.T && origin.cost + t >= 20) return;
//             } else {
//                 t = 1;
//             }
//
//             if (origin.inst == inst) {
//                 t += 1;
//             }
//
//             var step = new Step(origin.piece, origin.cost + t, origin.depth + 1, result, inst);
//
//             if (tree.TryAdd(result, step) && step.depth < Path.MaxInstructions) {
//                 if (result.Kind == PieceKind.T || step.cost < 20)
//                     next.Enqueue(new StepRef(step));
//             }
//         }
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private Piece Rotate(in Piece piece, bool cw) {
//             var r = SRSNoAlloc.TryRotate(piece, board, cw, out var result);
//             if (r >= 0) {
//                 return result.WithTSpinStatus(board.CheckTSpin(result, r));
//             } else {
//                 return Piece.Invalid;
//             }
//         }
//     }
// }