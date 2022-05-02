using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public enum MoveType
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        LeftRotation,
        RightRotation,
        SoftDrop,
        SonicDrop,
        HardDrop,
    }
    public interface ITetrisMinoStatus
    {
        /// <summary>
        /// 方块位置
        /// </summary>
        (int X, int Y) Position { get; set; }
        int Stage { get; set; }
        int Kickcnt { get; set; }
        ITetrisMino TetrisMino { get; set; }
        // 欠考虑 可能要改
        bool LastRotation { get; set; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LeftRoll();
        void RightRoll();
        void _180Roll();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        void MoveLeft(int distance = 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        void MoveRight(int distance = 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        void MoveBottom(int distance = 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        void MoveTop(int distance = 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        (int X, int Y)[] GetMinoFieldListInBoard();

        public ITetrisMinoStatus Clone();

   
    }
}
