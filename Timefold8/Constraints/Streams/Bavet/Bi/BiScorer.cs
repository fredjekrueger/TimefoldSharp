using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public sealed class BiScorer<A, B> : AbstractScorer
    {

        private readonly Func<IWeightedScoreImpacter, A, B, UndoScoreImpacter> scoreImpacter;

        public BiScorer(IWeightedScoreImpacter weightedScoreImpacter, Func<IWeightedScoreImpacter, A, B, UndoScoreImpacter> scoreImpacter, int inputStoreIndex)
            : base(weightedScoreImpacter, inputStoreIndex)
        {
            this.scoreImpacter = scoreImpacter;
        }

        protected override UndoScoreImpacter Impact(ITuple t)
        {
            BiTuple<A, B> tuple = (BiTuple<A, B>)t;
            return scoreImpacter.Invoke(weightedScoreImpacter, tuple.factA, tuple.factB);
        }
    }
}
