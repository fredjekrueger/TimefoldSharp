using System.Collections;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class ShufflingValueSelector : AbstractCachingValueSelector, EntityIndependentValueSelector
    {
        public ShufflingValueSelector(EntityIndependentValueSelector childValueSelector, SelectionCacheType cacheType)
            : base(childValueSelector, cacheType)
        {
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
    }
}
