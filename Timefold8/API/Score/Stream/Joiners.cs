using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.Impl.Score.Stream;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public sealed class Joiners
    {
        public static BiJoiner<A, A, Property_> Equal<A, Property_>(Func<A, Property_> mapping)
        {
            return Equal(mapping, mapping);
        }

        public static BiJoiner<A, B, Property_> Equal<A, B, Property_>(Func<A, Property_> leftMapping, Func<B, Property_> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.EQUAL, rightMapping);
        }

        public static BiJoiner<A, A, Property_> LessThan<A, Property_>(Func<A, Property_> mapping)
        {
            return LessThan(mapping, mapping);
        }

        public static BiJoiner<A, B, Property_> LessThan<A, B, Property_>(Func<A, Property_> leftMapping, Func<B, Property_> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.LESS_THAN, rightMapping);
        }

        public static BiJoiner<A, B, Property_> GreaterThan<A, B, Property_>(Func<A, Property_> leftMapping, Func<B, Property_> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.GREATER_THAN, rightMapping);
        }

        public static BiJoiner<A, A, Property_> Overlapping<A, Property_>(Func<A, Property_> startMapping, Func<A, Property_> endMapping)
        {
            return Overlapping(startMapping, endMapping, startMapping, endMapping);
        }

        public static BiJoiner<A, B, Property_> Overlapping<A, B, Property_>(Func<A, Property_> leftStartMapping, Func<A, Property_> leftEndMapping,
            Func<B, Property_> rightStartMapping, Func<B, Property_> rightEndMapping)
        {
            return Joiners.LessThan(leftStartMapping, rightEndMapping).And(Joiners.GreaterThan(leftEndMapping, rightStartMapping));
        }

        public static BiJoiner<A, B, Property_> LessThanOrEqual<A, B, Property_>(Func<A, Property_> leftMapping, Func<B, Property_> rightMapping)
        {
            return JoinerSupport.GetJoinerService().NewBiJoiner(leftMapping, JoinerType.LESS_THAN_OR_EQUAL, rightMapping);
        }
    }
}