using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class TetrisGameViewBoard : TetrisGameBoard, IViewBoard
    {
        public TetrisGameViewBoard(int Width = 10, int Height = 40, int ShowHeight = 20, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null):
            base (Width, Height, ShowHeight, tetrisRule, tetrisMinoGenerator)
        { }

        

        public byte[][] GetGameField()
        {
            byte[][] gameField = new byte[ShowHeight][];
            for (int i = 0; i < ShowHeight; ++i)
            {
                gameField[i] = new byte[Width];
                int pi = ShowHeight - 1 - i;
                for (int j = 0; j < Width; ++j)
                {
                    gameField[i][j] = Field[i][j];
                }

            }

            
            if (TetrisMinoStatus != null)
            {
                ITetrisMinoStatus ghost = new TetrisMinoStatus { Position = TetrisMinoStatus.Position, Stage = TetrisMinoStatus.Stage, TetrisMino = TetrisMinoStatus.TetrisMino };
                while (true)
                {
                    ghost.MoveBottom();
                    if (!TetrisRule.CheckMinoOk(this, ghost))
                    {
                        break;
                    }
                }
                ghost.MoveTop();
                foreach (var pos in ghost?.GetMinoFieldListInBoard())
                {
                    // pos 要在显示区域内
                    // 肯定有问题.jpg
                    //Console.SetCursorPosition(printLeft + 1 + pos.Y * 2, tempTop + (ShowHeight - pos.X));
                    gameField[pos.X][pos.Y] = (byte)(ghost.TetrisMino.MinoType + 11);
                }
                foreach (var pos in TetrisMinoStatus?.GetMinoFieldListInBoard())
                {
                    // pos 要在显示区域内
                    // 肯定有问题.jpg
                    gameField[pos.X][pos.Y] = (byte)(ghost.TetrisMino.MinoType + 1);
                    //Console.Write("[]");
                }
            }
            return gameField;
        }

        public virtual List<byte[][]> GetNextQueueField()
        {
            throw new NotImplementedException();
        }

        public virtual byte[][] GetHoldField()
        {
            throw new NotImplementedException();

        }
    }
}
