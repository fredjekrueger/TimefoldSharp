using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public class EqualsIndexer<T> : Indexer<T>
    {

        private readonly int indexKeyFrom;
        private readonly int indexKeyTo;
        private readonly Func<Indexer<T>> downstreamIndexerSupplier;
        private readonly Dictionary<object, Indexer<T>> downstreamIndexerMap = new Dictionary<object, Indexer<T>>();

        public EqualsIndexer(Func<Indexer<T>> downstreamIndexerSupplier)
            : this(0, 1, downstreamIndexerSupplier)
        {

        }

        public EqualsIndexer(int indexKeyFromInclusive, int indexKeyToExclusive, Func<Indexer<T>> downstreamIndexerSupplier)
        {
            this.indexKeyFrom = indexKeyFromInclusive;
            this.indexKeyTo = indexKeyToExclusive;
            this.downstreamIndexerSupplier = downstreamIndexerSupplier;
        }

        public void ForEach(IndexProperties indexProperties, Action<T> tupleConsumer)
        {
            var indexKey = indexProperties.ToKey(indexKeyFrom, indexKeyTo);
            downstreamIndexerMap.TryGetValue(indexKey, out Indexer<T> downstreamIndexer);
            if (downstreamIndexer == null || downstreamIndexer.IsEmpty())
            {
                return;
            }
            downstreamIndexer.ForEach(indexProperties, tupleConsumer);
        }

        public bool IsEmpty()
        {
            return downstreamIndexerMap.Count == 0;
        }

        public ElementAwareListEntry<T> Put(IndexProperties indexProperties, T tuple)
        {
            var indexKey = indexProperties.ToKey(indexKeyFrom, indexKeyTo);
            // Avoids computeIfAbsent in order to not create lambdas on the hot path.
            downstreamIndexerMap.TryGetValue(indexKey, out Indexer<T> downstreamIndexer);
            if (downstreamIndexer == null)
            {
                downstreamIndexer = downstreamIndexerSupplier.Invoke();
                downstreamIndexerMap.Add(indexKey, downstreamIndexer);
            }
            return downstreamIndexer.Put(indexProperties, tuple);
        }

        private Indexer<T> GetDownstreamIndexer(IndexProperties indexProperties, object indexerKey, ElementAwareListEntry<T> entry)
        {
            return downstreamIndexerMap[indexerKey];
        }

        public void Remove(IndexProperties indexProperties, ElementAwareListEntry<T> entry)
        {
            var indexKey = indexProperties.ToKey(indexKeyFrom, indexKeyTo);
            Indexer<T> downstreamIndexer = GetDownstreamIndexer(indexProperties, indexKey, entry);
            downstreamIndexer.Remove(indexProperties, entry);
            if (downstreamIndexer.IsEmpty())
            {
                downstreamIndexerMap.Remove(indexKey);
            }
        }
    }
}
