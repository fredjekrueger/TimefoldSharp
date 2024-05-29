using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Bi
{
    public class DefaultBiJoiner<A, B> : AbstractJoiner<B>, BiJoiner<A, B>
    {
        private readonly Func<A, object>[] leftMappings;

        public Func<A, object> GetLeftMapping(int index)
        {
            return (Func<A, object>)leftMappings[index];
        }

        public DefaultBiJoiner(Func<A, object> leftMapping, JoinerType joinerType, Func<B, object> rightMapping) : base(rightMapping, joinerType)
        {
            this.leftMappings = new Func<A, object>[] { leftMapping };
        }

        public DefaultBiJoiner(Func<A, object>[] leftMappings, JoinerType[] joinerTypes, Func<B, object>[] rightMappings) : base(rightMappings, joinerTypes)
        {
            this.leftMappings = leftMappings;
        }



        public DefaultBiJoiner<A, B> And(BiJoiner<A, B> otherJoiner)
        {
            DefaultBiJoiner<A, B> castJoiner = (DefaultBiJoiner<A, B>)otherJoiner;
            int joinerCount = GetJoinerCount();
            int castJoinerCount = castJoiner.GetJoinerCount();
            int newJoinerCount = joinerCount + castJoinerCount;
            JoinerType[] newJoinerTypes = new JoinerType[newJoinerCount];
            Func<A, object>[] newLeftMappings = new Func<A, object>[newJoinerCount];
            Func<B, object>[] newRightMappings = new Func<B, object>[newJoinerCount];
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
            return new DefaultBiJoiner<A, B>(newLeftMappings, newJoinerTypes, newRightMappings);
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

        private static readonly DefaultBiJoiner<A, B> NONE = new DefaultBiJoiner<A, B>(new Func<A, object>[0], new JoinerType[0], new Func<B, object>[0]);

        public static DefaultBiJoiner<A, B> Merge(List<DefaultBiJoiner<A, B>> joinerList)
        {
            if (joinerList.Count() == 1)
            {
                return joinerList[0];
            }
            //return joinerList.stream().reduce(NONE, DefaultBiJoiner::and);
            var result = joinerList.Aggregate(NONE, (current, joiner) => current.And(joiner));
            return result;
        }

        BiJoiner<A, B> BiJoiner<A, B>.And(BiJoiner<A, B> otherJoiner)
        {
            return And(otherJoiner);
        }
    }
}
