using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;

namespace TimefoldSharp.Core.Impl.Phase
{
    public abstract class PhaseLifecycleListenerAdapter : SolverLifecycleListenerAdapter, PhaseLifecycleListener
    {
        public virtual void PhaseEnded(AbstractPhaseScope phaseScope)
        {
        }

        public virtual void PhaseStarted(AbstractPhaseScope phaseScope)
        {
        }

        public virtual void StepEnded(AbstractStepScope stepScope)
        {
        }

        public virtual void StepStarted(AbstractStepScope stepScope)
        {
        }
    }
}
