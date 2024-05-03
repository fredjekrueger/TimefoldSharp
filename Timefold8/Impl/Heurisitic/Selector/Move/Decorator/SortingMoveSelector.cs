using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator
{
    public class SortingMoveSelector : AbstractCachingMoveSelector
    {
        protected readonly SelectionSorter<Heurisitic.Move.Move> sorter;

        public SortingMoveSelector(MoveSelector childMoveSelector, SelectionCacheType cacheType,
                SelectionSorter<Heurisitic.Move.Move> sorter)
                : base(childMoveSelector, cacheType)
        {

            this.sorter = sorter;
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
