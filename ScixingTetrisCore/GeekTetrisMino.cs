using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class GeekTetrisMino : ITetrisMino
    {
        public (int X, int Y)[][] _field { get; private set; }
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public MinoType MinoType { get; private set; }

        public static readonly GeekTetrisMino I = new()
        {
            _field = new (int, int)[][]
            {
                //0b0000,
                //0b0000,
                //0b1111,
                //0b0000,
                new[] { (0, 0), (1, 0), (2, 0), (-1, 0) },
                //0b0010,
                //0b0010,
                //0b0010,
                //0b0010,
                new[] { (0, 0), (0, 1), (0, 2), (0, -1) },
                //0b0000,
                //0b1111,
                //0b0000,
                //0b0000,
                new[] { (0, 0), ( 1, 0), (2, 0), ( -1, 0) },
                //0b0100,
                //0b0100,
                //0b0100,
                //0b0100,
                new[] { (0, 0), (0, 1), (0, 2), (0, -1) },
            },
            Width = 4,
            Height = 4,
            Name = "I",
            MinoType = MinoType.SC_I,
        };
        public static readonly GeekTetrisMino O = new()
        {
            _field = new (int, int)[][]
            {
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (0, 0), (1, 0), (1, 1), (0, 1) }, 
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (0, 0), (1, 0), (1, 1), (0, 1) }, 
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (0, 0), (1, 0), (1, 1), (0, 1) }, 
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (0, 0), (1, 0), (1, 1), (0, 1) }, 
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (1, 1), (1, 2), (2, 1), (2, 2) },
            },
            Width = 4,
            Height = 4,
            Name = "O",
            MinoType = MinoType.SC_O,
        };
        public static readonly GeekTetrisMino T = new()
        {
            _field = new (int, int)[][]
            {
                
                //0b000,
                //0b111,
                //0b010,
                new [] { (0, 0), (0, 1), (-1, 0), (0, -1) },
                //0b010,
                //0b011,
                //0b010,
                new [] { (0, 0), (1, 0), (-1, 0), (0, -1) },
                //0b010,
                //0b111,
                //0b000,
                new [] { (0, 0), (1, 0), (0, 1), (0, -1) },
                //0b010,
                //0b110,
                //0b010,
                new [] { (0, 0), (1, 0), (0, 1), (-1, 0) },
            },
            Width = 3,
            Height = 3,
            Name = "T",
            MinoType = MinoType.SC_T,
        };
        public static readonly GeekTetrisMino J = new()
        {
            _field = new (int, int)[][]
            {
                //0b000,
                //0b111,
                //0b100,
                new [] { (0, 0), (1, 0), (2, 0), (0, -1) },
                //0b010,
                //0b010,
                //0b011,
                new [] { (0, 0), (1, 0), (0, 1), (0, 2) },
                //0b001,
                //0b111,
                //0b000,
                new [] { (0, 0), (0, 1), (-1, 0), (-2, 0) },
                //0b110,
                //0b010,
                //0b010,
                new [] { (0, 0), (0, -1), (0, -2), (-1, 0) },
            },
            Width = 3,
            Height = 3,
            Name = "J",
            MinoType = MinoType.SC_J,
        };
        public static readonly GeekTetrisMino L = new()
        {
            _field = new (int, int)[][]
            {
                //0b010,
                //0b010,
                //0b110,
                new [] { (0, 0), (1, 0), ( 2, 0), ( 0, 1) },
                //0b100,
                //0b111,
                //0b000,
                new [] { (0, 0), (0, 1), (0, 2), (-1, 0) },
                //0b011,
                //0b010,
                //0b010,
                new [] { (0, 0), (0, -1), (-1, 0), (-2, 0) },
                
                //0b000,
                //0b111,
                //0b001,
                new [] { (0, 0), (1, 0), (0, -1), (0, -2) },
            },
            Width = 3,
            Height = 3,
            Name = "L",
            MinoType = MinoType.SC_L,
        };
        public static readonly GeekTetrisMino S = new()
        {
            _field = new (int, int)[][]
            {
                //0b000,
                //0b110,
                //0b011,
                new [] { (0, 0), (1, 0), (1, 1), (0, -1) },
                //0b001,
                //0b011,
                //0b010,
                new [] { (0, 0), (0, -1), (1, -1), (-1, 0) },
                //0b110,
                //0b011,
                //0b000,
                new [] { (0, 0), (1, 0), (1, 1), (0, -1) },
                //0b010,
                //0b110,
                //0b100,
                new [] { (0, 0), (0, -1), (1, -1), (-1, 0) },
            },
            Width = 3,
            Height = 3,
            Name = "S",
            MinoType = MinoType.SC_S,
        };
        public static readonly GeekTetrisMino Z = new()
        {
            _field = new (int, int)[][]
            {
                //0b000,
                //0b011,
                //0b110,
                new [] { (0, 0), (1, 0), (0, 1), (1, -1) },
                //0b010,
                //0b011,
                //0b001,
                new [] { (0, 0), (1, 0), (-1, -1), (0, -1) },
                //0b011,
                //0b110,
                //0b000,
                new [] { (0, 0), (1, 0), (0, 1), (1, -1) },
                //0b100,
                //0b110,
                //0b010,
                new [] { (0, 0), (1, 0), (-1, -1), (0, -1) },
            },
            Width = 3,
            Height = 3,
            Name = "Z",
            MinoType = MinoType.SC_Z,
        };

        public (int X, int Y)[] GetMinoField(int Stage)
        {
            return ((int X, int Y)[])_field[Stage].Clone();
        }

        public ITetrisMino[] GetMinoList()
        {
            return new[]
            {
                I,
                O,
                T,
                J,
                L,
                Z,
                S,
            };
        } 
    }
}
