using System.Collections.Generic;

namespace Hikari.Puzzle {
    public readonly struct LockResult {
        public readonly PlacementKind placementKind;
        public readonly bool b2b;
        public readonly bool perfectClear;
        public readonly uint ren;
        public readonly uint attack;
        public readonly List<int> clearedLines;

        public LockResult(PlacementKind placementKind, bool perfectClear, uint ren, List<int> clearedLines,
            bool prevB2B) {
            this.placementKind = placementKind;
            b2b = placementKind.IsLineClear() ? placementKind.IsContinuous() : prevB2B;
            this.perfectClear = perfectClear;
            this.ren = ren;
            var b2bBonus = prevB2B && b2b && placementKind.IsLineClear();
            attack = (uint) ((perfectClear ? 10 : placementKind.GetGarbage())
                             + (b2bBonus ? 1 : 0)
                             + Game.GetRenAttack(ren));
            this.clearedLines = clearedLines;

            // Debug.Log($"{placementKind.ToString()} {(b2bBonus ? " B2B" : "")} Combo{ren} {(perfectClear ? " Clear!" : "")}");
        }
    }
}