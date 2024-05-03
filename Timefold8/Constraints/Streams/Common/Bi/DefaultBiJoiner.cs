using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Bi
{
    public class DefaultBiJoiner<A, B, Property_> : AbstractJoiner<B, Property_>, BiJoiner<A, B, Property_>
    {
        private readonly Func<A, Property_>[] leftMappings;

        public Func<A, Property_> GetLeftMapping(int index)
        {
            return (Func<A, Property_>)leftMappings[index];
        }

        public DefaultBiJoiner(Func<A, Property_> leftMapping, JoinerType joinerType, Func<B, Property_> rightMapping) : base(rightMapping, joinerType)
        {
            this.leftMappings = new Func<A, Property_>[] { leftMapping };
        }

        public DefaultBiJoiner(Func<A, Property_>[] leftMappings, JoinerType[] joinerTypes, Func<B, Property_>[] rightMappings) : base(rightMappings, joinerTypes)
        {
            this.leftMappings = leftMappings;
        }



        public DefaultBiJoiner<A, B, Property_> And(BiJoiner<A, B, Property_> otherJoiner)
        {
            DefaultBiJoiner<A, B, Property_> castJoiner = (DefaultBiJoiner<A, B, Property_>)otherJoiner;
            int joinerCount = GetJoinerCount();
            int castJoinerCount = castJoiner.GetJoinerCount();
            int newJoinerCount = joinerCount + castJoinerCount;
            JoinerType[] newJoinerTypes = new JoinerType[newJoinerCount];
            Func<A, Property_>[] newLeftMappings = new Func<A, Property_>[newJoinerCount];
            Func<B, Property_>[] newRightMappings = new Func<B, Property_>[newJoinerCount];
            newJoinerTypes = ExtendArray(this.joinerTypes, newJoinerCount);
            newLeftMappings = ExtendArray(this.leftMappings, newJoinerCount);
            newRightMappings = ExtendArray(this.rightMappings, newJoinerCount);

            /*Array.Copy(this.joinerTypes, newJoinerTypes, newJoinerCount);
            Array.Copy(leftMappings , newLeftMappings, newJoinerCount);
            Array.Copy(rightMappings, newRightMappings, newJoinerCount);*/

            for (int i = 0; i < castJoinerCount; i++)
            {
                int newJoinerIndex = i + joinerCount;
                newJoinerTypes[newJoinerIndex] = castJoiner.GetJoinerType(i);
                newLeftMappings[newJoinerIndex] = castJoiner.GetLeftMapping(i);
                newRightMappings[newJoinerIndex] = castJoiner.GetRightMapping(i);
            }
            return new DefaultBiJoiner<A, B, Property_>(newLeftMappings, newJoinerTypes, newRightMappings);
        }

        T[] ExtendArray<T>(T[] source, int length)
        {
            T[] newArray = new T[length];
            Array.Copy(source, newArray, source.Length);
            for (int i = source.Length; i < length; i++)
            {
            }
            return newArray;
        }

        private static readonly DefaultBiJoiner<A, B, Property_> NONE = new DefaultBiJoiner<A, B, Property_>(new Func<A, Property_>[0], new JoinerType[0], new Func<B, Property_>[0]);

        public static DefaultBiJoiner<A, B, Property_> Merge(List<DefaultBiJoiner<A, B, Property_>> joinerList)
        {
            if (joinerList.Count() == 1)
            {
                return joinerList[0];
            }
            //return joinerList.stream().reduce(NONE, DefaultBiJoiner::and);
            var result = joinerList.Aggregate(NONE, (current, joiner) => current.And(joiner));
            return result;
        }

        BiJoiner<A, B, Property_> BiJoiner<A, B, Property_>.And(BiJoiner<A, B, Property_> otherJoiner)
        {
            return And(otherJoiner);
        }
    }
}
