using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;

namespace TimefoldSharp.Core.Impl.LocalSearch.Event
{
    public interface LocalSearchPhaseLifecycleListener : SolverLifecycleListener
    {
        void PhaseStarted(LocalSearchPhaseScope phaseScope);

        void StepStarted(LocalSearchStepScope stepScope);

        void StepEnded(LocalSearchStepScope stepScope);

        void PhaseEnded(LocalSearchPhaseScope phaseScope);
    }
}
