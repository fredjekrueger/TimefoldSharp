using System.Collections;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class CachingValueSelector : AbstractCachingValueSelector, EntityIndependentValueSelector
    {

        readonly bool randomSelection;

        public CachingValueSelector(EntityIndependentValueSelector childValueSelector, SelectionCacheType cacheType, bool randomSelection)
                : base(childValueSelector, cacheType)
        {
            this.randomSelection = randomSelection;
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
