using TimefoldSharp.Core.Impl.ConstructionHeuristic.Event;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.decider.Forager
{
    public interface ConstructionHeuristicForager : ConstructionHeuristicPhaseLifecycleListener
    {
        bool IsQuitEarly();
        ConstructionHeuristicMoveScope PickMove(ConstructionHeuristicStepScope stepScope);
        void AddMove(ConstructionHeuristicMoveScope moveScope);
    }
}
