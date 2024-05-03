using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public abstract class AbstractInverseEntityFilteringValueSelector : AbstractDemandEnabledSelector, EntityIndependentValueSelector
    {

        protected readonly EntityIndependentValueSelector childValueSelector;

        protected SingletonInverseVariableSupply inverseVariableSupply;

        protected AbstractInverseEntityFilteringValueSelector(EntityIndependentValueSelector childValueSelector)
        {
            if (childValueSelector.IsNeverEnding())
            {
                throw new Exception("The selector (" + this
                        + ") has a childValueSelector (" + childValueSelector
                        + ") with neverEnding (" + childValueSelector.IsNeverEnding() + ").\n"
                        + "This is not allowed because  cannot decorate a never-ending child value selector.\n"
                    + "This could be a result of using random selection order (which is often the default).");
            }
            this.childValueSelector = childValueSelector;
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
