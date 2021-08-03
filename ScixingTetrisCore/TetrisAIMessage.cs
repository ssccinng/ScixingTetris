using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public enum TetrisMovement
    {
        Left,
        Left3,
        HugeLeft,
        Right,
        Right3,
        HugeRight,
        CW,
        CCW,
        _180,
        SoftDrop,
        SonicDrop,
        HardDrop,
        Wait,
    }
    // 加个wait(?
    public class TetrisAIMessage
    {
        public List<TetrisMovement> Path { get; } = new(); // 路径? 可能不要string比较好
        //public ITetrisMino TetrisMino;
        // 方块最终状态
        public ITetrisMinoStatus TetrisMinoStatus;
        public bool Hold;
        // 需要一个ai状态（？
        public string Pathstr
        {
            get
            {
                StringBuilder res = new StringBuilder();
                for (int i = 0; i < Path?.Count; ++i)
                {
                    res.Append(Path[i] switch
                    {
                        TetrisMovement.Left => 'l',
                        TetrisMovement.Left3 => "lll",
                        TetrisMovement.HugeLeft => 'L',
                        TetrisMovement.Right => 'r',
                        TetrisMovement.Right3 => "rrr",
                        TetrisMovement.HugeRight => "R",
                        TetrisMovement.CW => 'z',
                        TetrisMovement.CCW => 'x',
                        TetrisMovement._180 => 'c',
                        TetrisMovement.SoftDrop => 's',
                        TetrisMovement.SonicDrop => 'S',
                        TetrisMovement.HardDrop => 'w',
                        _ => "",
                    }); 
                }
                return res.ToString();
            }
        }
    }
}
