using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class KosTetrisGameBoard: TetrisGameViewBoard
    {
        public KosTetrisGameBoard(int Width = 10, int Height = 40, int ShowHeight = 20, ITetrisRule tetrisRule = null, ITetrisMinoGenerator tetrisMinoGenerator = null) :
           base(Width, Height, ShowHeight, tetrisRule, tetrisMinoGenerator)
        { }
    }
}
