namespace Hikari.AI.Eval {
    public readonly struct TspinHole {
        public readonly int line;
        public readonly int x;
        public readonly int y;
        public readonly int r;

        public TspinHole(int line, int x, int y, int r) {
            this.line = line;
            this.x = x;
            this.y = y;
            this.r = r;
        }

        public override string ToString() {
            return $"TspinHole at ({x}, {y})#{r} of {line} line(s)";
        }
    }
}