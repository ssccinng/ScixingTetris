using static Hikari.Puzzle.PlacementKind;

namespace Hikari.Puzzle {
    public enum PlacementKind {
        None = 0b00_000,
        Clear1 = 0b00_001,
        Clear2 = 0b00_010,
        Clear3 = 0b00_011,
        Clear4 = 0b00_100,
        Mini = 0b01_000,
        Mini1 = 0b01_001,
        Mini2 = 0b01_010,
        TSpin = 0b10_000,
        TSpin1 = 0b10_001,
        TSpin2 = 0b10_010,
        TSpin3 = 0b10_011
    }

    public static class PlacementKindExtensions {
        public static uint GetGarbage(this PlacementKind placementKind) {
            return placementKind switch {
                Clear2 => 1,
                Mini2 => 1,
                Clear3 => 2,
                TSpin1 => 2,
                Clear4 => 4,
                TSpin2 => 4,
                TSpin3 => 6,
                _ => 0
            };
        }

        public static bool IsContinuous(this PlacementKind placementKind) {
            return placementKind >= Clear4;
        }

        public static bool IsLineClear(this PlacementKind placementKind) {
            return ((int) placementKind & 0b111) != 0;
        }

        public static bool IsFullTspinClear(this PlacementKind placementKind) {
            return ((int) placementKind & 0b10_000) != 0 && placementKind.IsLineClear();
        }

        public static string GetFullName(this PlacementKind placementKind) {
            return placementKind switch {
                Clear1 => "Single",
                Clear2 => "Double",
                Clear3 => "Triple",
                Clear4 => "Quad",
                TSpin1 => "T-Spin Single",
                TSpin2 => "T-Spin Double",
                TSpin3 => "T-Spin Triple",
                Mini1 => "T-Spin Mini Single",
                Mini2 => "T-Spin Mini Double",
                _ => "..."
            };
        }
    }

    public static class PlacementKindFactory {
        public static PlacementKind Create(int clearLine, TSpinStatus tSpin) {
            return (PlacementKind) ((int) tSpin * 8 + clearLine);
        }
    }
}