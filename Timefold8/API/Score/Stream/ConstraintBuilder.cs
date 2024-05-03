namespace TimefoldSharp.Core.API.Score.Stream
{
    public interface ConstraintBuilder
    {
        Constraint AsConstraint(string constraintName);
        Constraint AsConstraint(string constraintPackage, string constraintName);

    }
}
