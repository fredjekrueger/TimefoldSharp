using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.AbstractSelectors;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator
{
    public class ProbabilityMoveSelector : AbstractMoveSelector, SelectionCacheLifecycleListener
    {

        protected readonly MoveSelector childMoveSelector;
        protected readonly SelectionCacheType cacheType;
        protected readonly SelectionProbabilityWeightFactory<Heurisitic.Move.Move> probabilityWeightFactory;

        public ProbabilityMoveSelector(MoveSelector childMoveSelector, SelectionCacheType cacheType,
            SelectionProbabilityWeightFactory<Heurisitic.Move.Move> probabilityWeightFactory)
        {
            this.childMoveSelector = childMoveSelector;
            this.cacheType = cacheType;
            this.probabilityWeightFactory =
                    (SelectionProbabilityWeightFactory<Heurisitic.Move.Move>)probabilityWeightFactory;
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

        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override long GetSize()
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
    }

}
