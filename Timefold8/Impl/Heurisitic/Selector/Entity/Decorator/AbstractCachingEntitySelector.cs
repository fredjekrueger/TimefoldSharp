using System.Collections;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public abstract class AbstractCachingEntitySelector
        : AbstractDemandEnabledSelector, SelectionCacheLifecycleListener, EntitySelector
    {
        private EntitySelector childEntitySelector;
        private SelectionCacheType? cacheType;

        protected AbstractCachingEntitySelector(EntitySelector childEntitySelector, SelectionCacheType? cacheType)
        {
            this.childEntitySelector = childEntitySelector;
            this.cacheType = cacheType;

            phaseLifecycleSupport.AddEventListener(childEntitySelector);
            phaseLifecycleSupport.AddEventListener(new SelectionCacheLifecycleBridge(cacheType, this));
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
