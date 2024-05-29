using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.Impl.Score.Stream;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public sealed class Joiners
    {
        public static BiJoiner<A, A> Equal<A>(Func<A, object> mapping)
        {
            return Equal(mapping, mapping);
        }

        public static BiJoiner<A, B> Equal<A, B>(Func<A, object> leftMapping, Func<B, object> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.EQUAL, rightMapping);
        }

        public static BiJoiner<A, A> LessThan<A>(Func<A, object> mapping)
        {
            return LessThan(mapping, mapping);
        }

        public static BiJoiner<A, B> LessThan<A, B>(Func<A, object> leftMapping, Func<B, object> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.LESS_THAN, rightMapping);
        }

        public static BiJoiner<A, B> GreaterThan<A, B>(Func<A, object> leftMapping, Func<B, object> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.GREATER_THAN, rightMapping);
        }

        public static BiJoiner<A, A> Overlapping<A>(Func<A, object> startMapping, Func<A, object> endMapping)
        {
            return Overlapping(startMapping, endMapping, startMapping, endMapping);
        }

        public static BiJoiner<A, B> Overlapping<A, B>(Func<A, object> leftStartMapping, Func<A, object> leftEndMapping,
            Func<B, object> rightStartMapping, Func<B, object> rightEndMapping)
        {
            return Joiners.LessThan(leftStartMapping, rightEndMapping).And(Joiners.GreaterThan(leftEndMapping, rightStartMapping));
        }

        public static BiJoiner<A, B> LessThanOrEqual<A, B>(Func<A, object> leftMapping, Func<B, object> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.LESS_THAN_OR_EQUAL, rightMapping);
        }
    }
}