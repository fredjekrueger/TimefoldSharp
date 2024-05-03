using TimefoldSharp.Core.API.Domain.ValueRange;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Buildin.Composite
{
    public class NullableCountableValueRange<T> : AbstractCountableValueRange<T>
    {

        private readonly CountableValueRange<T> childValueRange;
        private readonly long size;

        public NullableCountableValueRange(CountableValueRange<T> childValueRange)
        {
            this.childValueRange = childValueRange;
            size = childValueRange.GetSize() + 1L;
        }

        public override IEnumerator<T> CreateOriginalIterator()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<T> CreateRandomIterator(Random workingRandom)
        {
            throw new NotImplementedException();
        }

        public override long GetSize()
        {
            throw new NotImplementedException();
        }
    }
}
