using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Uni
{
    public interface InnerUniConstraintStream<A> : UniConstraintStream<A>
    {
        BiConstraintStream<A, B> Join<B>(UniConstraintStream<B> otherStream, BiJoinerComber<A, B> joinerComber);
    }
}
