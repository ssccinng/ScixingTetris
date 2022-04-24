using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    /// <summary>
    /// 游戏board接口
    /// </summary>
    public interface ITetrisGameBoard: ITetrisRuleBoard
    {
        
        // 需要吗内联吗
        /// <summary>
        /// Next表
        /// </summary>
        Queue<ITetrisMino> NextQueue { get; }
        // hold
        /// <summary>
        /// 左旋
        /// </summary>
        /// <returns></returns>
        bool LeftRotation();
        /// <summary>
        /// 右旋
        /// </summary>
        /// <returns></returns>
        bool RightRotation();
        /// <summary>
        /// 180转
        /// </summary>
        /// <returns></returns>
        bool _180Rotation();
        
        /// <summary>
        /// 左移
        /// </summary>
        /// <returns></returns>
        bool MoveLeft();
        /// <summary>
        /// 右移
        /// </summary>
        /// <returns></returns>
        bool MoveRight();
        /// <summary>
        /// 上移
        /// </summary>
        /// <returns></returns>
        bool MoveUp();
        /// <summary>
        /// 软降
        /// </summary>
        bool SoftDrop();
        /// <summary>
        /// 软降到底
        /// </summary>
        bool SonicDrop();
        /// <summary>
        /// 硬降（软降到底并锁定）
        /// </summary>
        /// <returns></returns>
        bool HardDrop();
        /// <summary>
        /// 按hold
        /// </summary>
        /// <returns></returns>
        bool OnHold();
        /// <summary>
        /// 生成新方块
        /// </summary>
        /// <returns></returns>
        bool SpawnNewPiece();

        bool IsCellFreeWithMino(int x, int y);

        void ResetGame();

        void ReceiveGarbage(List<int> garbages);
        //void AddField(List<byte[]> field);
        
    }
}
