using Hikari.Puzzle;
using Unity.Burst;

namespace Hikari.AI.Eval {
    public interface IEvaluator {
        public Value EvaluateBoard([NoAlias] in SimpleColBoard board, [NoAlias] in Weights weights);

        public Reward EvaluateMove(int time, Piece piece,
            [NoAlias] in SimpleColBoard board, [NoAlias] in SimpleLockResult lockResult,
            bool parentIsB2B, [NoAlias] in Weights weights);
    }
}