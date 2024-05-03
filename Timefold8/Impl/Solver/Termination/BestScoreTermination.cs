using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class BestScoreTermination : AbstractTermination
    {

        private readonly int levelsSize;
        private readonly API.Score.Score bestScoreLimit;
        private readonly double[] timeGradientWeightNumbers;

        public BestScoreTermination(ScoreDefinition scoreDefinition, API.Score.Score bestScoreLimit, double[] timeGradientWeightNumbers)
        {
            levelsSize = scoreDefinition.GetLevelsSize();
            this.bestScoreLimit = bestScoreLimit;
            if (bestScoreLimit == null)
            {
                throw new Exception("The bestScoreLimit (" + bestScoreLimit + ") cannot be null.");
            }
            this.timeGradientWeightNumbers = timeGradientWeightNumbers;
            if (timeGradientWeightNumbers.Count() != levelsSize - 1)
            {
                throw new Exception(
                        "The timeGradientWeightNumbe.");
            }
        }

        public override double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override double CalculateSolverTimeGradient(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public override bool IsPhaseTerminated(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override bool IsSolverTerminated(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
