using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Uni
{
    public interface InnerUniConstraintStream<A> : UniConstraintStream<A>
    {
        BiConstraintStream<A, B> Join<B, Property_>(UniConstraintStream<B> otherStream, BiJoinerComber<A, B, Property_> joinerComber);
    }
}
