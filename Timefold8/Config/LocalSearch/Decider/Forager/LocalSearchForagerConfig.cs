using TimefoldSharp.Core.Config.Solver;

namespace TimefoldSharp.Core.Config.LocalSearch.Decider.Forager
{
    public class LocalSearchForagerConfig : AbstractConfig<LocalSearchForagerConfig>
    {
        protected int? acceptedCountLimit = null;
        protected LocalSearchPickEarlyType? pickEarlyType = null;
        protected FinalistPodiumType? finalistPodiumType = null;
        protected bool? breakTieRandomly = null;

        public LocalSearchForagerConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public LocalSearchForagerConfig Inherit(LocalSearchForagerConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public void SetAcceptedCountLimit(int? acceptedCountLimit)
        {
            this.acceptedCountLimit = acceptedCountLimit;
        }

        public void SetPickEarlyType(LocalSearchPickEarlyType? pickEarlyType)
        {
            this.pickEarlyType = pickEarlyType;
        }

        public LocalSearchPickEarlyType? GetPickEarlyType()
        {
            return pickEarlyType;
        }

        public int? GetAcceptedCountLimit()
        {
            return acceptedCountLimit;
        }

        public FinalistPodiumType? GetFinalistPodiumType()
        {
            return finalistPodiumType;
        }

        public bool? GetBreakTieRandomly()
        {
            return breakTieRandomly;
        }
    }
}
