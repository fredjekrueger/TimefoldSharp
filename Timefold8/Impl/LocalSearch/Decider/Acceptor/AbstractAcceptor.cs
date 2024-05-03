using TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor
{
    public abstract class AbstractAcceptor : LocalSearchPhaseLifecycleListenerAdapter, Acceptor
    {
        public abstract bool IsAccepted(LocalSearchMoveScope moveScope);
    }
}
