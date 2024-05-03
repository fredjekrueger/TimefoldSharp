using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public class InitializedValueSelector : AbstractDemandEnabledSelector, ValueSelector
    {
        readonly GenuineVariableDescriptor variableDescriptor;
        readonly ValueSelector childValueSelector;
        readonly bool bailOutEnabled;


        public static ValueSelector Create(ValueSelector valueSelector)
        {
            if (valueSelector is EntityIndependentValueSelector)
            {
                return new EntityIndependentInitializedValueSelector((EntityIndependentValueSelector)valueSelector);
            }
            else
            {
                return new InitializedValueSelector(valueSelector);
            }
        }

        public override bool IsCountable()
        {
            return childValueSelector.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return childValueSelector.IsNeverEnding();
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize(object entity)
        {
            throw new NotImplementedException();
        }

        protected InitializedValueSelector(ValueSelector childValueSelector)
        {
            this.variableDescriptor = childValueSelector.GetVariableDescriptor();
            this.childValueSelector = childValueSelector;
            bailOutEnabled = childValueSelector.IsNeverEnding();
            phaseLifecycleSupport.AddEventListener(childValueSelector);
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
