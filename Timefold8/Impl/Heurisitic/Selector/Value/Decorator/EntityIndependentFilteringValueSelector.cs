using System.Collections;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class EntityIndependentFilteringValueSelector : FilteringValueSelector, EntityIndependentValueSelector
    {

        public EntityIndependentFilteringValueSelector(EntityIndependentValueSelector childValueSelector,
                List<SelectionFilter<object>> filterList) : base(childValueSelector, filterList)
        {
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
