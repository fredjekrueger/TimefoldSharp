using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public class NoneIndexer<T> : Indexer<T>
    {
        private ElementAwareList<T> tupleList = new ElementAwareList<T>();

        public void ForEach(IndexProperties indexProperties, Action<T> tupleConsumer)
        {
            tupleList.ForEach(tupleConsumer);
        }

        public bool IsEmpty()
        {
            return tupleList.Size() == 0;
        }

        public ElementAwareListEntry<T> Put(IndexProperties indexProperties, T tuple)
        {
            return tupleList.Add(tuple);
        }

        public void Remove(IndexProperties indexProperties, ElementAwareListEntry<T> entry)
        {
            entry.Remove();
        }
    }
}
