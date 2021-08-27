using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class TetrisAIGeekGame
    {
        public ITetrisAI TetrisAI;

        // 完全的工具board 不有任何多于操作
        public GeekTetrisAIBoard TetrisGameBoard;

        public TetrisAIMessage TetrisAIMessage { get; private set; }

        public void Init()
        {
            // Geek 输入一万个next
            TetrisAI.Init();
        }

        public TetrisAIGeekGame(ITetrisAI tetrisAI, GeekTetrisAIBoard tetrisGameBoard)
        {
            TetrisAI = tetrisAI;
            TetrisGameBoard = tetrisGameBoard;
        }
        public void GameStart()
        {
            // 准备
            Init();
            while (!TetrisAI.IsDead)
            {
                //if (Console.KeyAvailable)
                //{
                //    ConsoleKeyInfo key = Console.ReadKey(true);
                //    Console.WriteLine(key.Key);
                //}
                TetrisGameBoard.PrintBoard();
                Thread.Sleep(500);
                TetrisAIMessage tetrisAIMessage  = TetrisAI.GetMove();
                if (tetrisAIMessage == null) continue;
                TetrisAI.MoveNext();
                TetrisGameBoard.LockMino(tetrisAIMessage.TetrisMinoStatus);

                //for (int i = 0; i < tetrisAIMessage.Path.Count; ++i)
                //{
                //    switch (tetrisAIMessage.Path[i])
                //    {
                //        case TetrisMovement.Left:
                //            TetrisGameBoard.MoveLeft();
                //            break;
                //        case TetrisMovement.Left3:
                //            break;
                //        case TetrisMovement.HugeLeft:
                //            break;
                //        case TetrisMovement.Right:
                //            TetrisGameBoard.MoveRight();
                //            break;
                //        case TetrisMovement.Right3:
                //            break;
                //        case TetrisMovement.HugeRight:
                //            break;
                //        case TetrisMovement.CW:
                //            break;
                //        case TetrisMovement.CCW:
                //            break;
                //        case TetrisMovement._180:
                //            break;
                //        case TetrisMovement.SoftDrop:
                //            TetrisGameBoard.SoftDrop();
                //            break;
                //        case TetrisMovement.SonicDrop:
                //            TetrisGameBoard.SonicDrop();
                //            break;
                //        case TetrisMovement.HardDrop:
                //            TetrisGameBoard.HardDrop();
                //            break;
                //        case TetrisMovement.Wait:
                //            break;
                //        default:
                //            break;
                //    }
                //}
                // 执行
            }

            // 输出分数
        }
    }
}
