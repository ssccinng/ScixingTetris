using System.Collections.Generic;
using System.Linq;
using Hikari.Puzzle;
using Hikari.Puzzle.Visual;
using Unity.Mathematics;

namespace Hikari.AI.Visual {
    public static class PlacementPreviewFactory {
        public static IEnumerable<PlacementPreviewData> Get(IEnumerable<Piece> plan, Board board) {
            plan = plan.ToList();
            var separates = new List<Dictionary<int, int>>();

            foreach (var piece in plan) {
                if (piece.IsInvalid) continue;
                
                var cells = piece.GetCells();
                var placement = board.Lock(piece);
                
                var connections = CellVisualHelper.Connections[(int) piece.Kind][piece.Spin];
                yield return new PlacementPreviewData(piece.Kind,new[] {0, 1, 2, 3}.Select(i => {
                    var pos = cells[i];
                    pos = new int2(pos.x, Offset(pos.y));
                    var connection = connections[i];
                    return (pos: pos, con: connection);
                }), placement);

                var corrupted = new Dictionary<int, int>();
                var offset = 0;
                for (var i = 0; i < placement.clearedLines.Count; i++) {
                    if (corrupted.TryGetValue(placement.clearedLines[i] - offset, out var num))
                        corrupted[placement.clearedLines[i] - offset] = num + 1;
                    else
                        corrupted.Add(placement.clearedLines[i] - offset, 1);

                    offset++;
                }

                separates.Add(corrupted);
            }
            
            int Offset(int y) {
                for (var i = separates.Count - 1; i >= 0; i--) {
                    var sep = separates[i];
                    y += sep.Where(kv => kv.Key <= y).Sum(kv => kv.Value);
                }

                return y;
            }
        }
    }
}