using ScixingTetrisCore;
using ScixingTetrisCore.Tools;
using System.Threading.Tasks;

namespace KingofSwl.Client.Pages
{
    public partial class TetrisTest
    {
        TetrisGameBoard _tetrisBoard = new(ShowHeight: 22, tetrisMinoGenerator: new Bag7Generator<TetrisMino>());

        protected override Task OnInitializedAsync()
        {
            _tetrisBoard.GameStart();
            return base.OnInitializedAsync();
        }
    }
}
