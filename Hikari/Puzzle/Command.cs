using System;

namespace Hikari.Puzzle {
    [Flags]
    public enum Command : byte {
        Left = 0x1,
        Right = 0x2,
        RotateLeft = 0x4,
        RotateRight = 0x8,
        SoftDrop = 0x10,
        HardDrop = 0x20,
        Hold = 0x40
    }
}