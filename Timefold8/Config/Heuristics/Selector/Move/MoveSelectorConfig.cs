using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move
{
    public interface MoveSelectorConfig<Config_> : SelectorConfig<Config_> where Config_ : MoveSelectorConfig<Config_>
    {
        SelectionCacheType? GetCacheType();
        Type GetFilterClass();
        Type GetProbabilityWeightFactoryClass();
        long? GetSelectedCountLimit();
        SelectionOrder? GetSelectionOrder();
        Type GetSorterClass();
        SelectionSorterOrder? GetSorterOrder();
        Type GetSorterWeightFactoryClass();
        double? GetFixedProbabilityWeight();
        Type GetSorterComparatorClass();
        void InheritFolded(MoveSelectorConfig<Config_> foldedConfig);
    }
}
