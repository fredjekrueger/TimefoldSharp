using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Bi
{
    public sealed class FilteringBiJoiner<A, B> : BiJoiner<A, B>
    {
        private readonly Func<A, B, bool> filter;

        public FilteringBiJoiner(Func<A, B, bool> filter)
        {
            this.filter = filter;
        }

        public BiJoiner<A, B> And(BiJoiner<A, B> otherJoiner)
        {
            throw new NotImplementedException();
        }

        public Func<A, B, bool> GetFilter()
        {
            return filter;
        }
    }
}
