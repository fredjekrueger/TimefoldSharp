namespace TimefoldSharp.Core.Impl.Domain.Score.Definition
{
    public interface ScoreDefinition
    {
        int GetLevelsSize();
        API.Score.Score ParseScore(string scoreString);
        int GetFeasibleLevelsSize();
        bool IsNegativeOrZero(API.Score.Score score);
        API.Score.Score GetZeroScore();
        API.Score.Score GetOneSoftestScore();
    }
}
