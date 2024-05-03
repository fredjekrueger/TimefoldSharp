using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class ReinitializeVariableValueSelector : AbstractDemandEnabledSelector, ValueSelector
    {

        private readonly ValueSelector childValueSelector;

        public ReinitializeVariableValueSelector(ValueSelector childValueSelector)
        {
            this.childValueSelector = childValueSelector;
            phaseLifecycleSupport.AddEventListener(childValueSelector);
        }

        public long GetSize(object entity)
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
