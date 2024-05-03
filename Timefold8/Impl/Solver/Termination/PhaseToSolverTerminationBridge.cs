using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class PhaseToSolverTerminationBridge : AbstractTermination
    {

        private readonly Termination solverTermination;

        public PhaseToSolverTerminationBridge(Termination solverTermination)
        {
            this.solverTermination = solverTermination;
        }

        public override double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope)
        {
            return solverTermination.CalculateSolverTimeGradient(phaseScope.GetSolverScope());
        }

        public override double CalculateSolverTimeGradient(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public override bool IsPhaseTerminated(AbstractPhaseScope phaseScope)
        {
            return solverTermination.IsSolverTerminated(phaseScope.GetSolverScope());
        }

        public override bool IsSolverTerminated(SolverScope solverScope)
        {
            throw new Exception("can only be used for phase termination.");
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {

        }
    }
}
