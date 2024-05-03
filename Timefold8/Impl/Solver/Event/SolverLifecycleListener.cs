using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Event
{
    public interface SolverLifecycleListener : EventListenerTF
    {
        void SolvingStarted(SolverScope solverScope);

        void SolvingEnded(SolverScope solverScope);

        void SolvingError(SolverScope solverScope, Exception exception);
    }
}