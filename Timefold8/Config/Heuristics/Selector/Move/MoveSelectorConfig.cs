using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Util;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move
{
    public interface MoveSelectorConfig<Config_> : SelectorConfig<Config_> where Config_ : MoveSelectorConfig<Config_>
    {
        MoveSelectorConfigImpl MoveSelectorConfigImpl { get; set; }
    }

    public class MoveSelectorConfigImpl
    {
        public SelectionCacheType? CacheType { get; set; } = null;
        public Type FilterClass { get; set; } = null;
        public Type ProbabilityWeightFactoryClass { get; set; } = null;
        public long? SelectedCountLimit { get; set; } = null;
        public SelectionOrder? SelectionOrder { get; set; } = null;
        public Type SorterClass { get; set; } = null;
        public SelectionSorterOrder? SorterOrder { get; set; } = null;
        public Type SorterWeightFactoryClass { get; set; } = null;
        public double? FixedProbabilityWeight { get; set; } = null;
        public Type SorterComparatorClass { get; set; } = null;
        public void InheritFolded<Config_>(MoveSelectorConfig<Config_> foldedConfig) where Config_ : MoveSelectorConfig<Config_>
        {
            InheritCommon(foldedConfig);
        }

        private void InheritCommon<Config_>(MoveSelectorConfig<Config_> inheritedConfig) where Config_ : MoveSelectorConfig<Config_>
        {
            CacheType = ConfigUtils.InheritOverwritableProperty(CacheType, inheritedConfig.MoveSelectorConfigImpl.CacheType);
            SelectionOrder = ConfigUtils.InheritOverwritableProperty(SelectionOrder, inheritedConfig.MoveSelectorConfigImpl.SelectionOrder);
            FilterClass = ConfigUtils.InheritOverwritableProperty(FilterClass, inheritedConfig.MoveSelectorConfigImpl.FilterClass);
            SorterComparatorClass = ConfigUtils.InheritOverwritableProperty(SorterComparatorClass, inheritedConfig.MoveSelectorConfigImpl.SorterComparatorClass);
            SorterWeightFactoryClass = ConfigUtils.InheritOverwritableProperty(SorterWeightFactoryClass, inheritedConfig.MoveSelectorConfigImpl.SorterWeightFactoryClass);
            SorterOrder = ConfigUtils.InheritOverwritableProperty(SorterOrder, inheritedConfig.MoveSelectorConfigImpl.SorterOrder);
            SorterClass = ConfigUtils.InheritOverwritableProperty(SorterClass, inheritedConfig.MoveSelectorConfigImpl.SorterClass);
            ProbabilityWeightFactoryClass = ConfigUtils.InheritOverwritableProperty(ProbabilityWeightFactoryClass, inheritedConfig.MoveSelectorConfigImpl.ProbabilityWeightFactoryClass);
            SelectedCountLimit = ConfigUtils.InheritOverwritableProperty(SelectedCountLimit, inheritedConfig.MoveSelectorConfigImpl.SelectedCountLimit);
            FixedProbabilityWeight = ConfigUtils.InheritOverwritableProperty(FixedProbabilityWeight, inheritedConfig.MoveSelectorConfigImpl.FixedProbabilityWeight);
        }
    }
}