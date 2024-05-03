using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu.Size
{
    public interface TabuSizeStrategy
    {
        int DetermineTabuSize(LocalSearchStepScope stepScope);
    }
}
