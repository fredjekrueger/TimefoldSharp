using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.Impl.Score.Stream
{
    public interface JoinerService
    {
        BiJoiner<A, B> NewBiJoiner<A, B>(Func<A, object> leftMapping, JoinerType joinerType, Func<B, object> rightMapping);
    }
}
