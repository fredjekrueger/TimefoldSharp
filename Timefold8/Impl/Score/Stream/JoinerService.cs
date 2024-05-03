using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.Impl.Score.Stream
{
    public interface JoinerService
    {
        BiJoiner<A, B, Property_> NewBiJoiner<A, B, Property_>(Func<A, Property_> leftMapping, JoinerType joinerType, Func<B, Property_> rightMapping);
    }
}
