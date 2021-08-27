using ScixingTetrisCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore
{
    public class GeekWeight
    {
        // 消1 2 3 4
        public int[] clear;
        // 每行的高度权
        public int[] ColHeight = new int[] { 50, 15, 15, 10, 10, 10, 10, 9, 8, 0 };
        public int Height = 20;
        public int Cell = 200;

    }
    public class GeekFieldEvaluator : IFieldEvaluator
    {
        GeekWeight geekWeight = new GeekWeight();
        /// <summary>
        /// 场地评分
        /// </summary>
        /// <param name="tetrisGameBoard"></param>
        /// <returns></returns>
        public double GetFieldScore(ITetrisAIBoard tetrisGameBoard)
        {
            double score = 0;
            var Board = (tetrisGameBoard as GeekTetrisAIBoard);
            for (int i = 0; i < Board.Width; ++i)
            {
                // 场地高度分数 然后还要有洞的分
                score += geekWeight.ColHeight[i] * Board.ColHeight[i];
            }
            int maxHeight = Board.ColHeight.Max();
            // 高度问题
            if (maxHeight > 15) score += -geekWeight.Height * maxHeight;
            else score += geekWeight.Height * maxHeight;
            for (int i = 0; i < tetrisGameBoard.Width; ++i)
            {
                for (int j = 0; j < tetrisGameBoard.ColHeight[i]; ++j)
                {
                    if (tetrisGameBoard.IsCellFree(j, i))
                    {
                        score += -geekWeight.Cell;
                        if (!tetrisGameBoard.IsCellFree(j, i + 1)) score += -geekWeight.Cell;
                        if (!tetrisGameBoard.IsCellFree(j, i - 1)) score += -geekWeight.Cell;
                    }

                }
            }

            return score;
        }
        /// <summary>
        /// 行为评分
        /// </summary>
        /// <returns></returns>
        public double GetMoveScore()
        {

            return 0;
        }
    }
}
