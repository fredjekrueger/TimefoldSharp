namespace TimefoldSharp.Core.API.Score.Stream
{
    public interface ConstraintStream
    {
        Constraint Penalize(string constraintName, Score constraintWeight);
    }
}
