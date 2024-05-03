namespace TimefoldSharp.Core.Impl.Score.Trend
{
    public class InitializingScoreTrend
    {
        private readonly InitializingScoreTrendLevel[] trendLevels;

        public InitializingScoreTrend(InitializingScoreTrendLevel[] trendLevels)
        {
            this.trendLevels = trendLevels;
        }

        public static InitializingScoreTrend ParseTrend(string initializingScoreTrendString, int levelsSize)
        {
            String[] trendTokens = initializingScoreTrendString.Split('/');
            bool tokenIsSingle = trendTokens.Count() == 1;
            if (!tokenIsSingle && trendTokens.Count() != levelsSize)
            {
                throw new Exception("The initializingScoreTrendString (" + initializingScoreTrendString
                        + ") doesn't follow the correct pattern (" + "):"
                        + " the trendTokens length (" + trendTokens.Count()
                        + ") differs from the levelsSize (" + levelsSize + ").");
            }
            InitializingScoreTrendLevel[] trendLevels = new InitializingScoreTrendLevel[levelsSize];
            for (int i = 0; i < levelsSize; i++)
            {
                trendLevels[i] = (InitializingScoreTrendLevel)Enum.Parse(typeof(InitializingScoreTrendLevel), trendTokens[tokenIsSingle ? 0 : i]);
            }
            return new InitializingScoreTrend(trendLevels);
        }

        public bool IsOnlyDown()
        {
            foreach (var trendLevel in trendLevels)
            {
                if (trendLevel != InitializingScoreTrendLevel.ONLY_DOWN)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public enum InitializingScoreTrendLevel
    {
        ANY, ONLY_UP, ONLY_DOWN
    }
}
