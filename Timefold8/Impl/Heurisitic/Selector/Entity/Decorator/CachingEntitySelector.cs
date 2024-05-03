using TimefoldSharp.Core.Config.Heuristics.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public sealed class CachingEntitySelector : AbstractCachingEntitySelector
    {

        private readonly bool randomSelection;

        public CachingEntitySelector(EntitySelector childEntitySelector, SelectionCacheType cacheType, bool randomSelection)
            : base(childEntitySelector, cacheType)
        {
            this.randomSelection = randomSelection;
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
