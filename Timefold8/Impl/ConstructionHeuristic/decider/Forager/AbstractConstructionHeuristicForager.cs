using TimefoldSharp.Core.Impl.ConstructionHeuristic.Event;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.decider.Forager
{
    public abstract class AbstractConstructionHeuristicForager : ConstructionHeuristicPhaseLifecycleListenerAdapter, ConstructionHeuristicForager
    {
        public abstract void AddMove(ConstructionHeuristicMoveScope moveScope);

        public abstract bool IsQuitEarly();

        public abstract ConstructionHeuristicMoveScope PickMove(ConstructionHeuristicStepScope stepScope);
    }
}
