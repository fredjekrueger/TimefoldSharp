using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class DowncastingValueSelector : AbstractDemandEnabledSelector, ValueSelector
    {

        private readonly ValueSelector childValueSelector;
        private readonly Type downcastEntityClass;

        public DowncastingValueSelector(ValueSelector childValueSelector, Type downcastEntityClass)
        {
            this.childValueSelector = childValueSelector;
            this.downcastEntityClass = downcastEntityClass;
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
            return childValueSelector.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return childValueSelector.IsNeverEnding();
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