using System;

namespace Hikari.AI {
    public class HikariConfig {
        public int maxDepth = 128;
        public bool useHold = true;
        public bool useSpeculation = true;
        public int previews = 12;
        public bool singleThread = false;

        public void Validate() {
            if (maxDepth < 3) throw new Exception($"Max depth is too small: {maxDepth}");
            if (previews <= 0) throw new Exception($"Minimum valid preview is 1: {maxDepth}");
        }
    }
}