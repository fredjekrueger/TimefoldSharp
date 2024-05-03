using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Buildin.Collection
{
    public class ListValueRange<T> : AbstractCountableValueRange<T>
    {
        private IEnumerable<T> list;

        public ListValueRange(IEnumerable<T> list)
        {
            this.list = list;
        }

        public override IEnumerator<T> CreateOriginalIterator()
        {
            return list.GetEnumerator();
        }

        public override IEnumerator<T> CreateRandomIterator(Random workingRandom)
        {
            return new CachedListRandomIterator<T>(list.ToList(), workingRandom);
        }

        public override long GetSize()
        {
            return list.Count();
        }
    }
}
