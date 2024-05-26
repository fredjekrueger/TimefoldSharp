namespace TimefoldSharp.Core.API.Score.Stream.Bi
{
    public interface BiConstraintStream<A, B> : ConstraintStream
    {
        BiConstraintBuilder<A, B> Penalize(Score constraintWeight, Func<A, B, int> matchWeigher);
        BiConstraintBuilder<A, B> Penalize(Score constraintWeight);
        BiConstraintStream<A, B> Filter(Func<A, B, bool> predicate);
        BiConstraintBuilder<A, B> Reward(Score constraintWeight);
        BiConstraintBuilder<A, B> Reward(Score constraintWeight, Func<A, B, int> matchWeigher);
    }
}