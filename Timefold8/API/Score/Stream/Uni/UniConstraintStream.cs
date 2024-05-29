using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.API.Score.Stream.Uni
{
    public interface UniConstraintStream<A> : ConstraintStream
    {
        BiConstraintStream<A, B> Join<B>(Type otherClass, BiJoiner<A, B> joiner1, BiJoiner<A, B> joiner2);
        BiConstraintStream<A, B> Join<B>(Type otherClass, params BiJoiner<A, B>[] joiners);
        BiConstraintStream<A, B> Join<B>(UniConstraintStream<B> otherStream, params BiJoiner<A, B>[] joiners);

        UniConstraintStream<A> Filter(Func<A, bool> predicate);
        UniConstraintBuilder<A> Penalize(Score constraintWeight);
    }
}
