using TimefoldSharp.Core.API.Score.Buildin.HardSoft;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public class HardSoftScoreContext : ScoreContext
    {
        public HardSoftScoreContext(AbstractScoreInliner parent, AbstractConstraint constraint, API.Score.Score constraintWeight)
            : base(parent, constraint, constraintWeight)
        {

        }

        HardSoftScore hardSoftScore => (HardSoftScore)constraintWeight;
        HardSoftScoreInliner parentHardSoftScoreInliner => (HardSoftScoreInliner)parent;

        public UndoScoreImpacter ChangeHardScoreBy(int matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            int hardImpact = hardSoftScore.HardScore() * matchWeight;
            parentHardSoftScoreInliner.HardScore += hardImpact;
            UndoScoreImpacter undoScoreImpact = new UndoScoreImpacter() { Action = () => parentHardSoftScoreInliner.HardScore -= hardImpact };
            if (!constraintMatchEnabled)
            {
                return undoScoreImpact;
            }
            return ImpactWithConstraintMatch(undoScoreImpact, HardSoftScore.OfHard(hardImpact), constraintMatchSupplier);
        }

        public UndoScoreImpacter ChangeSoftScoreBy(int matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            int softImpact = hardSoftScore.SoftScore() * matchWeight;
            parentHardSoftScoreInliner.SoftScore += softImpact;
            UndoScoreImpacter undoScoreImpact = new UndoScoreImpacter() { Action = () => parentHardSoftScoreInliner.SoftScore -= softImpact };
            if (!constraintMatchEnabled)
            {
                return undoScoreImpact;
            }
            return ImpactWithConstraintMatch(undoScoreImpact, HardSoftScore.OfSoft(softImpact), constraintMatchSupplier);
        }

        public UndoScoreImpacter ChangeScoreBy(int matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            int hardImpact = hardSoftScore.HardScore() * matchWeight;
            int softImpact = hardSoftScore.SoftScore() * matchWeight;
            parentHardSoftScoreInliner.HardScore += hardImpact;
            parentHardSoftScoreInliner.SoftScore += softImpact;
            UndoScoreImpacter undoScoreImpact = new UndoScoreImpacter()
            {
                Action = () =>
                {
                    parentHardSoftScoreInliner.HardScore -= hardImpact;
                    parentHardSoftScoreInliner.SoftScore -= softImpact;
                }
            };
            if (!constraintMatchEnabled)
            {
                return undoScoreImpact;
            }
            return ImpactWithConstraintMatch(undoScoreImpact, HardSoftScore.Of(hardImpact, softImpact), constraintMatchSupplier);
        }
    }
}
