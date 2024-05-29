using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;
using TimefoldSharp.Core.Impl.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public sealed class DefaultJoinerService : JoinerService
    {
        public BiJoiner<A, B> NewBiJoiner<A, B>(Func<A, object> leftMapping, JoinerType joinerType, Func<B, object> rightMapping)
        {
            return new DefaultBiJoiner<A, B>(leftMapping, joinerType, rightMapping);
        }
    }
}