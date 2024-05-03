using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public interface Termination : PhaseLifecycleListener
    {
        bool IsSolverTerminated(SolverScope solverScope);
        double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope);
        bool IsPhaseTerminated(AbstractPhaseScope phaseScope);
        double CalculateSolverTimeGradient(SolverScope solverScope);
    }
}
