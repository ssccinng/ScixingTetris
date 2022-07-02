using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public enum MinoType
    {
        SC_I, SC_O, SC_T, SC_L, SC_J, SC_S, SC_Z
    }

    //public class TetrisBitMino : ITetrisMino
    //{
    //    public static readonly TetrisBitMino I = new()
    //    {
    //        _field = new int[][]
    //        {
    //            new int[]
    //            {
    //                0b0000,
    //                0b0000,
    //                0b1111,
    //                0b0000,
    //            },
    //            new int[]
    //            {
    //                0b0010,
    //                0b0010,
    //                0b0010,
    //                0b0010,
    //            },
    //            new int[]
    //            {
    //                0b0000,
    //                0b1111,
    //                0b0000,
    //                0b0000,
    //            },
    //            new int[]
    //            {
    //                0b0100,
    //                0b0100,
    //                0b0100,
    //                0b0100,
    //            },
    //        },
    //        Width = 4,
    //        Height = 4,
    //        Name = "I",
    //    };
    //    public static readonly TetrisBitMino O = new()
    //    {
    //        _field = new int[][]
    //        {
    //            new int[]
    //            {
    //                0b0000,
    //                0b0110,
    //                0b0110,
    //                0b0000,
    //            },
    //            new int[]
    //            {
    //                0b0000,
    //                0b0110,
    //                0b0110,
    //                0b0000,
    //            },
    //            new int[]
    //            {
    //                0b0000,
    //                0b0110,
    //                0b0110,
    //                0b0000,
    //            },
    //            new int[]
    //            {
    //                0b0000,
    //                0b0110,
    //                0b0110,
    //                0b0000,
    //            },
    //        },
    //        Width = 4,
    //        Height = 4,
    //        Name = "O",
    //    };
    //    public static readonly TetrisBitMino T = new()
    //    {
    //        _field = new int[][]
    //        {
    //            new int[]
    //            {
    //                0b000,
    //                0b111,
    //                0b010,
    //            },
    //            new int[]
    //            {
    //                0b010,
    //                0b011,
    //                0b010,
    //            },
    //            new int[]
    //            {
    //                0b010,
    //                0b111,
    //                0b000,
    //            },
    //            new int[]
    //            {
    //                0b010,
    //                0b110,
    //                0b010,
    //            },
    //        },
    //        Width = 3,
    //        Height = 3,
    //        Name = "T",
    //    };
    //    public static readonly TetrisBitMino J = new()
    //    {
    //        _field = new int[][]
    //        {
    //            new int[]
    //            {
    //                0b000,
    //                0b111,
    //                0b100,
    //            },
    //            new int[]
    //            {
    //                0b010,
    //                0b010,
    //                0b011,
    //            },
    //            new int[]
    //            {
    //                0b001,
    //                0b111,
    //                0b000,
    //            },
    //            new int[]
    //            {
    //                0b110,
    //                0b010,
    //                0b010,
    //            },
    //        },
    //        Width = 3,
    //        Height = 3,
    //        Name = "J",
    //    };
    //    public static readonly TetrisBitMino L = new()
    //    {
    //        _field = new int[][]
    //        {
    //            new int[]
    //            {
    //                0b000,
    //                0b111,
    //                0b001,
    //            },
    //            new int[]
    //            {
    //                0b011,
    //                0b010,
    //                0b010,
    //            },
    //            new int[]
    //            {
    //                0b100,
    //                0b111,
    //                0b000,
    //            },
    //            new int[]
    //            {
    //                0b010,
    //                0b010,
    //                0b110,
    //            },
    //        },
    //        Width = 3,
    //        Height = 3,
    //        Name = "L",
    //    };
    //    public static readonly TetrisBitMino S = new()
    //    {
    //        _field = new int[][]
    //        {
    //            new int[]
    //            {
    //                0b000,
    //                0b110,
    //                0b011,
    //            },
    //            new int[]
    //            {
    //                0b001,
    //                0b011,
    //                0b010,
    //            },
    //            new int[]
    //            {
    //                0b110,
    //                0b011,
    //                0b000,
    //            },
    //            new int[]
    //            {
    //                0b010,
    //                0b110,
    //                0b100,
    //            },
    //        },
    //        Width = 3,
    //        Height = 3,
    //        Name = "S",
    //    };
    //    public static readonly TetrisBitMino Z = new()
    //    {
    //        _field = new int[][]
    //        {
    //            new int[]
    //            {
    //                0b000,
    //                0b011,
    //                0b110,
    //            },
    //            new int[]
    //            {
    //                0b010,
    //                0b011,
    //                0b001,
    //            },
    //            new int[]
    //            {
    //                0b011,
    //                0b110,
    //                0b000,
    //            },
    //            new int[]
    //            {
    //                0b100,
    //                0b110,
    //                0b010,
    //            },
    //        },
    //        Width = 3,
    //        Height = 3,
    //        Name = "Z",
    //    };
    //    /// <summary>
    //    /// 第一维是旋转计数
    //    /// </summary>
    //    private int[][] _field { get; set; } = new int[4][];
    //    //public int[] Field { get => _field[Stage]; }

    //    //private (int X, int Y)[,] _kickTable;
    //    ///// <summary>
    //    ///// 旋转状态
    //    ///// </summary>
    //    //public int Stage = 0;
    //    /// <summary>
    //    /// 方块名字
    //    /// </summary>
    //    public string Name { get; private set; }
    //    public MinoType MinoType { get; private set; }
    //    /// <summary>
    //    /// 这个有可能非方形区域有问题
    //    /// </summary>
    //    public int Width { get; private set; }
    //    public int Height { get; private set; }
    //    //public TetrisBitMino(int[] field, int stage = 0)
    //    //{
    //    //    _field = new int[4][];
    //    //    _field[0] = (int[])field.Clone();
    //    //    // 必须一样
    //    //    Width = Height = field.Length;
    //    //}
    //    //public void LeftRoll()
    //    //{
    //    //    Stage += 3;
    //    //    Stage %= 4;
    //    //}
    //    //public void RightRoll()
    //    //{
    //    //    Stage++;
    //    //    Stage %= 4;
    //    //}public void _180Roll()
    //    //{
    //    //    Stage += 2;
    //    //    Stage %= 4;
    //    //}


    //}
    public class TetrisMino : ITetrisMino
    {
        /// <summary>
        /// 方块位置坐标
        /// </summary>
        public (int X, int Y)[][] _field { get; private set; }
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public MinoType MinoType { get; private set; }

        public static TetrisMino GetTetrisMino(MinoType minoType)
        {
            return minoType switch
            {
                MinoType.SC_I => I,
                MinoType.SC_J => J,
                MinoType.SC_L => L,
                MinoType.SC_O => O,
                MinoType.SC_S => S,
                MinoType.SC_T => T,
                MinoType.SC_Z => Z,
                _ => I
            };
        }

        public static readonly TetrisMino I = new()
        {
            _field = new (int, int)[][]
            {
                //0b0000,
                //0b0000,
                //0b1111,
                //0b0000,
                new[] { (2, 0), (2, 1), (2, 2), (2, 3) },
                //0b0010,
                //0b0010,
                //0b0010,
                //0b0010,
                new[] { (0, 2), (1, 2), (2, 2), (3, 2) },
                //0b0000,
                //0b1111,
                //0b0000,
                //0b0000,
                new[] { (1, 0), (1, 1), (1, 2), (1, 3) },
                //0b0100,
                //0b0100,
                //0b0100,
                //0b0100,
                new[] { (0, 1), (1, 1), (2, 1), (3, 1) },
            },
            Width = 4,
            Height = 4,
            Name = "I",
            MinoType = MinoType.SC_I,
        };
        public static readonly TetrisMino O = new()
        {
            _field = new (int, int)[][]
            {
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (1, 1), (1, 2), (2, 1), (2, 2) }, 
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (1, 1), (1, 2), (2, 1), (2, 2) }, 
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (1, 1), (1, 2), (2, 1), (2, 2) },
                //0b0000,
                //0b0110,
                //0b0110,
                //0b0000,
                new[] { (1, 1), (1, 2), (2, 1), (2, 2) }, 
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
        public static readonly TetrisMino T = new()
        {
            _field = new (int, int)[][]
            {
                
                //0b000,
                //0b111,
                //0b010,
                new [] { (1, 0), (1, 1), (1, 2), (2, 1) },
                //0b010,
                //0b011,
                //0b010,
                new [] { (0, 1), (1, 1), (1, 2), (2, 1) },
                //0b010,
                //0b111,
                //0b000,
                new [] { (0, 1), (1, 0), (1, 1), (1, 2) },
                //0b010,
                //0b110,
                //0b010,
                new [] { (0, 1), (1, 0), (1, 1), (2, 1) },
            },
            Width = 3,
            Height = 3,
            Name = "T",
            MinoType = MinoType.SC_T,
        };
        public static readonly TetrisMino J = new()
        {
            _field = new (int, int)[][]
            {
                //0b000,
                //0b111,
                //0b100,
                new [] { (1, 0), (1, 1), (1, 2), (2, 0) },
                //0b010,
                //0b010,
                //0b011,
                new [] { (0, 1), (1, 1), (2, 1), (2, 2) },
                //0b001,
                //0b111,
                //0b000,
                new [] { (0, 2), (1, 0), (1, 1), (1, 2) },
                //0b110,
                //0b010,
                //0b010,
                new [] { (0, 0), (0, 1), (1, 1), (2, 1) },
            },
            Width = 3,
            Height = 3,
            Name = "J",
            MinoType = MinoType.SC_J,
        };
        public static readonly TetrisMino L = new()
        {
            _field = new (int, int)[][]
            {
                //0b000,
                //0b111,
                //0b001,
                new [] { (1, 0), (1, 1), (1, 2), (2, 2) },
                //0b011,
                //0b010,
                //0b010,
                new [] { (0, 1), (0, 2), (1, 1), (2, 1) },
                //0b100,
                //0b111,
                //0b000,
                new [] { (0, 0), (1, 0), (1, 1), (1, 2) },
                //0b010,
                //0b010,
                //0b110,
                new [] { (0, 1), (1, 1), (2, 0), (2, 1) },
            },
            Width = 3,
            Height = 3,
            Name = "L",
            MinoType = MinoType.SC_L,
        };
        public static readonly TetrisMino S = new()
        {
            _field = new (int, int)[][]
            {
                //0b000,
                //0b110,
                //0b011,
                new [] { (1, 0), (1, 1), (2, 1), (2, 2) },
                //0b001,
                //0b011,
                //0b010,
                new [] { (0, 2), (1, 1), (1, 2), (2, 1) },
                //0b110,
                //0b011,
                //0b000,
                new [] { (0, 0), (0, 1), (1, 1), (1, 2) },
                //0b010,
                //0b110,
                //0b100,
                new [] { (0, 1), (1, 0), (1, 1), (2, 0) },
            },
            Width = 3,
            Height = 3,
            Name = "S",
            MinoType = MinoType.SC_S,
        };
        public static readonly TetrisMino Z = new()
        {
            _field = new (int, int)[][]
            {
                //0b000,
                //0b011,
                //0b110,
                new [] { (1, 1), (1, 2), (2, 0), (2, 1) },
                //0b010,
                //0b011,
                //0b001,
                new [] { (0, 1), (1, 1), (1, 2), (2, 2) },
                //0b011,
                //0b110,
                //0b000,
                new [] { (0, 1), (0, 2), (1, 0), (1, 1) },
                //0b100,
                //0b110,
                //0b010,
                new [] { (0, 0), (1, 0), (1, 1), (2, 1) },
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
                L,
                J,
                S,
                Z,
            };
        }
        public static ITetrisMino[] GetMinoListS()
        {
            return new[]
            {
                I,
                O,
                T,
                L,
                J,
                S,
                Z,
            };
        }
    }
}
