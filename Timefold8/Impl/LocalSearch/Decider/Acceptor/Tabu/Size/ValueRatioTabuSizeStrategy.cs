using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu.Size
{
    public class ValueRatioTabuSizeStrategy : AbstractTabuSizeStrategy
    {

        protected readonly double tabuRatio;

        public ValueRatioTabuSizeStrategy(double tabuRatio)
        {
            this.tabuRatio = tabuRatio;
            if (tabuRatio <= 0.0 || tabuRatio >= 1.0)
            {
                throw new Exception("The tabuRatio (" + tabuRatio
                        + ") must be between 0.0 and 1.0.");
            }
        }

        public override int DetermineTabuSize(LocalSearchStepScope stepScope)
        {
            throw new NotImplementedException();
        }
    }
}
