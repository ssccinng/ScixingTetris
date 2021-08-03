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
        readonly Random rnd;
        public abstract List<int> GetBitGarbage(List<int> GarbageList);
        public abstract List<byte[]> GetGarbage(List<int> GarbageList);
    }

    public class GuildLineGG : GarbageGenerator
    {
        public override List<int> GetBitGarbage(List<int> GarbageList)
        {
            throw new NotImplementedException();
        }

        public override List<byte[]> GetGarbage(List<int> GarbageList)
        {
            throw new NotImplementedException();
        }
    }
}
