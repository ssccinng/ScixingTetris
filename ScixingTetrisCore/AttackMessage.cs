using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public enum ClearType
    {
        None,
        Tspin,
        MiniTspin,
        Spin,
    }
    /// <summary>
    /// 攻击信息
    /// </summary>
    public class AttackMessage
    {
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
        public ClearType ClearType;
        /// <summary>
        /// B2B计数
        /// </summary>
        public int B2B;
    }
}
