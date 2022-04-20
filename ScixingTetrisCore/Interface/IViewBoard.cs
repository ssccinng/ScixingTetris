using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    public interface IViewBoard
    {
        /// <summary>
        /// 获取带色场地
        /// </summary>
        /// <returns></returns>
        byte[][] GetGameField();
        /// <summary>
        /// 获取hold
        /// </summary>
        /// <returns></returns>
        byte[][] GetHoldField();
        /// <summary>
        /// 获取next的场地
        /// </summary>
        /// <returns></returns>
        List<byte[][]> GetNextQueueField();

    }
}
