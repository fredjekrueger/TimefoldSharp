namespace TimefoldSharp.Core.API.Score.Stream
{
    public interface ConstraintProvider
    {
        List<Constraint> DefineConstraints(ConstraintFactory constraintFactory);
    }
}
