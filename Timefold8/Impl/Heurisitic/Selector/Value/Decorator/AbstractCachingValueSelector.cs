using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public abstract class AbstractCachingValueSelector : AbstractDemandEnabledSelector, SelectionCacheLifecycleListener, ValueSelector
    {

        protected readonly EntityIndependentValueSelector childValueSelector;
        protected readonly SelectionCacheType cacheType;

        protected List<Object> cachedValueList = null;

        public AbstractCachingValueSelector(EntityIndependentValueSelector childValueSelector,
                SelectionCacheType cacheType)
        {
            this.childValueSelector = childValueSelector;
            this.cacheType = cacheType;
            if (childValueSelector.IsNeverEnding())
            {
                throw new Exception("The selector (" + this
                + ") has a childValueSelector (" + childValueSelector
                        + ") with neverEnding (" + childValueSelector.IsNeverEnding() + ").");
            }
            phaseLifecycleSupport.AddEventListener(childValueSelector);
            if (!SelectionCacheTypeHelper.IsCached(cacheType))
            {
                throw new Exception("The selector (" + this
                        + ") does not support the cacheType (" + cacheType + ").");
            }
            phaseLifecycleSupport.AddEventListener(new SelectionCacheLifecycleBridge(cacheType, this));
        }

        public long GetSize(object entity)
        {
            return GetSize();
        }

        public long GetSize()
        {
            return cachedValueList.Count;
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
