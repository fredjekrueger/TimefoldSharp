using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Buildin.HardSoftLong;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public class HardSoftLongScoreInliner : AbstractScoreInliner
    {
        public long HardScore { get; set; }
        public long SoftScore { get; set; }

        public HardSoftLongScoreInliner(Dictionary<Constraint, API.Score.Score> constraintWeightMap, bool constraintMatchEnabled)
            : base(constraintWeightMap, constraintMatchEnabled)
        {
        }

        public override IWeightedScoreImpacter BuildWeightedScoreImpacter(AbstractConstraint constraint)
        {
            var constraintWeight = (HardSoftLongScore)constraintWeightMap[constraint];
            HardSoftLongScoreContext context = new HardSoftLongScoreContext(this, constraint, constraintWeight);
            if (constraintWeight.SoftScore() == 0)
            {
                return WeightedScoreImpacterHelper.Of(context, new LongImpactFunction() { ImpactFunction = (i, constMatchSupplier) => context.ChangeHardScoreBy(i, constMatchSupplier) });
            }
            else if (constraintWeight.HardScore() == 0)
            {
                return WeightedScoreImpacterHelper.Of(context, new LongImpactFunction() { ImpactFunction = (i, constMatchSupplier) => context.ChangeSoftScoreBy(i, constMatchSupplier) });
            }
            else
            {
                return WeightedScoreImpacterHelper.Of(context, new LongImpactFunction() { ImpactFunction = (i, constMatchSupplier) => context.ChangeScoreBy(i, constMatchSupplier) });
            }
        }

        public override API.Score.Score ExtractScore(int initScore)
        {
            return HardSoftLongScore.OfUninitialized(initScore, HardScore, SoftScore);
        }
    }
}