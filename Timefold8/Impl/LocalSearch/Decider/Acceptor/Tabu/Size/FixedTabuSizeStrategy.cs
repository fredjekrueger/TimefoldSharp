using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu.Size
{
    public class FixedTabuSizeStrategy : AbstractTabuSizeStrategy
    {

        protected readonly int tabuSize;

        public FixedTabuSizeStrategy(int tabuSize)
        {
            this.tabuSize = tabuSize;
            if (tabuSize < 0)
            {
                throw new Exception("The tabuSize (" + tabuSize
                        + ") cannot be negative.");
            }
        }

        public override int DetermineTabuSize(LocalSearchStepScope stepScope)
        {
            throw new NotImplementedException();
        }
    }
}
