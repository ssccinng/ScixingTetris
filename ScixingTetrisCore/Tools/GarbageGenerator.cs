using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScixingTetrisCore.Interface;

namespace ScixingTetrisCore.Tools
{
    public abstract class GarbageGenerator : IGarbageGenerator
    {
        protected readonly Random _rnd;
        public GarbageGenerator(int? seed = null)
        {
            _rnd = seed == null ? new Random() : new Random(seed.Value);
        }
        public abstract List<int> GetBitGarbage(List<int> GarbageList);
        public abstract List<byte[]> GetGarbage(List<int> GarbageList);
    }

    public class GuildLineGG : GarbageGenerator
    {
        public override List<int> GetBitGarbage(List<int> GarbageList)
        {
            List<int> list = new List<int>();
            foreach (var garbage in GarbageList)
            {
                list.Add(~(1 << _rnd.Next(10)));
            }
            return list;
            //throw new NotImplementedException();
        }

        public override List<byte[]> GetGarbage(List<int> GarbageList)
        {
            throw new NotImplementedException();
        }
    }
}
