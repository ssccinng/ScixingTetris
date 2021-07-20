using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScixingTetrisCore.Interface;
namespace ScixingTetrisCore.Tools
{
    public class TetrisMinoGenerator: ITetrisMinoGenerator
    {
        protected Random rnd;
        public IEnumerator<ITetrisMino> NextQueue;
        protected TetrisMinoGenerator(int? seed = null)
        {
            rnd = seed == null ? new Random() : new Random(seed.Value);
            //NextQueue = func();
        }
        public void SetSeed(int seed)
        {
            rnd = new Random(seed);
        }
        public ITetrisMino GetNextMino()
        {
            if (NextQueue == null) return null;
            NextQueue.MoveNext();
            var res = NextQueue.Current;
            return res;
        }
        //public abstract IEnumerator<ITetrisMino> GetNextMino();
    }

    public class Bag7Generator : TetrisMinoGenerator
    {
        public Bag7Generator(int? seed = null): base(seed)
        {
            NextQueue = GetNextQueue();
        }
        private IEnumerator<ITetrisMino> GetNextQueue()
        {
            // 要不要来个普通的mino?
            TetrisMino[] list = new []
            { 
                TetrisMino.I,
                TetrisMino.O,
                TetrisMino.T,
                TetrisMino.J,
                TetrisMino.L,
                TetrisMino.Z,
                TetrisMino.S,
            };
            while (true)
            {
                foreach (var res in list.OrderBy(s => rnd.Next()))
                {
                    yield return res;
                }
            }
        }
    }
}
