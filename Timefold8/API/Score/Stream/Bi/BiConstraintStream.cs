namespace TimefoldSharp.Core.API.Score.Stream.Bi
{
    /*public abstract class BiConstraintStream<A, B, Score_> : ConstraintStream where Score_ : Score<Score_>
    {
        public BiConstraintBuilder<A, B, Score_> Penalize(Score_ constraintWeight)
        {
            throw new NotImplementedException();
        }

        public Constraint Penalize<T>(string constraintName, Score<T> constraintWeight)
        {
            throw new NotImplementedException();
        }
    }*/


    public interface BiConstraintStream<A, B> : ConstraintStream
    {
        BiConstraintBuilder<A, B> Penalize(Score constraintWeight, Func<A, B, int> matchWeigher);
        BiConstraintBuilder<A, B> Penalize(Score constraintWeight);
        BiConstraintStream<A, B> Filter(Func<A, B, bool> predicate);
        BiConstraintBuilder<A, B> Reward(Score constraintWeight);
        BiConstraintBuilder<A, B> Reward(Score constraintWeight, Func<A, B, int> matchWeigher);
    }
}