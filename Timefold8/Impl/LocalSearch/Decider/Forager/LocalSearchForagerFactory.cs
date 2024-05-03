using TimefoldSharp.Core.Config.LocalSearch.Decider.Forager;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager
{
    public class LocalSearchForagerFactory
    {
        public static LocalSearchForagerFactory Create(LocalSearchForagerConfig foragerConfig)
        {
            return new LocalSearchForagerFactory(foragerConfig);
        }

        private readonly LocalSearchForagerConfig foragerConfig;

        public LocalSearchForagerFactory(LocalSearchForagerConfig foragerConfig)
        {
            this.foragerConfig = foragerConfig;
        }

        public LocalSearchForager BuildForager()
        {
            LocalSearchPickEarlyType pickEarlyType_ = foragerConfig.GetPickEarlyType() ?? LocalSearchPickEarlyType.NEVER;
            int acceptedCountLimit_ = foragerConfig.GetAcceptedCountLimit() ?? int.MaxValue;
            FinalistPodiumType finalistPodiumType_ = foragerConfig.GetFinalistPodiumType() ?? FinalistPodiumType.HIGHEST_SCORE;
            // Breaking ties randomly leads to better results statistically
            bool breakTieRandomly_ = foragerConfig.GetBreakTieRandomly() ?? true;
            return new AcceptedLocalSearchForager(FinalistPodiumTypeHelper.BuildFinalistPodium(finalistPodiumType_), pickEarlyType_, acceptedCountLimit_, breakTieRandomly_);
        }
    }
}
