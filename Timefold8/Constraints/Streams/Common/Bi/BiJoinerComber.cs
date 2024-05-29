using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Bi
{
    public sealed class BiJoinerComber<A, B>
    {
        private DefaultBiJoiner<A, B> mergedJoiner;

        public static BiJoinerComber<A, B> Comb(BiJoiner<A, B>[] joiners)
        {
            List<DefaultBiJoiner<A, B>> defaultJoinerList = new List<DefaultBiJoiner<A, B>>(joiners.Length);
            List<Func<A, B, bool>> filteringList = new List<Func<A, B, bool>>(joiners.Length);

            int indexOfFirstFilter = -1;
            // Make sure all indexing joiners, if any, come before filtering joiners. This is necessary for performance.
            for (int i = 0; i < joiners.Length; i++)
            {
                BiJoiner<A, B> joiner = joiners[i];
                if (joiner is FilteringBiJoiner<A, B>)
                {
                    // From now on, only allow filtering joiners.
                    indexOfFirstFilter = i;
                    filteringList.Add(((FilteringBiJoiner<A, B>)joiner).GetFilter());
                }
                //else if (joiner.GetType().GetGenericTypeDefinition() == typeof(DefaultBiJoiner<,,>))
                else if (joiner is DefaultBiJoiner<A, B>)
                {
                    if (indexOfFirstFilter >= 0)
                    {
                        throw new Exception("Indexing joiner (" + joiner + ") must not follow " +
                                "a filtering joiner (" + joiners[indexOfFirstFilter] + ").\n" +
                                "Maybe reorder the joiners such that filtering() joiners are later in the parameter list.");
                    }
                    defaultJoinerList.Add((DefaultBiJoiner<A, B>)joiner);
                }
                else
                {
                    throw new Exception("The joiner class (" + joiner.GetType() + ") is not supported.");
                }
            }
            DefaultBiJoiner<A, B> mergedJoiner = DefaultBiJoiner<A, B>.Merge(defaultJoinerList);
            Func<A, B, bool> mergedFiltering = MergeFiltering(filteringList);
            return new BiJoinerComber<A, B>(mergedJoiner, mergedFiltering);
        }

        private static Func<A, B, bool> MergeFiltering(List<Func<A, B, bool>> filteringList)
        {
            if (filteringList == null || filteringList.Count == 0)
            {
                return null;
            }
            switch (filteringList.Count)
            {
                case 1:
                    return filteringList[0];
                case 2:
                    return (A a, B b) => filteringList[0](a, b) && filteringList[1](a, b);
                default:
                    // Avoid predicate.and() when more than 2 predicates for debugging and potentially performance
                    return (A a, B b) =>
                    {
                        foreach (var predicate in filteringList)
                        {
                            if (!predicate(a, b))
                            {
                                return false;
                            }
                        }
                        return true;
                    };
            }
        }

        private readonly Func<A, B, bool> mergedFiltering;

        public BiJoinerComber(DefaultBiJoiner<A, B> mergedJoiner, Func<A, B, bool> mergedFiltering)
        {
            this.mergedJoiner = mergedJoiner;
            this.mergedFiltering = mergedFiltering;
        }

        public void AddJoiner(DefaultBiJoiner<A, B> extraJoiner)
        {
            mergedJoiner = mergedJoiner.And(extraJoiner);
        }

        public DefaultBiJoiner<A, B> GetMergedJoiner()
        {
            return mergedJoiner;
        }

        public Func<A, B, bool> GetMergedFiltering()
        {
            return mergedFiltering;
        }
    }
}
