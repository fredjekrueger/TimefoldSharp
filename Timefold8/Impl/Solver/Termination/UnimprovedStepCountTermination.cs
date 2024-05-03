using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class UnimprovedStepCountTermination : AbstractTermination
    {

        private readonly int unimprovedStepCountLimit;

        public UnimprovedStepCountTermination(int unimprovedStepCountLimit)
        {
            this.unimprovedStepCountLimit = unimprovedStepCountLimit;
            if (unimprovedStepCountLimit < 0)
            {
                throw new Exception("The unimprovedStepCountLimit (" + unimprovedStepCountLimit
                        + ") cannot be negative.");
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
