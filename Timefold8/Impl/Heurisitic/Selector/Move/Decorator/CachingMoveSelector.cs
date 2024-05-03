using TimefoldSharp.Core.Config.Heuristics.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator
{
    public class CachingMoveSelector : AbstractCachingMoveSelector
    {

        protected readonly bool randomSelection;

        public CachingMoveSelector(MoveSelector childMoveSelector, SelectionCacheType cacheType, bool randomSelection)
                : base(childMoveSelector, cacheType)
        {
            this.randomSelection = randomSelection;
        }

        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
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

