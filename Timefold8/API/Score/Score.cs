namespace TimefoldSharp.Core.API.Score
{
    public interface Score : IComparable<Score>
    {
        bool IsSolutionInitialized();

        Score Zero();
        Score Negate();
        Score Subtract(Score subtrahend);
        Score WithInitScore(int newInitScore);
        bool IsFeasible();
    }
}
