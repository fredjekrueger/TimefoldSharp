using System.Collections;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class SortingValueSelector : AbstractCachingValueSelector, EntityIndependentValueSelector
    {
        readonly SelectionSorter<object> sorter;

        public SortingValueSelector(EntityIndependentValueSelector childValueSelector, SelectionCacheType cacheType,
                SelectionSorter<object> sorter) : base(childValueSelector, cacheType)
        {
            this.sorter = sorter;
        }

        public IEnumerator<object> GetEnumerator()
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
