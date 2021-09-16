using ScixingTetrisCore;
using ScixingTetrisCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingofSwl.Client.Pages
{
    public partial class PersonTrainer
    {
        TetrisGameBoard tetrisBoard = new(ShowHeight: 22, tetrisMinoGenerator: new Bag7Generator<TetrisMino>());


    }
}
