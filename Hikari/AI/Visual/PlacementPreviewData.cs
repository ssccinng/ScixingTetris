using System.Collections.Generic;
using Hikari.Puzzle;
using Hikari.Puzzle.Visual;
using Unity.Mathematics;

namespace Hikari.AI.Visual {
    public readonly struct PlacementPreviewData {
        public readonly PieceKind kind;
        public readonly List<(int2 pos, ConnectionDirection con)> cells;
        public readonly LockResult placement;

        public PlacementPreviewData(PieceKind kind, IEnumerable<(int2 pos, ConnectionDirection con)> cells, LockResult placement) {
            this.kind = kind;
            this.cells = new List<(int2 pos, ConnectionDirection con)>(cells);
            this.placement = placement;
        }
    }
}