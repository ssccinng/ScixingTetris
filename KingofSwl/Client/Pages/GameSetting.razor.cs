using Microsoft.AspNetCore.Components.Web;

namespace KingofSwl.Client.Pages
{
    public partial class GameSetting
    {
        public KosSetting KosSetting;

        protected bool KeyDown(KeyboardEventArgs args, LinliuType type)
        {
            switch (type)
            {
                case LinliuType.Left:
                    KosSetting.Left = args.Code;
                    break;
                case LinliuType.Right:
                    KosSetting.Right = args.Code;
                    break;
                case LinliuType.SoftDrop:
                    KosSetting.SoftDrop = args.Code;
                    break;
                case LinliuType.SonicDrop:
                    KosSetting.SonicDrop = args.Code;
                    break;
                case LinliuType.HardDrop:
                    KosSetting.HardDrop = args.Code;
                    break;
                case LinliuType.LeftR:
                    KosSetting.LeftR = args.Code;
                    break;
                case LinliuType.RightR:
                    KosSetting.RightR = args.Code;
                    break;
                case LinliuType._180R:
                    KosSetting._180R = args.Code;
                    break;
                case LinliuType.Hold:
                    KosSetting.Hold = args.Code;
                    break;
                case LinliuType.Reset:
                    KosSetting.Reset = args.Code;
                    break;
                default:
                    break;
            }
            return false;
        }
    }

   
}
