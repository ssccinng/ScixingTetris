using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    // 基础board接口
    public interface ITetrisBoard
    {

        int Width { get; }
        int Height { get; }
        /// <summary>
        /// 维护场地高度
        /// </summary>
        int [] ColHeight { get; }

        // rule在基础board里应该也是不用的
        //ITetrisRule TetrisRule { get; }
        /// <summary>
        /// 控制台输出场地
        /// </summary>
        void PrintBoard(bool WithMino = false, int printLeft = 0, int printTop = 0);

        /// <summary>
        /// 判断该位置是否为空
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool IsCellFree(int x, int y);

        /// <summary>
        /// 锁定方块
        /// </summary>
        /// <returns></returns>
        bool LockMino();
        
        

    }
}
