using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScixingTetrisCore.Interface;

namespace ScixingTetrisCore.AI
{
    public class TetrisTreeNode
    {
        ITetrisNodeBoard TetrisNodeBoard;
        // 节点信息
        
        // 深度？
        public int Depth { get; }
        public bool IsExtend; // 需要吗（？
        // 节点评分
        public int Score;
        public int ChildBestScore;
        public TetrisTreeNode Parent;
        public List<TetrisTreeNode> ChirdList;

        public void Extend()
        {

            // Getchild
        }
    }
}
