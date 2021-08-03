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

        void LeftRoll();
        void RightRoll();

        void MoveLeft();
        void MoveRight();
        void MoveBottom();
        void MoveTop();

        (int X, int Y)[] GetMinoFieldListInBoard();

        public ITetrisMinoStatus Clone();

   
    }
}
