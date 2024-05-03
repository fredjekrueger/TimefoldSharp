using TimefoldSharp.Core.Impl.LocalSearch.Event;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager
{
    public abstract class LocalSearchPhaseLifecycleListenerAdapter : SolverLifecycleListenerAdapter, LocalSearchPhaseLifecycleListener
    {
        public virtual void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
        }

        public virtual void PhaseStarted(LocalSearchPhaseScope phaseScope)
        {
        }

        public virtual void StepEnded(LocalSearchStepScope stepScope)
        {
        }

        public virtual void StepStarted(LocalSearchStepScope stepScope)
        {
        }
    }
}
