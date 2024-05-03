using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;

namespace TimefoldSharp.Core.Impl.Phase.Event
{
    public interface PhaseLifecycleListener : SolverLifecycleListener
    {
        void PhaseStarted(AbstractPhaseScope phaseScope);

        void StepStarted(AbstractStepScope stepScope);

        void StepEnded(AbstractStepScope stepScope);

        void PhaseEnded(AbstractPhaseScope phaseScope);
    }
}
