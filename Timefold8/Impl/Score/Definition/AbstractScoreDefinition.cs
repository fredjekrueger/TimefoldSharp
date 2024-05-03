using TimefoldSharp.Core.Impl.Domain.Score.Definition;

namespace TimefoldSharp.Core.Impl.Score.Definition
{
    public abstract class AbstractScoreDefinition : ScoreDefinition
    {
        private readonly String[] levelLabels;

        public AbstractScoreDefinition(String[] levelLabels)
        {
            this.levelLabels = levelLabels;
        }

        public int GetFeasibleLevelsSize()
        {
            throw new NotImplementedException();
        }

        public int GetLevelsSize()
        {
            return levelLabels.Length;
        }

        public abstract API.Score.Score GetOneSoftestScore();

        public abstract API.Score.Score GetZeroScore();

        public abstract bool IsNegativeOrZero(API.Score.Score score);

        public abstract API.Score.Score ParseScore(string scoreString);
    }
}
