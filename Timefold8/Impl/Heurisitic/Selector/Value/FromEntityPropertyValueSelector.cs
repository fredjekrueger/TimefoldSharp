using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value
{
    public sealed class FromEntityPropertyValueSelector : AbstractDemandEnabledSelector, ValueSelector
    {

        private readonly ValueRangeDescriptor valueRangeDescriptor;
        private readonly bool randomSelection;

        private ISolution workingSolution;

        public FromEntityPropertyValueSelector(ValueRangeDescriptor valueRangeDescriptor, bool randomSelection)
        {
            this.valueRangeDescriptor = valueRangeDescriptor;
            this.randomSelection = randomSelection;
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            return randomSelection || !IsCountable();
        }

        public override bool IsCountable()
        {
            return valueRangeDescriptor.IsCountable();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize(object entity)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
