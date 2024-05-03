using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class EntityDependentSortingValueSelector : AbstractDemandEnabledSelector, ValueSelector
    {

        private readonly ValueSelector childValueSelector;
        private readonly SelectionCacheType cacheType;
        private readonly SelectionSorter<object> sorter;

        ScoreDirector scoreDirector = null;

        public EntityDependentSortingValueSelector(ValueSelector childValueSelector,
                SelectionCacheType cacheType, SelectionSorter<object> sorter)
        {
            this.childValueSelector = childValueSelector;
            this.cacheType = cacheType;
            this.sorter = sorter;
            if (childValueSelector.IsNeverEnding())
            {
                throw new Exception("The selector (" + this
                        + ") has a childValueSelector (" + childValueSelector
                        + ") with neverEnding (" + childValueSelector.IsNeverEnding() + ").");
            }
            if (cacheType != SelectionCacheType.STEP)
            {
                throw new Exception("The selector (" + this
                        + ") does not support the cacheType (" + cacheType + ").");
            }
            phaseLifecycleSupport.AddEventListener(childValueSelector);
        }

        public override bool IsNeverEnding()
        {
            return false;
        }

        public override bool IsCountable()
        {
            return true;
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize(object entity)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
