using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public class FilteringValueSelector : AbstractDemandEnabledSelector, ValueSelector
    {

        protected readonly ValueSelector childValueSelector;
        private readonly SelectionFilter<Object> selectionFilter;
        protected readonly bool bailOutEnabled;

        public static ValueSelector Create(ValueSelector valueSelector, List<SelectionFilter<Object>> filterList)
        {
            if (valueSelector is EntityIndependentValueSelector)
            {
                return new EntityIndependentFilteringValueSelector((EntityIndependentValueSelector)valueSelector, filterList);
            }
            else
            {
                return new FilteringValueSelector(valueSelector, filterList);
            }
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            return childValueSelector.IsNeverEnding();
        }

        public override bool IsCountable()
        {
            return childValueSelector.IsCountable();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize(object entity)
        {
            throw new NotImplementedException();
        }

        protected FilteringValueSelector(ValueSelector childValueSelector, List<SelectionFilter<Object>> filterList)
        {
            this.childValueSelector = childValueSelector;
            this.selectionFilter = SelectionFilter<object>.Compose(filterList);
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
