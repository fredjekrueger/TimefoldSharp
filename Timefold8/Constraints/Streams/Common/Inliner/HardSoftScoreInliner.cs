using TimefoldSharp.Core.API.Score.Buildin.HardSoft;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public class HardSoftScoreInliner : AbstractScoreInliner
    {

        public int HardScore { get; set; }
        public int SoftScore { get; set; }

        public HardSoftScoreInliner(Dictionary<Constraint, API.Score.Score> constraintWeightMap, bool constraintMatchEnabled)
            : base(constraintWeightMap, constraintMatchEnabled)
        {
        }

        public override IWeightedScoreImpacter BuildWeightedScoreImpacter(AbstractConstraint constraint)
        {
            var constraintWeight = (HardSoftScore)constraintWeightMap[constraint];
            HardSoftScoreContext context = new HardSoftScoreContext(this, constraint, constraintWeight);
            if (constraintWeight.SoftScore() == 0)
            {
                return WeightedScoreImpacterHelper.Of(context, new IntImpactFunction() { ImpactFunction = (i, constMatchSupplier) => context.ChangeHardScoreBy(i, constMatchSupplier) });
            }
            else if (constraintWeight.HardScore() == 0)
            {
                return WeightedScoreImpacterHelper.Of(context, new IntImpactFunction() { ImpactFunction = (i, constMatchSupplier) => context.ChangeSoftScoreBy(i, constMatchSupplier) });
            }
            else
            {
                return WeightedScoreImpacterHelper.Of(context, new IntImpactFunction() { ImpactFunction = (i, constMatchSupplier) => context.ChangeScoreBy(i, constMatchSupplier) });
            }
        }

        public override API.Score.Score ExtractScore(int initScore)
        {
            return HardSoftScore.OfUninitialized(initScore, HardScore, SoftScore);
        }
    }
}
