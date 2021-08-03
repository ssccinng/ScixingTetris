using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScixingTetrisCore.Interface
{
    /// <summary>
    /// TetrisAI接口
    /// </summary>
    public interface ITetrisAI
    {
        //ITetrisAI GetAI();
        bool IsDead { get; }
        void Init();

        TetrisAIMessage GetMove(int garbage = 0);
        void AddNext(ITetrisMino[] minoType);

        void SetPamras();
        void MoveNext();
        void ResetAI(int []Field, ITetrisMino[] NextQueue = null);
        //void
    }
}
