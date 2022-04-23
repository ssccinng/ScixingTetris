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
        public static readonly GuildLineGG GuildLineGG = new GuildLineGG();

        protected readonly Random _rnd;
        public int GarbageColor;
        public int Width;
        public GarbageGenerator(int? seed = null, int garbageColor = 8, int width = 10)
        {
            _rnd = seed == null ? new Random() : new Random(seed.Value);
            GarbageColor = garbageColor;
            Width = width;
        }
        public abstract List<int> GetBitGarbage(List<int> GarbageList);
        public abstract List<byte[]> GetGarbage(List<int> GarbageList);
    }

    public class GuildLineGG : GarbageGenerator
    {
        public override List<int> GetBitGarbage(List<int> GarbageList)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < GarbageList.Count; ++i)
            {
                int gField = ~(1 << _rnd.Next(Width));
                for (int j = 0; j < GarbageList[i]; ++j)
                {
                    //list.Add(~(1 << _rnd.Next(10)));
                    list.Add(gField);
                }
                
            }
            //foreach (var garbage in GarbageList)
            //{
            //    int gField = ~(1 << _rnd.Next(10));
            //    for (int i = 0; i < garbage; i++)
            //    {
            //        list.Add(gField); 
            //    }
            //}
            return list;
            //throw new NotImplementedException();
        }

        public override List<byte[]> GetGarbage(List<int> GarbageList)
        {
            List<byte[]> list = new List<byte[]>();
            for (int i = 0; i < GarbageList.Count; i++)
            {
                var rnd = _rnd.Next(Width);
                for (int j = 0; j < GarbageList[i]; ++j)
                {
                    // 这个其实要根据width来
                    byte[] b = new byte[Width];
                    for (int k = 0; k < Width; k++)
                    {
                        b[k] = (byte)(k == rnd ? 0 : GarbageColor);
                    }
                    list.Add(b);
                }
            }
            return list;
            //throw new NotImplementedException();
        }
    }
}
