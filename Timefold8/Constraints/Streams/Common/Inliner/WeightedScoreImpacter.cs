using System.Numerics;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public interface IWeightedScoreImpacter
    {
        UndoScoreImpacter ImpactScore(int matchWeight, ConstraintMatchSupplier constraintMatchSupplier);
        UndoScoreImpacter ImpactScore(long matchWeight, ConstraintMatchSupplier constraintMatchSupplier);
        UndoScoreImpacter ImpactScore(BigInteger matchWeight, ConstraintMatchSupplier constraintMatchSupplier);

        ScoreContext GetContext();
    }

    public class IntImpactFunction
    {
        public Func<int, ConstraintMatchSupplier, UndoScoreImpacter> ImpactFunction;
    }

    public class LongImpactFunction
    {
        public Func<long, ConstraintMatchSupplier, UndoScoreImpacter> ImpactFunction;
    }

    public static class WeightedScoreImpacterHelper
    {
        public static IWeightedScoreImpacter Of(ScoreContext context, IntImpactFunction impactFunction)
        {
            return new IntWeightedScoreImpacter(impactFunction, context);
        }

        public static IWeightedScoreImpacter Of(ScoreContext context, LongImpactFunction impactFunction)
        {
            return new LongWeightedScoreImpacter(impactFunction, context);
        }
    }
}
