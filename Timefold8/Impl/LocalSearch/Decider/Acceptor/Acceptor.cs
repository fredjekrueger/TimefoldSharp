using TimefoldSharp.Core.Impl.LocalSearch.Event;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor
{
    public interface Acceptor : LocalSearchPhaseLifecycleListener
    {
        bool IsAccepted(LocalSearchMoveScope moveScope);
    }
}
