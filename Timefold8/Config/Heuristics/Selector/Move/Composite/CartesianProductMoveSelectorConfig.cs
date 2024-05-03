using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite
{
    public class CartesianProductMoveSelectorConfig : MoveSelectorConfig<CartesianProductMoveSelectorConfig>, AbstractMoveSelectorConfig
    {
        private bool? ignoreEmptyChildIterators = null;
        SelectionCacheType? cacheType = null;
        SelectionOrder? selectionOrder = null;
        Type filterClass = null;
        long? selectedCountLimit = null;

        private List<AbstractMoveSelectorConfig> moveSelectorConfigList = null;

        public CartesianProductMoveSelectorConfig(List<AbstractMoveSelectorConfig> moveSelectorConfigList)
        {
            this.moveSelectorConfigList = moveSelectorConfigList;
        }

        public CartesianProductMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public CartesianProductMoveSelectorConfig Inherit(CartesianProductMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig Inherit(AbstractMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public List<AbstractMoveSelectorConfig> GetMoveSelectorList()
        {
            return moveSelectorConfigList;
        }

        AbstractMoveSelectorConfig AbstractConfig<AbstractMoveSelectorConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }

        public bool? GetIgnoreEmptyChildIterators()
        {
            return ignoreEmptyChildIterators;
        }

        public SelectionCacheType? GetCacheType()
        {
            return cacheType;
        }

        public Type GetFilterClass()
        {
            return filterClass;
        }

        public Type GetProbabilityWeightFactoryClass()
        {
            throw new NotImplementedException();
        }

        public long? GetSelectedCountLimit()
        {
            return selectedCountLimit;
        }

        public SelectionOrder? GetSelectionOrder()
        {
            return selectionOrder;
        }

        public Type GetSorterClass()
        {
            throw new NotImplementedException();
        }

        public SelectionSorterOrder? GetSorterOrder()
        {
            throw new NotImplementedException();
        }

        public Type GetSorterWeightFactoryClass()
        {
            throw new NotImplementedException();
        }

        public Type GetSorterComparatorClass()
        {
            throw new NotImplementedException();
        }

        public double? GetFixedProbabilityWeight()
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<CartesianProductMoveSelectorConfig> foldedConfig)
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<AbstractMoveSelectorConfig> foldedConfig)
        {
            throw new NotImplementedException();
        }
    }
}
