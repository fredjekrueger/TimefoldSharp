using TimefoldSharp.Core.Config.Heuristics.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public sealed class ShufflingEntitySelector : AbstractCachingEntitySelector
    {

        public ShufflingEntitySelector(EntitySelector childEntitySelector, SelectionCacheType cacheType)
            : base(childEntitySelector, cacheType)
        {

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
