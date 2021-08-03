using ScixingTetrisCore.AI;
using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    // 如何配置规则（？ 如何配置不同方块
    public class TetrisAI : ITetrisAI
    {
        public TetrisTreeNode SearchTree;
        // Next表 以供查询
        private List<ITetrisMino> tetrisMinos;

        public bool IsDead => throw new NotImplementedException();

        public TetrisAIMessage GetMove(int garbage)
        {
            TetrisAIMessage res = new ();
            // 这个根据需要可以改一下 毕竟一直需要 也许可以改成只读的 然后添加
            return res;
        }
        public void MoveNext()
        {
            // 放弃首位 选择最后一次GetMove获得的结果，若没有Get过则默认放弃一个
        }
        public void AddNext(ITetrisMino[] minoType)
        {
            tetrisMinos.AddRange(minoType);
        }
        public void AddNext(string NextQueue)
        {
            for (int i = 0; i < NextQueue.Length; ++i)
            {
                // 从NextQueue获取
            }
        }



        /// <summary>
        /// 重置场地
        /// </summary>
        public void ResetAI()
        {

        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void ResetAI(int[] Field, ITetrisMino[] NextQueue = null)
        {
            throw new NotImplementedException();
        }

        public void SetPamras()
        {
            throw new NotImplementedException();
        }
    }
}
