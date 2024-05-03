using TimefoldSharp.Core.Config.ConstructHeuristic.Decider.Forager;
using TimefoldSharp.Core.Impl.Heurisitic;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.decider.Forager
{
    public class ConstructionHeuristicForagerFactory
    {
        private readonly ConstructionHeuristicForagerConfig foragerConfig;

        public static ConstructionHeuristicForagerFactory Create(ConstructionHeuristicForagerConfig foragerConfig)
        {
            return new ConstructionHeuristicForagerFactory(foragerConfig);
        }

        public ConstructionHeuristicForagerFactory(ConstructionHeuristicForagerConfig foragerConfig)
        {
            this.foragerConfig = foragerConfig;
        }

        public ConstructionHeuristicForager BuildForager(HeuristicConfigPolicy configPolicy)
        {
            ConstructionHeuristicPickEarlyType? pickEarlyType_;
            if (foragerConfig.GetPickEarlyType() == null)
            {
                pickEarlyType_ = configPolicy.BuilderInfo.GetInitializingScoreTrend().IsOnlyDown()
                        ? ConstructionHeuristicPickEarlyType.FIRST_NON_DETERIORATING_SCORE
                        : ConstructionHeuristicPickEarlyType.NEVER;
            }
            else
            {
                pickEarlyType_ = foragerConfig.GetPickEarlyType();
            }
            return new DefaultConstructionHeuristicForager(pickEarlyType_);
        }
    }
}
