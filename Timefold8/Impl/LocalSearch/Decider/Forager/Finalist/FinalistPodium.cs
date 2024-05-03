using TimefoldSharp.Core.Impl.LocalSearch.Event;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager.Finalist
{
    public interface FinalistPodium : LocalSearchPhaseLifecycleListener
    {
        void AddMove(LocalSearchMoveScope moveScope);
        List<LocalSearchMoveScope> GetFinalistList();
    }
}
