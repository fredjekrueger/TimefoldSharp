using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu.Size
{
    public abstract class AbstractTabuSizeStrategy : TabuSizeStrategy
    {
        public abstract int DetermineTabuSize(LocalSearchStepScope stepScope);
    }
}
