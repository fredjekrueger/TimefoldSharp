using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Score.Trend;

namespace TimefoldSharp.Core.Impl.Score.Director
{
    public abstract class InnerScoreDirectorFactory : ScoreDirectorFactory
    {

        public InnerScoreDirector BuildScoreDirector(bool lookUpEnabled, bool constraintMatchEnabledPreference)
        {
            return BuildScoreDirector(lookUpEnabled, constraintMatchEnabledPreference, true);
        }

        public abstract void AssertScoreFromScratch(ISolution solution);

        public abstract InnerScoreDirector BuildScoreDirector(bool lookUpEnabled, bool constraintMatchEnabledPreference, bool expectShadowVariablesInCorrectState);

        public ScoreDirector BuildScoreDirector()
        {
            throw new NotImplementedException();
        }

        public abstract InitializingScoreTrend GetInitializingScoreTrend();
    }
}
