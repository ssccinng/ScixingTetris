using Microsoft.AspNetCore.Components.Web;
using ScixingTetrisCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KingofSwl.Client.Pages
{
    public partial class KosTetrisClient
    {
        public KosSetting KosSetting { get; set; }
        public KosClientBoard _tetrisBoard { get; set; }
        //public KosClientBoard _tetrisBoard { get; set; } = new(ShowHeight: 25, tetrisMinoGenerator: new Bag7Generator<TetrisMino>());

        string KeyPressed = "";
        /// <summary>
        /// 场地
        /// </summary>
        public byte[][] field;

        /// <summary>
        /// hold块
        /// </summary>
        public byte[][] holdField;
        /// <summary>
        /// next块
        /// </summary>
        public List<byte[][]> nextFields;
        TestControl testControl;
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

            //_tetrisBoard.GameStart();
            //field = _tetrisBoard.GetGameField();
            //holdField = _tetrisBoard.GetHoldField();
            //nextFields = _tetrisBoard.GetNextQueueField();
           
            isLoading = false;
            return;
        }

        public void UpdateClient()
        {
            testControl = new TestControl(_tetrisBoard);
            testControl.das = KosSetting.Das;
            testControl.arr = KosSetting.Arr;
            testControl.ss = KosSetting.SoftDropSpeed;

            testControl.NextF += () => this.InvokeAsync(() => {
                if (this._tetrisBoard != null)
                {
                    field = _tetrisBoard.GetGameField();
                    holdField = _tetrisBoard.GetHoldField();
                    nextFields = _tetrisBoard.GetNextQueueField();

                    this.StateHasChanged();
                }

            });
        }
        protected void KeyDown(KeyboardEventArgs args)
        {
            if (args.Repeat) return;
            var keycode = args.Code;
            if (keycode == KosSetting.Left)
            {
                testControl.SetKeyStatus(LinliuType.Left, true);
            }
            else if (keycode == KosSetting.Right)
            {
                testControl.SetKeyStatus(LinliuType.Right, true);
            }
            else if (keycode == KosSetting.SoftDrop && !args.Repeat)
            {
                testControl.SetKeyStatus(LinliuType.SoftDrop, true);
            }
            else if (keycode == KosSetting.LeftR)
            {
                _tetrisBoard.LeftRotation();
            }
            else if (keycode == KosSetting.RightR)
            {
                _tetrisBoard.RightRotation();
            }
            else if (keycode == KosSetting._180R)
            {
                _tetrisBoard._180Rotation();
            }
            else if (keycode == KosSetting.HardDrop && !args.Repeat)
            {

                _tetrisBoard.HardDrop();

            }
            else if (keycode == KosSetting.SonicDrop)
            {
                _tetrisBoard.SonicDrop();
            }
            else if (keycode == KosSetting.Hold)
            {
                _tetrisBoard.OnHold();
            }
            else if (keycode == KosSetting.Reset)
            {
                _tetrisBoard.ResetGame();

            }
            KeyPressed = $"Key Pressed: [{args.Key}]";// get key pressed in the arguments
                                                      //field = _tetrisBoard.GetGameField();
        }
        protected void Keyup(KeyboardEventArgs args)
        {
            var keycode = args.Code;
            if (keycode == KosSetting.Left)
            {
                testControl.SetKeyStatus(LinliuType.Left, false);
            }
            else if (keycode == KosSetting.Right)
            {
                testControl.SetKeyStatus(LinliuType.Right, false);
            }
            else if (keycode == KosSetting.SoftDrop)
            {
                testControl.SetKeyStatus(LinliuType.SoftDrop, false);
            }
            KeyPressed = $"Key Pressed: [{args.Key}]";// get key pressed in the arguments
                                                      //field = _tetrisBoard.GetGameField();
        }
    }
}
