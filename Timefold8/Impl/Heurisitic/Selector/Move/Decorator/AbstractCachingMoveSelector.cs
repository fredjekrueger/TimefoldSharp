using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.AbstractSelectors;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator
{
    public abstract class AbstractCachingMoveSelector : AbstractMoveSelector, SelectionCacheLifecycleListener
    {

        protected readonly MoveSelector childMoveSelector;
        protected readonly SelectionCacheType cacheType;

        protected List<Heurisitic.Move.Move> cachedMoveList = null;

        public AbstractCachingMoveSelector(MoveSelector childMoveSelector, SelectionCacheType cacheType)
        {
            this.childMoveSelector = childMoveSelector;
            this.cacheType = cacheType;
            if (childMoveSelector.IsNeverEnding())
            {
                throw new Exception("The selector (" + this
                        + ") has a childMoveSelector (" + childMoveSelector
                        + ") with neverEnding (" + childMoveSelector.IsNeverEnding() + ").");
            }
            phaseLifecycleSupport.AddEventListener(childMoveSelector);
            if (!SelectionCacheTypeHelper.IsCached(cacheType))
            {
                throw new Exception("The selector (" + this
                        + ") does not support the cacheType (" + cacheType + ").");
            }
            phaseLifecycleSupport.AddEventListener(new SelectionCacheLifecycleBridge(cacheType, this));
        }

        public override long GetSize()
        {
            throw new NotImplementedException();
        }
    }
}
