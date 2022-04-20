using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface ITetrisMinoStatus
    {
        /// <summary>
        /// 方块位置
        /// </summary>
        (int X, int Y) Position { get; set; }
        int Stage { get; set; }
        ITetrisMino TetrisMino { get; set; }
        // 欠考虑 可能要改
        bool LastRotation { get; set; }
        void LeftRoll();
        void RightRoll();
        void _180Roll();

        void MoveLeft(int distance = 1);
        void MoveRight(int distance = 1);
        void MoveBottom(int distance = 1);
        void MoveTop(int distance = 1);

        (int X, int Y)[] GetMinoFieldListInBoard();

        public ITetrisMinoStatus Clone();

   
    }
}
