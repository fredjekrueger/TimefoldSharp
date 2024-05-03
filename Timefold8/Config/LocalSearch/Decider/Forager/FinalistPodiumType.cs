using TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager.Finalist;

namespace TimefoldSharp.Core.Config.LocalSearch.Decider.Forager
{
    public enum FinalistPodiumType
    {
        HIGHEST_SCORE,
        STRATEGIC_OSCILLATION,
        STRATEGIC_OSCILLATION_BY_LEVEL,
        STRATEGIC_OSCILLATION_BY_LEVEL_ON_BEST_SCORE
    }

    public static class FinalistPodiumTypeHelper
    {
        public static FinalistPodium BuildFinalistPodium(FinalistPodiumType podium)
        {
            switch (podium)
            {
                case FinalistPodiumType.HIGHEST_SCORE:
                    return new HighestScoreFinalistPodium();
                case FinalistPodiumType.STRATEGIC_OSCILLATION:
                case FinalistPodiumType.STRATEGIC_OSCILLATION_BY_LEVEL:
                    return new StrategicOscillationByLevelFinalistPodium(false);
                case FinalistPodiumType.STRATEGIC_OSCILLATION_BY_LEVEL_ON_BEST_SCORE:
                    return new StrategicOscillationByLevelFinalistPodium(true);
                default:
                    throw new Exception("The finalistPodiumType (" + podium + ") is not implemented.");
            }
        }
    }
}
