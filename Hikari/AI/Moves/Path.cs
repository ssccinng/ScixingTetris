using System.Collections.ObjectModel;
using Hikari.Puzzle;

namespace Hikari.AI.Moves {
    public struct Path {
        public const int MaxInstructions = 16;
        
        public bool holdOnly;
        public bool hold;
        public ReadOnlyCollection<Instruction> instructions;
        public int time;
        public Piece result;
    }
}