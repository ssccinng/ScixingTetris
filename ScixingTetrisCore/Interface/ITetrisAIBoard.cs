using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface ITetrisAIBoard: ITetrisRuleBoard
    {
        //锁定其他方块
        bool LockMino(ITetrisMinoStatus tetrisMino);
        bool LeftRotation(ITetrisMinoStatus tetrisMino);
        /// <summary>
        /// 右旋
        /// </summary>
        /// <returns></returns>
        bool RightRotation(ITetrisMinoStatus tetrisMino);
        /// <summary>
        /// 180转
        /// </summary>
        /// <returns></returns>
        bool _180Rotation(ITetrisMinoStatus tetrisMino);
        /// <summary>
        /// 左移
        /// </summary>
        /// <returns></returns>
        bool MoveLeft(ITetrisMinoStatus tetrisMino);
        /// <summary>
        /// 右移
        /// </summary>
        /// <returns></returns>
        bool MoveRight(ITetrisMinoStatus tetrisMino);
        /// <summary>
        /// 软降
        /// </summary>
        bool SoftDrop(ITetrisMinoStatus tetrisMino);
        /// <summary>
        /// 软降到底
        /// </summary>
        bool SonicDrop(ITetrisMinoStatus tetrisMino);
        /// <summary>
        /// 硬降（软降到底并锁定）
        /// </summary>
        /// <returns></returns>
        bool HardDrop(ITetrisMinoStatus tetrisMino);
        bool ResetBoard(byte[][] field);
    }
}
