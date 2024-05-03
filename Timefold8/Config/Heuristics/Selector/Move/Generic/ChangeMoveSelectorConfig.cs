using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic
{
    public class ChangeMoveSelectorConfig : MoveSelectorConfig<ChangeMoveSelectorConfig>, AbstractMoveSelectorConfig
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

        public void SetEntitySelectorConfig(EntitySelectorConfig entitySelectorConfig)
        {
            this.entitySelectorConfig = entitySelectorConfig;
        }

        public EntitySelectorConfig GetEntitySelectorConfig()
        {
            return entitySelectorConfig;
        }

        public ChangeMoveSelectorConfig WithValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.SetValueSelectorConfig(valueSelectorConfig);
            return this;
        }

        public void SetValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.valueSelectorConfig = valueSelectorConfig;
        }

        public ValueSelectorConfig GetValueSelectorConfig()
        {
            return valueSelectorConfig;
        }

        public AbstractMoveSelectorConfig Inherit(AbstractMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public ChangeMoveSelectorConfig Inherit(ChangeMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        ChangeMoveSelectorConfig AbstractConfig<ChangeMoveSelectorConfig>.CopyConfig()
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

        public double? GetFixedProbabilityWeight()
        {
            return fixedProbabilityWeight;
        }

        public void InheritFolded(MoveSelectorConfig<ChangeMoveSelectorConfig> foldedConfig)
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<AbstractMoveSelectorConfig> foldedConfig)
        {
            throw new NotImplementedException();
        }
    }
}
