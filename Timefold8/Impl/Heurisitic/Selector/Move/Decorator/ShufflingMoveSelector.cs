using TimefoldSharp.Core.Config.Heuristics.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator
{
    public class ShufflingMoveSelector : AbstractCachingMoveSelector
    {

        public ShufflingMoveSelector(MoveSelector childMoveSelector, SelectionCacheType cacheType)
                : base(childMoveSelector, cacheType)
        {
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
