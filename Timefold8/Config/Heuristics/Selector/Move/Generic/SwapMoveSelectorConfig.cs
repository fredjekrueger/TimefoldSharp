using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic
{
    public class SwapMoveSelectorConfig : MoveSelectorConfig<SwapMoveSelectorConfig>, AbstractMoveSelectorConfig
    {

        private EntitySelectorConfig entitySelectorConfig = null;
        private ValueSelectorConfig valueSelectorConfig = null;
        protected SelectionCacheType? cacheType = null;
        protected SelectionOrder? selectionOrder = null;
        protected Type filterClass;
        protected long? selectedCountLimit = null;
        protected Type sorterComparatorClass;
        protected Type sorterWeightFactoryClass;
        protected SelectionSorterOrder? sorterOrder = null;
        protected Type sorterClass = null;
        protected Type probabilityWeightFactoryClass = null;
        private double? fixedProbabilityWeight = null;

        private EntitySelectorConfig secondaryEntitySelectorConfig = null;
        private List<string> variableNameIncludeList = null;

        public SwapMoveSelectorConfig CopyConfig()
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

        public double? GetFixedProbabilityWeight()
        {
            return fixedProbabilityWeight;
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

        public Type GetSorterComparatorClass()
        {
            return sorterComparatorClass;
        }

        public SelectionSorterOrder? GetSorterOrder()
        {
            return sorterOrder;
        }

        public Type GetSorterWeightFactoryClass()
        {
            return sorterWeightFactoryClass;
        }

        public SwapMoveSelectorConfig Inherit(SwapMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig Inherit(AbstractMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<SwapMoveSelectorConfig> foldedConfig)
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<AbstractMoveSelectorConfig> foldedConfig)
        {
            InheritCommon(foldedConfig);
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        AbstractMoveSelectorConfig AbstractConfig<AbstractMoveSelectorConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }

        public EntitySelectorConfig GetEntitySelectorConfig()
        {
            return entitySelectorConfig;
        }

        public EntitySelectorConfig GetSecondaryEntitySelectorConfig()
        {
            return secondaryEntitySelectorConfig;
        }

        public void SetEntitySelectorConfig(EntitySelectorConfig entitySelectorConfig)
        {
            this.entitySelectorConfig = entitySelectorConfig;
        }

        public void SetSecondaryEntitySelectorConfig(EntitySelectorConfig secondaryEntitySelectorConfig)
        {
            this.secondaryEntitySelectorConfig = secondaryEntitySelectorConfig;
        }

        public void SetVariableNameIncludeList(List<string> variableNameIncludeList)
        {
            this.variableNameIncludeList = variableNameIncludeList;
        }

        public List<string> GetVariableNameIncludeList()
        {
            return variableNameIncludeList;
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
