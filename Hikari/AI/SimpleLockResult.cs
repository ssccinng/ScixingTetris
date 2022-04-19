using Hikari.Puzzle;

namespace Hikari.AI {
    public readonly struct SimpleLockResult {
        public readonly PlacementKind placementKind;
        public readonly bool backToBack;
        public readonly bool perfectClear;
        public readonly bool death;
        public readonly int ren;

        public SimpleLockResult(PlacementKind placementKind, bool death, bool backToBack, bool perfectClear, int ren) {
            this.placementKind = placementKind;
            this.backToBack = backToBack;
            this.death = death;
            this.perfectClear = perfectClear;
            this.ren = ren;
        }

        public int GetAttack() {
            if (!placementKind.IsLineClear()) return -1;
            if (perfectClear) return 10;
            return (int)placementKind.GetGarbage() + (backToBack ? 1 : 0) + Game.GetRenAttack(ren);
        }
    }
}