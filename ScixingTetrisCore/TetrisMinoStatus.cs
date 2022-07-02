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
        public bool LastRotation { get; set; }
        public int Kickcnt { get; set; }

        // 内部坐标
        protected int _x, _y;
        public void LeftRoll()
        {
            Stage += 3;
            Stage &= 3;
        }
        public void RightRoll()
        {
            Stage++;
            Stage &= 3;
        }
        public void _180Roll()
        {
            Stage += 2;
            //Stage %= 4;
            Stage &= 3;
        }
        public void MoveLeft(int distance = 1)
        {
            _y -= distance;
        }
        public void MoveRight(int distance = 1)
        {
            _y += distance;
        }
        public void MoveBottom(int distance = 1)
        {
            _x -= distance;
        }
        public void MoveTop(int distance = 1)
        {
            _x += distance;
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
        
        public ITetrisMinoStatus Clone()
        {
            var res = new TetrisMinoStatus();
            res.TetrisMino = this.TetrisMino;
            res.Position = this.Position;
            res.Stage = this.Stage;
            return res;
        }

        
    }
}
