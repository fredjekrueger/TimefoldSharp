using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public abstract class AbstractEntityPlacerFactory<EntityPlacerConfig_>
        : AbstractFromConfigFactory<EntityPlacerConfig_>, EntityPlacerFactory
            where EntityPlacerConfig_ : EntityPlacerConfig<EntityPlacerConfig_>
    {
        protected AbstractEntityPlacerFactory(EntityPlacerConfig_ placerConfig) : base(placerConfig)
        {
        }

        protected AbstractMoveSelectorConfig BuildChangeMoveSelectorConfig(HeuristicConfigPolicy configPolicy, string entitySelectorConfigId, GenuineVariableDescriptor variableDescriptor)
        {
            ChangeMoveSelectorConfig changeMoveSelectorConfig = new ChangeMoveSelectorConfig();
            changeMoveSelectorConfig.SetEntitySelectorConfig(EntitySelectorConfig.NewMimicSelectorConfig(entitySelectorConfigId));
            ValueSelectorConfig changeValueSelectorConfig = new ValueSelectorConfig().WithVariableName(variableDescriptor.GetVariableName());
            if (ValueSelectorConfig.HasSorter(configPolicy.GetValueSorterManner(), variableDescriptor))
            {
                changeValueSelectorConfig = changeValueSelectorConfig
                        .WithCacheType(variableDescriptor.IsValueRangeEntityIndependent() ? SelectionCacheType.PHASE : SelectionCacheType.STEP)
                        .WithSelectionOrder(SelectionOrder.SORTED)
                        .WithSorterManner(configPolicy.GetValueSorterManner());
            }
            return changeMoveSelectorConfig.WithValueSelectorConfig(changeValueSelectorConfig);
        }

        public abstract EntityPlacer BuildEntityPlacer(HeuristicConfigPolicy configPolicy);
    }
}
