using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface ITetrisBoard
    {
        ITetrisRule TetrisRule { get; }
        /// <summary>
        /// 控制台输出场地
        /// </summary>
        void PrintBoard(bool WithMino, int printLeft, int printTop);
        bool LockMino();
        bool IsCellFree(int x, int y);
    }
}
