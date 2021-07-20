using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    interface ITetrisGame
    {
        /// <summary>
        /// 游戏开始
        /// </summary>
        void GameStart();
        /// <summary>
        /// 游戏结束
        /// </summary>
        void GameEnd();
    }
}
