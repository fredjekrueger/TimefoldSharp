using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public interface Indexer<T>
    {
        ElementAwareListEntry<T> Put(IndexProperties indexProperties, T tuple);
        void ForEach(IndexProperties indexProperties, Action<T> tupleConsumer);
        bool IsEmpty();
        void Remove(IndexProperties indexProperties, ElementAwareListEntry<T> entry);
    }
}
