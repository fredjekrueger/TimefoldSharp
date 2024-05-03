using TimefoldSharp.Core.API.Score;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public abstract class ScoreContext
    {
        protected readonly AbstractScoreInliner parent;
        protected readonly AbstractConstraint constraint;
        protected readonly Score constraintWeight;
        protected readonly bool constraintMatchEnabled;

        protected ScoreContext(AbstractScoreInliner parent, AbstractConstraint constraint, Score constraintWeight)
        {
            this.parent = parent;
            this.constraint = constraint;
            this.constraintWeight = constraintWeight;
            this.constraintMatchEnabled = parent.IsConstraintMatchEnabled();
        }

        public AbstractConstraint GetConstraint()
        {
            return constraint;
        }

        protected UndoScoreImpacter ImpactWithConstraintMatch(UndoScoreImpacter undoScoreImpact, Score score, ConstraintMatchSupplier constraintMatchSupplier)
        {
            throw new NotImplementedException();
        }
    }
}
