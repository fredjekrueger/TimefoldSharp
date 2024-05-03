using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public abstract class AbstractTermination : PhaseLifecycleListenerAdapter, Termination
    {
        public abstract double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope);

        public abstract double CalculateSolverTimeGradient(SolverScope solverScope);

        public abstract bool IsPhaseTerminated(AbstractPhaseScope phaseScope);

        public abstract bool IsSolverTerminated(SolverScope solverScope);


    }
}
