using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScixingTetrisCore.Interface;

namespace ScixingTetrisCore
{
    public class TetrisMinoStatus : ITetrisMinoStatus
    {
        /// <summary>
        /// 是不是不太好
        /// </summary>
        public (int X, int Y) Position { get => (_x, _y); set { _x = value.X; _y = value.Y; } }
        
        public int Stage { get; set; }
        /// <summary>
        /// 方块模板数据
        /// </summary>
        public ITetrisMino TetrisMino { get; set; }

        // 内部坐标
        protected int _x, _y;
        public virtual void LeftRoll()
        {
            Stage += 3;
            Stage %= 4;
        }
        public virtual void RightRoll()
        {
            Stage++;
            Stage %= 4;
        }
        public virtual void MoveLeft()
        {
            _y -= 1;
        }
        public virtual void MoveRight()
        {
            _y += 1;
        }
        public virtual void MoveBottom()
        {
            _x -= 1;
        }
        public virtual void MoveTop()
        {
            _x += 1;
        }

        public (int, int)[] GetMinoFieldListInBoard()
        {
            var res = TetrisMino.GetMinoField(Stage);
            for (int i = 0; i < res.Length; ++i)
            {
                res[i] = (res[i].X + _x, res[i].Y + _y);
            }
            return res;
        }
    }
}
