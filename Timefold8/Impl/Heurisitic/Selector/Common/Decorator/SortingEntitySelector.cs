using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public sealed class SortingEntitySelector : AbstractCachingEntitySelector
    {

        private readonly SelectionSorter<Object> sorter;

        public SortingEntitySelector(EntitySelector childEntitySelector, SelectionCacheType cacheType, SelectionSorter<Object> sorter)
            : base(childEntitySelector, cacheType)
        {
            this.sorter = sorter;
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
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
