using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public enum ClearType
    {
        None = 1 << 0,
        Tspin = 1 << 1,
        Minispin = 1 << 2,
        Spin = 1 << 3,
        B2B = 1 << 4,
    }
    /// <summary>
    /// 攻击信息
    /// </summary>
    public class ClearMessage
    {
        public bool[] ClearFlag;
        public ITetrisMinoStatus tetrisMinoStatus;
        /// <summary>
        /// 是否全消
        /// </summary>
        public bool IsPerfectClear;
        /// <summary>
        /// 连击数
        /// </summary>
        public int Combo;
        /// <summary>
        /// 此次消除行数
        /// </summary>
        public int ClearRows;
        /// <summary>
        /// 消除类型
        /// </summary>
        public ClearType ClearType = ClearType.None;
        /// <summary>
        /// B2B计数
        /// </summary>
        public int B2B;
        //public int Kickcnt;
    }
}
