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
        public int[] ColHeight;
        public int Height;

    }
    public class GeekFieldEvaluator : IFieldEvaluator
    {


        public double GetFieldScore(ITetrisGameBoard tetrisGameBoard)
        {
            throw new NotImplementedException();
        }
    }
}
