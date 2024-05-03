using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class BestScoreFeasibleTermination : AbstractTermination

    {
        private readonly int feasibleLevelsSize;
        private readonly double[] timeGradientWeightFeasibleNumbers;

        public BestScoreFeasibleTermination(ScoreDefinition scoreDefinition, double[] timeGradientWeightFeasibleNumbers)
        {
            feasibleLevelsSize = scoreDefinition.GetFeasibleLevelsSize();
            this.timeGradientWeightFeasibleNumbers = timeGradientWeightFeasibleNumbers;
            if (timeGradientWeightFeasibleNumbers.Count() != feasibleLevelsSize - 1)
            {
                throw new Exception("The timeGradientWeightNumbers ");
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
