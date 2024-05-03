using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Event
{
    public abstract class SolverLifecycleListenerAdapter : SolverLifecycleListener
    {
        public virtual void SolvingStarted(SolverScope solverScope)
        {
            // Hook method
        }

        public virtual void SolvingEnded(SolverScope solverScope)
        {
            // Hook method
        }

        public virtual void SolvingError(SolverScope solverScope, Exception exception) { }
    }
}
