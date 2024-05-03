using TimefoldSharp.Core.API.Domain.ValueRange;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange
{
    public abstract class AbstractCountableValueRange<T> : CountableValueRange<T>
    {
        public bool Contains(T value)
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerator<T> CreateOriginalIterator();

        public abstract IEnumerator<T> CreateRandomIterator(Random workingRandom);

        public abstract long GetSize();

        public bool IsEmpty()
        {
            return GetSize() == 0;
        }
    }
}
