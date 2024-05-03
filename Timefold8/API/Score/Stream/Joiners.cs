using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.Impl.Score.Stream;

namespace TimefoldSharp.Core.API.Score.Stream
{
    /*public interface IJoinerProperty
    {

    }*/

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

        public static BiJoiner<A, B, Property_> LessThan<A, B, Property_>(
            Func<A, Property_> leftMapping, Func<B, Property_> rightMapping)
        {
            return JoinerSupport.GetJoinerService()
                    .NewBiJoiner(leftMapping, JoinerType.LESS_THAN, rightMapping);
        }
    }
}