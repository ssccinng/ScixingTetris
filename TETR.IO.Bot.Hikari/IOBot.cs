using Carter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TETR.IO.Bot.Hikari
{
    public class MoveResult
    {
        public bool hold { get; set; }
        public List<string> moves { get; set; } = new List<string>();
        public int[][] expected_cells { get; set; }

    }
    public class IOBot: CarterModule
    {
        
    }
}
