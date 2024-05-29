using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class UniScorer<A> : AbstractScorer
    {

        private readonly Func<IWeightedScoreImpacter, A, UndoScoreImpacter> scoreImpacter;

        public UniScorer(IWeightedScoreImpacter weightedScoreImpacter, Func<IWeightedScoreImpacter, A, UndoScoreImpacter> scoreImpacter, int inputStoreIndex)
            : base(weightedScoreImpacter, inputStoreIndex)
        {
            this.scoreImpacter = scoreImpacter;
        }

        protected override UndoScoreImpacter Impact(ITuple t)
        {
            UniTuple<A> tuple = (UniTuple<A>)t;
            return scoreImpacter.Invoke(weightedScoreImpacter, tuple.factA);
        }
    }
}
