using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScixingTetrisCore.Battle
{
    
    public class KosBattle
    {
        public KosServerBoard Player1 { get; set; }
        public KosServerBoard Player2 { get; set; }

        
        /// <summary>
        /// 谁的回合
        /// </summary>
        public int WhoTurn { get; set; }
        /// <summary>
        /// 还剩几次操作
        /// </summary>
        public int MoveRemain { get; set; } = 7;
        public KosBattle()
        {
            Player1 = new ();
            Player2 = new ();
            Player2.GameStart();
            Player1.GameStart();
            // 检测死亡
            Player1.OnAtk += Player2.ReceiveGarbage;
            Player2.OnAtk += Player1.ReceiveGarbage;
        }
        public bool CommitMove(List<List<MoveType>> moveTypes)
        {
            if (moveTypes.Count > MoveRemain) return false;
            if (WhoTurn == 0)
            {
                for (int i = 0; i < moveTypes.Count; i++)
                {
                    foreach (var move in moveTypes[i])
                    {
                        switch (move)
                        {
                            case MoveType.MoveLeft:
                                Player1.MoveLeft();
                                break;
                            case MoveType.MoveRight:
                                Player1.MoveRight();
                                break;
                            case MoveType.MoveUp:
                                Player1.MoveUp();
                                break;
                            case MoveType.LeftRotation:
                                Player1.LeftRotation();
                                break;
                            case MoveType.RightRotation:
                                Player1.RightRotation();
                                break;
                            case MoveType.SoftDrop:
                                Player1.SoftDrop();
                                break;
                            case MoveType.SonicDrop:
                                Player1.SonicDrop();
                                break;
                            case MoveType.HardDrop:
                                Player1.HardDrop();
                                break;
                            default:
                                break;
                        }
                    }
                    --MoveRemain;
                }
            }
            else
            {

            }
            if (MoveRemain == 0)
            {
                WhoTurn ^= 1;
            }
            return true;
        }
    }
}
