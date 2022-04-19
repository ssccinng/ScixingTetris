namespace Hikari.AI.Eval {
    /// <summary>
    /// Reward is the score of a move.
    /// <para/> This is also the score of an edge of graph.
    /// </summary>
    public struct Reward {
        public int evaluation;
        public int attack;
        
        // Setting -1 to attack will discontinue spike, but 0 won't
        // So we can use -1 as no line clear placements and 0 as hold only moves
        
        public static Reward operator *(Reward lhs, int rhs) {
            return new Reward {
                evaluation = lhs.evaluation * rhs
            };
        }

        public static Reward operator /(Reward lhs, int rhs) {
            return new Reward {
                evaluation = lhs.evaluation / rhs
            };
        }
    }
}