using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;
using TimefoldSharp.Core.Impl.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public sealed class DefaultJoinerService : JoinerService
    {
        public BiJoiner<A, B, Property_> NewBiJoiner<A, B, Property_>(Func<A, Property_> leftMapping, JoinerType joinerType, Func<B, Property_> rightMapping)
        {
            return new DefaultBiJoiner<A, B, Property_>(leftMapping, joinerType, rightMapping);
        }
    }
}