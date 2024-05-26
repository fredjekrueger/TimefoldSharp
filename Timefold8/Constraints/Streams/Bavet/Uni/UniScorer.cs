using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class UniScorer<A, Context_> : AbstractScorer<Context_>
        where Context_ : ScoreContext
    {

        private readonly Func<IWeightedScoreImpacter, A, UndoScoreImpacter> scoreImpacter;

        public UniScorer(IWeightedScoreImpacter weightedScoreImpacter, Func<IWeightedScoreImpacter, A, UndoScoreImpacter> scoreImpacter, int inputStoreIndex)
            :base(weightedScoreImpacter, inputStoreIndex)
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
