using System.Collections;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class ProbabilityValueSelector : AbstractDemandEnabledSelector, EntityIndependentValueSelector, SelectionCacheLifecycleListener
    {
        private readonly EntityIndependentValueSelector childValueSelector;
        private readonly SelectionCacheType cacheType;
        private readonly SelectionProbabilityWeightFactory<Object> probabilityWeightFactory;

        double probabilityWeightTotal = -1.0;

        public ProbabilityValueSelector(EntityIndependentValueSelector childValueSelector,
                SelectionCacheType cacheType,
                SelectionProbabilityWeightFactory<Object> probabilityWeightFactory)
        {
            this.childValueSelector = childValueSelector;
            this.cacheType = cacheType;
            this.probabilityWeightFactory = probabilityWeightFactory;
            if (childValueSelector.IsNeverEnding())
            {
                throw new Exception("The selector (" + this
                        + ") has a childValueSelector (" + childValueSelector
                        + ") with neverEnding (" + childValueSelector.IsNeverEnding() + ").");
            }
            phaseLifecycleSupport.AddEventListener(childValueSelector);
            if (!SelectionCacheTypeHelper.IsCached(cacheType))
            {
                throw new Exception("The selector (" + this
                        + ") does not support the cacheType (" + cacheType + ").");
            }
            phaseLifecycleSupport.AddEventListener(new SelectionCacheLifecycleBridge(cacheType, this));
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
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

        public IEnumerator<object> Iterator(object entity)
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
