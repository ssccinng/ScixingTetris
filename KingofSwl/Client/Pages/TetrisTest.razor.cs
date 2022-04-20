using ScixingTetrisCore;
using ScixingTetrisCore.Tools;
using System.Threading.Tasks;

namespace KingofSwl.Client.Pages
{
    public partial class TetrisTest
    {
        
        KosTetrisGameBoard _tetrisBoard = new(ShowHeight: 24, tetrisMinoGenerator: new Bag7Generator<TetrisMino>());
        string[] _colorTable = new[]
        {
            "white",
            "#33a3dc",
            "#ffe600",
            "#7d5886",
            "#e0861a",
            "#145b7d",
            "#7fb80e",
            "#d71345",
            "",
            "",
            "",
            "#33a3dc3f",
            "#ffe6003f",
            "#7d58863f",
            "#e0861a3f",
            "#145b7d3f",
            "#7fb80e3f",
            "#d713453f",

        };
        protected override Task OnInitializedAsync()
        {
            _tetrisBoard.GameStart();
            field = _tetrisBoard.GetGameField();
            testControl = new TestControl(_tetrisBoard);
            
            testControl.NextF += () => this.InvokeAsync(() => { field = _tetrisBoard.GetGameField(); this.StateHasChanged(); });
            return base.OnInitializedAsync();
        }
    }
}
