using TimefoldSharp.Core.Impl.LocalSearch.Event;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager
{
    public interface LocalSearchForager : LocalSearchPhaseLifecycleListener
    {
        bool SupportsNeverEndingMoveSelector();
        bool IsQuitEarly();
        void AddMove(LocalSearchMoveScope moveScope);
        LocalSearchMoveScope PickMove(LocalSearchStepScope stepScope);
    }
}
