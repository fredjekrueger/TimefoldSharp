using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite
{
    public class UnionMoveSelectorConfig : MoveSelectorConfig<UnionMoveSelectorConfig>, AbstractMoveSelectorConfig
    {
        protected SelectionCacheType? cacheType = null;
        protected SelectionOrder? selectionOrder = null;
        protected Type selectorProbabilityWeightFactoryClass;
        protected Type filterClass;
        protected Type sorterComparatorClass;
        protected Type sorterWeightFactoryClass;
        protected SelectionSorterOrder? sorterOrder = null;
        protected Type sorterClass = null;

        protected Type probabilityWeightFactoryClass = null;

        protected long? selectedCountLimit = null;

        private double? fixedProbabilityWeight = null;

        public UnionMoveSelectorConfig()
        {
        }

        public UnionMoveSelectorConfig(List<AbstractMoveSelectorConfig> moveSelectorConfigList)
        {
            this.moveSelectorConfigList = moveSelectorConfigList;
        }

        public UnionMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public UnionMoveSelectorConfig Inherit(UnionMoveSelectorConfig inheritedConfig)
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

        private List<AbstractMoveSelectorConfig> moveSelectorConfigList = null;

        public UnionMoveSelectorConfig WithMoveSelectors(List<AbstractMoveSelectorConfig> moveSelectorConfigs)
        {
            this.moveSelectorConfigList = moveSelectorConfigs;
            return this;
        }

        AbstractMoveSelectorConfig AbstractConfig<AbstractMoveSelectorConfig>.CopyConfig()
        {
            throw new NotImplementedException();
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
            return probabilityWeightFactoryClass;
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
            return sorterClass;
        }

        public SelectionSorterOrder? GetSorterOrder()
        {
            return sorterOrder;
        }

        public Type GetSorterWeightFactoryClass()
        {
            return sorterWeightFactoryClass;
        }

        public Type GetSorterComparatorClass()
        {
            return sorterComparatorClass;
        }

        public List<AbstractMoveSelectorConfig> GetMoveSelectorList()
        {
            return moveSelectorConfigList;
        }

        internal Type GetSelectorProbabilityWeightFactoryClass()
        {
            return selectorProbabilityWeightFactoryClass;
        }

        public double? GetFixedProbabilityWeight()
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<UnionMoveSelectorConfig> foldedConfig)
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<AbstractMoveSelectorConfig> foldedConfig)
        {
            InheritCommon(foldedConfig);
        }

        private void InheritCommon(MoveSelectorConfig<AbstractMoveSelectorConfig> inheritedConfig)
        {
            cacheType = ConfigUtils.InheritOverwritableProperty(cacheType, inheritedConfig.GetCacheType());
            selectionOrder = ConfigUtils.InheritOverwritableProperty(selectionOrder, inheritedConfig.GetSelectionOrder());
            filterClass = ConfigUtils.InheritOverwritableProperty(filterClass, inheritedConfig.GetFilterClass());
            sorterComparatorClass = ConfigUtils.InheritOverwritableProperty(sorterComparatorClass, inheritedConfig.GetSorterComparatorClass());
            sorterWeightFactoryClass = ConfigUtils.InheritOverwritableProperty(sorterWeightFactoryClass, inheritedConfig.GetSorterWeightFactoryClass());
            sorterOrder = ConfigUtils.InheritOverwritableProperty(sorterOrder, inheritedConfig.GetSorterOrder());
            sorterClass = ConfigUtils.InheritOverwritableProperty(sorterClass, inheritedConfig.GetSorterClass());
            probabilityWeightFactoryClass = ConfigUtils.InheritOverwritableProperty(probabilityWeightFactoryClass, inheritedConfig.GetProbabilityWeightFactoryClass());
            selectedCountLimit = ConfigUtils.InheritOverwritableProperty(selectedCountLimit, inheritedConfig.GetSelectedCountLimit());

            fixedProbabilityWeight = ConfigUtils.InheritOverwritableProperty(fixedProbabilityWeight, inheritedConfig.GetFixedProbabilityWeight());
        }
    }
}
