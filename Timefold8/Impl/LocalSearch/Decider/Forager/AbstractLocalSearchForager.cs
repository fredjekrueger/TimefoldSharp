using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager
{
    public abstract class AbstractLocalSearchForager : LocalSearchPhaseLifecycleListenerAdapter, LocalSearchForager
    {
        public abstract void AddMove(LocalSearchMoveScope moveScope);

        public abstract bool IsQuitEarly();

        public abstract LocalSearchMoveScope PickMove(LocalSearchStepScope stepScope);

        public abstract bool SupportsNeverEndingMoveSelector();
    }
}
