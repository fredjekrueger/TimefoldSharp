using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Buildin.HardSoftLong;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public class HardSoftLongScoreContext : ScoreContext
    {
        public HardSoftLongScoreContext(AbstractScoreInliner parent, AbstractConstraint constraint, Score constraintWeight) : base(parent, constraint, constraintWeight)
        {}

        HardSoftLongScore hardSoftScore => (HardSoftLongScore)constraintWeight;
        HardSoftLongScoreInliner parentHardSoftScoreInliner => (HardSoftLongScoreInliner)parent;

        public UndoScoreImpacter ChangeSoftScoreBy(long matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            long softImpact = hardSoftScore.SoftScore() * matchWeight;
            parentHardSoftScoreInliner.SoftScore += softImpact;
            UndoScoreImpacter undoScoreImpact = new UndoScoreImpacter() { Action = () => parentHardSoftScoreInliner.SoftScore -= softImpact};
            if (!constraintMatchEnabled)
            {
                return undoScoreImpact;
            }
            return ImpactWithConstraintMatch(undoScoreImpact, HardSoftLongScore.OfSoft(softImpact), constraintMatchSupplier);
        }

        public UndoScoreImpacter ChangeHardScoreBy(long matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            long hardImpact = hardSoftScore.HardScore() * matchWeight;
            parentHardSoftScoreInliner.HardScore += hardImpact;
            UndoScoreImpacter undoScoreImpact = new UndoScoreImpacter() { Action = () => parentHardSoftScoreInliner.HardScore -= hardImpact };
            if (!constraintMatchEnabled)
            {
                return undoScoreImpact;
            }
            return ImpactWithConstraintMatch(undoScoreImpact, HardSoftLongScore.OfHard(hardImpact), constraintMatchSupplier);
        }

        public UndoScoreImpacter ChangeScoreBy(long matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            long hardImpact = hardSoftScore.HardScore() * matchWeight;
            long softImpact = hardSoftScore.SoftScore() * matchWeight;
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
            return ImpactWithConstraintMatch(undoScoreImpact, HardSoftLongScore.Of(hardImpact, softImpact),
                    constraintMatchSupplier);
        }
    }
}
