using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class ScoreCalculationCountTermination : AbstractTermination
    {

        private readonly long scoreCalculationCountLimit;

        public ScoreCalculationCountTermination(long scoreCalculationCountLimit)
        {
            this.scoreCalculationCountLimit = scoreCalculationCountLimit;
            if (scoreCalculationCountLimit < 0L)
            {
                throw new Exception("The scoreCalculationCountLimit (" + scoreCalculationCountLimit + ") cannot be negative.");
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
