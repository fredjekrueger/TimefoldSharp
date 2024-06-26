using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public class LongWeightedScoreImpacter : IWeightedScoreImpacter
    {
        private readonly LongImpactFunction impactFunction;
        private readonly ScoreContext context;

        public LongWeightedScoreImpacter(LongImpactFunction impactFunction, ScoreContext context)
        {
            this.impactFunction = impactFunction;
            this.context = context;
        }

        public ScoreContext GetContext()
        {
            throw new NotImplementedException();
        }

        public UndoScoreImpacter ImpactScore(int matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            context.GetConstraint().AssertCorrectImpact(matchWeight);
            return impactFunction.ImpactFunction.Invoke( matchWeight, constraintMatchSupplier); // int can be cast to long
        }

        public UndoScoreImpacter ImpactScore(long matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            context.GetConstraint().AssertCorrectImpact(matchWeight);
            return impactFunction.ImpactFunction.Invoke( matchWeight, constraintMatchSupplier);
        }

        public UndoScoreImpacter ImpactScore(BigInteger matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            throw new NotImplementedException();
        }
    }
}
