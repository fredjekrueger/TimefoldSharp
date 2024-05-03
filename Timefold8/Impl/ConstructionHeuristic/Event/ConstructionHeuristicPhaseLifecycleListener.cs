using TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Event
{
    public interface ConstructionHeuristicPhaseLifecycleListener : SolverLifecycleListener
    {
        void StepStarted(ConstructionHeuristicStepScope stepScope);
        void PhaseStarted(ConstructionHeuristicPhaseScope phaseScope);
        void StepEnded(ConstructionHeuristicStepScope stepScope);
        void PhaseEnded(ConstructionHeuristicPhaseScope phaseScope);
    }
}
