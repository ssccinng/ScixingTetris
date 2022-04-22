using ScixingTetrisCore;
using ScixingTetrisCore.Tools;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KingofSwl.Client.Pages
{
    public partial class TetrisTest
    {
        public KosSetting KosSetting { get; set; }
        Stopwatch stopWatch =  new Stopwatch();
        public KosTetrisGameBoard _tetrisBoard = new(ShowHeight: 25, tetrisMinoGenerator: new Bag7Generator<TetrisMino>());
        string[] _colorTable = new[]
        {
            "white",
            //"Translate",
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
        bool _isLoading = true;
        protected override async Task OnInitializedAsync()
        {
            var cookieContent = await localStorage.GetItemAsync<KosSetting>("KSetting");

            if (cookieContent == null)
            {
                KosSetting = new KosSetting();
            }
            else
            {
                KosSetting = cookieContent;
            }
            
            _tetrisBoard.GameStart();
            field = _tetrisBoard.GetGameField();
            holdField = _tetrisBoard.GetHoldField();
            nextFields = _tetrisBoard.GetNextQueueField();
            testControl = new TestControl(_tetrisBoard);
            testControl.das = KosSetting.Das;
            testControl.arr = KosSetting.Arr;
            testControl.ss = KosSetting.SoftDropSpeed;
            
            testControl.NextF += () => this.InvokeAsync(() => {
                if (Canuse) {
                    field = _tetrisBoard.GetGameField();
                    holdField = _tetrisBoard.GetHoldField();
                    nextFields = _tetrisBoard.GetNextQueueField();
                }
               
                this.StateHasChanged(); });
            _isLoading = false;
            return;
        }
    }
}
