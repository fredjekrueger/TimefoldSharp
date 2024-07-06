using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public class ComparisonIndexer<T> : Indexer<T>
    {
        private readonly int indexKeyPosition;
        private readonly Func<Indexer<T>> downstreamIndexerSupplier;
        private readonly KeyComparator keyComparator;
        private readonly bool hasOrEquals;
        private readonly SortedDictionary<object, Indexer<T>> comparisonMap;

        public ComparisonIndexer(JoinerType comparisonJoinerType, Func<Indexer<T>> downstreamIndexerSupplier)
            : this(comparisonJoinerType, 0, downstreamIndexerSupplier)
        {

        }

        public ComparisonIndexer(JoinerType comparisonJoinerType, int indexKeyPosition, Func<Indexer<T>> downstreamIndexerSupplier)
        {
            this.indexKeyPosition = indexKeyPosition;
            this.downstreamIndexerSupplier = downstreamIndexerSupplier;
            /*
             * For GT/GTE, the iteration order is reversed.
             * This allows us to iterate over the entire map, stopping when the threshold is reached.
             * This is done so that we can avoid using head/tail sub maps, which are expensive.
             */
            this.keyComparator =
                    (comparisonJoinerType == JoinerType.GREATER_THAN || comparisonJoinerType == JoinerType.GREATER_THAN_OR_EQUAL)
                            ? new KeyComparator() { reversed = true } : new KeyComparator();
            this.hasOrEquals = comparisonJoinerType == JoinerType.GREATER_THAN_OR_EQUAL
                    || comparisonJoinerType == JoinerType.LESS_THAN_OR_EQUAL;
            this.comparisonMap = new SortedDictionary<object, Indexer<T>>(keyComparator);
        }

        public void ForEach(IndexProperties indexProperties, Action<T> tupleConsumer)
        {
            int size = comparisonMap.Count;
            if (size == 0)
            {
                return;
            }
            var indexKey = indexProperties.ToKey(indexKeyPosition);
            if (size == 1)
            { // Avoid creation of the entry set and iterator.
                var entry = comparisonMap.FirstOrDefault();
                VisitEntry(indexProperties, tupleConsumer, indexKey, entry);
            }
            else
            {
                foreach (var entry in comparisonMap)
                {
                    bool boundaryReached = VisitEntry(indexProperties, tupleConsumer, indexKey, entry);
                    if (boundaryReached)
                    {
                        return;
                    }
                }
            }
        }

        private bool VisitEntry(IndexProperties indexProperties, Action<T> tupleConsumer, object indexKey, KeyValuePair<object, Indexer<T>> entry)
        {
            // Comparator matches the order of iteration of the map, so the boundary is always found from the bottom up.
            int comparison = keyComparator.Compare(entry.Key, indexKey);
            if (comparison >= 0)
            { // Possibility of reaching the boundary condition.
                if (comparison > 0 || !hasOrEquals)
                {
                    // Boundary condition reached when we're out of bounds entirely, or when GTE/LTE is not allowed.
                    return true;
                }
            }
            // Boundary condition not yet reached; include the indexer in the range.
            entry.Value.ForEach(indexProperties, tupleConsumer);
            return false;
        }

        public bool IsEmpty()
        {
            return comparisonMap.Count == 0;
        }

        public ElementAwareListEntry<T> Put(IndexProperties indexProperties, T tuple)
        {
            var indexKey = indexProperties.ToKey(indexKeyPosition);
            // Avoids computeIfAbsent in order to not create lambdas on the hot path.
            comparisonMap.TryGetValue(indexKey, out Indexer<T> downstreamIndexer);
            if (downstreamIndexer == null)
            {
                downstreamIndexer = downstreamIndexerSupplier.Invoke();
                comparisonMap.Add(indexKey, downstreamIndexer);
            }
            return downstreamIndexer.Put(indexProperties, tuple);
        }

        public void Remove(IndexProperties indexProperties, ElementAwareListEntry<T> entry)
        {
            var indexKey = indexProperties.ToKey(indexKeyPosition);
            var downstreamIndexer = GetDownstreamIndexer(indexProperties, indexKey, entry);
            downstreamIndexer.Remove(indexProperties, entry);
            if (downstreamIndexer.IsEmpty())
            {
                comparisonMap.Remove(indexKey);
            }
        }

        private Indexer<T> GetDownstreamIndexer(IndexProperties indexProperties, object indexerKey, ElementAwareListEntry<T> entry)
        {
            return comparisonMap[indexerKey];
        }
    }

    public class KeyComparator : IComparer<object>
    {
        public static KeyComparator INSTANCE = new KeyComparator();
        public bool reversed = false;

        public int Compare(object o1, object o2)
        {
            if (o1 == o2)
            {
                return 0;
            }
            if (reversed)
                return ((IComparable)o2).CompareTo((IComparable)o1);
            else
                return ((IComparable)o1).CompareTo((IComparable)o2);
        }
    }
}
