using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Phase
{
    public interface Phase : PhaseLifecycleListener
    {
        void Solve(SolverScope solverScope);
    }
}
