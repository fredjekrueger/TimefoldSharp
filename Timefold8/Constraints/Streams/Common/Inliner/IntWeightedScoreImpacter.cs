using System.Numerics;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    internal class IntWeightedScoreImpacter : IWeightedScoreImpacter
    {
        private readonly IntImpactFunction impactFunction;
        private readonly ScoreContext context;

        public IntWeightedScoreImpacter(IntImpactFunction impactFunction, ScoreContext context)
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
            return impactFunction.ImpactFunction.Invoke(matchWeight, constraintMatchSupplier);
        }

        public UndoScoreImpacter ImpactScore(long matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            throw new NotImplementedException();
        }

        public UndoScoreImpacter ImpactScore(BigInteger matchWeight, ConstraintMatchSupplier constraintMatchSupplier)
        {
            throw new NotImplementedException();
        }
    }
}
