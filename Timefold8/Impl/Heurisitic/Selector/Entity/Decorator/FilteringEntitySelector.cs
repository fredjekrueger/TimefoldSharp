using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public sealed class FilteringEntitySelector : AbstractDemandEnabledSelector, EntitySelector
    {

        private readonly EntitySelector childEntitySelector;
        private readonly SelectionFilter<Object> selectionFilter;
        private readonly bool bailOutEnabled;

        public FilteringEntitySelector(EntitySelector childEntitySelector, List<SelectionFilter<object>> filterList)
        {
            this.childEntitySelector = childEntitySelector;
            if (filterList == null || filterList.Count == 0)
                throw new Exception("must have at least one filter, but got (" + filterList + ").");

            this.selectionFilter = SelectionFilter<object>.Compose(filterList);
            bailOutEnabled = childEntitySelector.IsNeverEnding();
            phaseLifecycleSupport.AddEventListener(childEntitySelector);
        }

        public IEnumerator<object> EndingIterator()
        {
            throw new NotImplementedException();
        }

        public EntityDescriptor GetEntityDescriptor()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize()
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

        public IEnumerator<object> ListIterator()
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
