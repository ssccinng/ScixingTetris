using System;

namespace Hikari.Puzzle.Visual {
    [Flags]
    public enum ConnectionDirection {
        Up = 0b1,
        Right = 0b10,
        Down = 0b100,
        Left = 0b1000
    }
}