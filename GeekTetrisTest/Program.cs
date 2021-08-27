using System;
using ScixingTetrisCore;
namespace GeekTetrisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            TetrisAIGeekGame tetrisAIGeekGame = new(new GeekTetrisAI(), new GeekTetrisAIBoard());
            tetrisAIGeekGame.GameStart();
        }
    }
}
