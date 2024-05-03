using System.Collections;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public sealed class ProbabilityEntitySelector
        : AbstractDemandEnabledSelector, SelectionCacheLifecycleListener, EntitySelector
    {

        public ProbabilityEntitySelector(EntitySelector childEntitySelector, SelectionCacheType cacheType,
            SelectionProbabilityWeightFactory<object> probabilityWeightFactory)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> EndingIterator()
        {
            throw new NotImplementedException();
        }

        public EntityDescriptor GetEntityDescriptor()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize()
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

        public IEnumerator<object> ListIterator()
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
