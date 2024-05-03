namespace TimefoldSharp.Core.API.Score.Stream
{
    public interface Constraint
    {
        ConstraintFactory GetConstraintFactory();
        string GetConstraintName();
        string GetConstraintId();
    }
}
