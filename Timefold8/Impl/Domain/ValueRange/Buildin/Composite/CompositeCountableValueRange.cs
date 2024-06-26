using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Solver.Random;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Buildin.Composite
{
    public class CompositeCountableValueRange<T> : AbstractCountableValueRange<T>
    {
        private List<CountableValueRange<T>> childValueRangeList;
        private long size;

        public CompositeCountableValueRange(List<CountableValueRange<T>> childValueRangeList)
        {
            this.childValueRangeList = childValueRangeList;
            long size = 0L;
            foreach (var childValueRange in childValueRangeList)
            {
                size += childValueRange.GetSize();
            }
            this.size = size;
        }

        public override long GetSize()
        {
            return size;
        }

        public override T Get(long index)
        {
            long remainingIndex = index;
            foreach (var childValueRange in childValueRangeList)
            {
                long childSize = childValueRange.GetSize();
                if (remainingIndex < childSize)
                {
                    return childValueRange.Get(remainingIndex);
                }
                remainingIndex -= childSize;
            }
            throw new Exception("The index (" + index + ") must be less than the size (" + size + ").");
        }

        public override IEnumerator<T> CreateRandomIterator(Random workingRandom)
        {
            return new RandomCompositeValueRangeIterator(workingRandom, this);
        }

        public override bool Contains(T value)
        {
            foreach (var childValueRange in childValueRangeList)
            {
                if (childValueRange.Contains(value))
                {
                    return true;
                }
            }
            return false;
        }

        public override IEnumerator<T> CreateOriginalIterator()
        {
            IEnumerable<T> stream = Enumerable.Empty<T>();
            foreach (CountableValueRange<T> childValueRange in childValueRangeList)
            {
                stream = stream.Concat(OriginalIteratorToStream(childValueRange));
            }
            return stream.GetEnumerator();
        }

        private static IEnumerable<T> OriginalIteratorToStream(CountableValueRange<T> valueRange)
        {
            return valueRange.CreateOriginalIterator().AsEnumerable<T>().Take((int)valueRange.GetSize());
        }

        private class RandomCompositeValueRangeIterator : IEnumerator<T> {

        private Random workingRandom;
            CompositeCountableValueRange<T> parent;

            public RandomCompositeValueRangeIterator(Random workingRandom, CompositeCountableValueRange<T> parent)
            {
                this.workingRandom = workingRandom;
                this.parent = parent;
            }

            public T Current 
            {
                get
                {
                    long index = RandomUtils.NextLong(workingRandom, parent.size);
                    long remainingIndex = index;
                    foreach (var childValueRange in parent.childValueRangeList)
                    {
                        long childSize = childValueRange.GetSize();
                        if (remainingIndex < childSize)
                        {
                            return childValueRange.Get(remainingIndex);
                        }
                        remainingIndex -= childSize;
                    }
                    throw new Exception("Impossible state because index (" + index
                            + ") is always less than the size (" + parent.size + ").");
                }
            }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
}
}
