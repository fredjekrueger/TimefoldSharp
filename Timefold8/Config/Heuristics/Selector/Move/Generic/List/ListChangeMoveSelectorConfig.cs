using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic.List
{
    public class ListChangeMoveSelectorConfig : MoveSelectorConfig<AbstractMoveSelectorConfig>
    {
        private ValueSelectorConfig valueSelectorConfig = null;

        public ListChangeMoveSelectorConfig Inherit(ListChangeMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public ListChangeMoveSelectorConfig WithValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.SetValueSelectorConfig(valueSelectorConfig);
            return this;
        }

        public void SetValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.valueSelectorConfig = valueSelectorConfig;
        }

        public SelectionCacheType? GetCacheType()
        {
            throw new NotImplementedException();
        }

        public Type GetFilterClass()
        {
            throw new NotImplementedException();
        }

        public Type GetProbabilityWeightFactoryClass()
        {
            throw new NotImplementedException();
        }

        public long? GetSelectedCountLimit()
        {
            throw new NotImplementedException();
        }

        public SelectionOrder? GetSelectionOrder()
        {
            throw new NotImplementedException();
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

        public double? GetFixedProbabilityWeight()
        {
            throw new NotImplementedException();
        }

        public Type GetSorterComparatorClass()
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig Inherit(AbstractMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public void InheritFolded(MoveSelectorConfig<AbstractMoveSelectorConfig> foldedConfig)
        {
            throw new NotImplementedException();
        }
    }
}
