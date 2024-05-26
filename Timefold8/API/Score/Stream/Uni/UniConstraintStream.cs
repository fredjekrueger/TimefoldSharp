using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.API.Score.Stream.Uni
{
    public interface UniConstraintStream<A> : ConstraintStream
    {
        BiConstraintStream<A, B> Join<B, Property_>(Type otherClass, BiJoiner<A, B, Property_> joiner1, BiJoiner<A, B, Property_> joiner2);
        BiConstraintStream<A, B> Join<B, Property_>(Type otherClass, params BiJoiner<A, B, Property_>[] joiners);
        BiConstraintStream<A, B> Join<B, Property_>(UniConstraintStream<B> otherStream, params BiJoiner<A, B, Property_>[] joiners);

        UniConstraintStream<A> Filter(Func<A, bool> predicate);
        UniConstraintBuilder<A> Penalize(Score constraintWeight);
    }
}
