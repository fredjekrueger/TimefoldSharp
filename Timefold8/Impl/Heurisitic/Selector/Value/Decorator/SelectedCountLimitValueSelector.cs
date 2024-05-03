using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class SelectedCountLimitValueSelector : AbstractDemandEnabledSelector, EntityIndependentValueSelector
    {

        private readonly ValueSelector childValueSelector;
        private readonly long? selectedCountLimit;

        public SelectedCountLimitValueSelector(ValueSelector childValueSelector, long? selectedCountLimit)
        {
            this.childValueSelector = childValueSelector;
            this.selectedCountLimit = selectedCountLimit;
            if (selectedCountLimit < 0L)
            {
                throw new Exception("The selector (" + this
                        + ") has a negative selectedCountLimit (" + selectedCountLimit + ").");
            }
            phaseLifecycleSupport.AddEventListener(childValueSelector);
        }


        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
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
