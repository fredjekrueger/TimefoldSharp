using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class AbstractScorer : TupleLifecycle
    {

        protected IWeightedScoreImpacter weightedScoreImpacter;
        private int inputStoreIndex;

        protected AbstractScorer(IWeightedScoreImpacter weightedScoreImpacter, int inputStoreIndex)
        {
            this.weightedScoreImpacter = weightedScoreImpacter;
            this.inputStoreIndex = inputStoreIndex;
        }

        public void Insert(ITuple tuple)
        {
            tuple.SetStore(inputStoreIndex, Impact(tuple));
        }

        protected abstract UndoScoreImpacter Impact(ITuple tuple);

        public void Retract(ITuple tuple)
        {
            UndoScoreImpacter undoScoreImpacter = tuple.GetStore<UndoScoreImpacter>(inputStoreIndex);
            // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
            if (undoScoreImpacter != null)
            {
                undoScoreImpacter.Action();
                tuple.SetStore(inputStoreIndex, null);
            }
        }

        public void Update(ITuple tuple)
        {
            UndoScoreImpacter undoScoreImpacter = tuple.GetStore<UndoScoreImpacter>(inputStoreIndex);
            // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
            if (undoScoreImpacter != null)
            {
                undoScoreImpacter.Action();
            }
            tuple.SetStore(inputStoreIndex, Impact(tuple));
        }
    }
}
