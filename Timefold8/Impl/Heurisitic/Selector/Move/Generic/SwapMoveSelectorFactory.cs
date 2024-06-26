using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class SwapMoveSelectorFactory : AbstractMoveSelectorFactory<AbstractMoveSelectorConfig>
    {
        public SwapMoveSelectorConfig swapConfig => (SwapMoveSelectorConfig)config;

        protected override MoveSelector BuildBaseMoveSelector(HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, bool randomSelection)
        {
            EntitySelectorConfig entitySelectorConfig = swapConfig.GetEntitySelectorConfig() ?? new EntitySelectorConfig();
            EntitySelectorConfig secondaryEntitySelectorConfig = swapConfig.GetSecondaryEntitySelectorConfig() ?? entitySelectorConfig;
            SelectionOrder selectionOrder = SelectionOrderHelper.FromRandomSelectionBoolean(randomSelection);
            EntitySelector leftEntitySelector = EntitySelectorFactory.Create(entitySelectorConfig).BuildEntitySelector(configPolicy, minimumCacheType, selectionOrder);
            EntitySelector rightEntitySelector = EntitySelectorFactory.Create(secondaryEntitySelectorConfig).BuildEntitySelector(configPolicy, minimumCacheType, selectionOrder);
            EntityDescriptor entityDescriptor = leftEntitySelector.GetEntityDescriptor();
            List<GenuineVariableDescriptor> variableDescriptorList = DeduceVariableDescriptorList(entityDescriptor, swapConfig.GetVariableNameIncludeList());

            return new SwapMoveSelector(leftEntitySelector, rightEntitySelector, variableDescriptorList,
                    randomSelection);
        }

        protected List<GenuineVariableDescriptor> DeduceVariableDescriptorList(EntityDescriptor entityDescriptor, List<string> variableNameIncludeList)
        {
            List<GenuineVariableDescriptor> variableDescriptorList = entityDescriptor.GetGenuineVariableDescriptorList();
            if (variableNameIncludeList == null)
            {
                return variableDescriptorList;
            }

            return variableNameIncludeList
           .Select(variableNameInclude => variableDescriptorList
               .FirstOrDefault(variableDescriptor => variableDescriptor.GetVariableName() == variableNameInclude))
           .Where(variableDescriptor => variableDescriptor != null)// ?.FirstOrDefault()
           .ToList();
        }

        protected override AbstractMoveSelectorConfig BuildUnfoldedMoveSelectorConfig(HeuristicConfigPolicy configPolicy)
        {
            EntityDescriptor onlyEntityDescriptor = swapConfig.GetEntitySelectorConfig() == null ? null
                : EntitySelectorFactory.Create(swapConfig.GetEntitySelectorConfig())
                        .ExtractEntityDescriptor(configPolicy);
            if (swapConfig.GetSecondaryEntitySelectorConfig() != null)
            {
                EntityDescriptor onlySecondaryEntityDescriptor =
                        EntitySelectorFactory.Create(swapConfig.GetSecondaryEntitySelectorConfig())
                                .ExtractEntityDescriptor(configPolicy);
                if (onlyEntityDescriptor != onlySecondaryEntityDescriptor)
                {
                    throw new Exception("The entitySelector (" + swapConfig.GetEntitySelectorConfig()
                            + ")'s entityClass (" + (onlyEntityDescriptor == null ? null : onlyEntityDescriptor.EntityClass)
                            + ") and secondaryEntitySelectorConfig (" + swapConfig.GetSecondaryEntitySelectorConfig()
                            + ")'s entityClass ("
                            + (onlySecondaryEntityDescriptor == null ? null : onlySecondaryEntityDescriptor.EntityClass)
                            + ") must be the same entity class.");
                }
            }
            if (onlyEntityDescriptor != null)
            {
                List<GenuineVariableDescriptor> variableDescriptorList =
                        onlyEntityDescriptor.GetGenuineVariableDescriptorList();
                // If there is a single list variable, unfold to list swap move selector config.
                if (variableDescriptorList.Count == 1 && variableDescriptorList[0].IsListVariable())
                {
                    return BuildListSwapMoveSelectorConfig(variableDescriptorList[0], true);
                }
                // No need for unfolding or deducing
                return null;
            }
            List<EntityDescriptor> entityDescriptors =
                    configPolicy.BuilderInfo.SolutionDescriptor.GetGenuineEntityDescriptors();
            return BuildUnfoldedMoveSelectorConfig(entityDescriptors);
        }

        protected AbstractMoveSelectorConfig BuildUnfoldedMoveSelectorConfig(List<EntityDescriptor> entityDescriptors)
        {
            List<AbstractMoveSelectorConfig> moveSelectorConfigList = new List<AbstractMoveSelectorConfig>(entityDescriptors.Count);

            List<GenuineVariableDescriptor> variableDescriptorList = entityDescriptors.First().GetGenuineVariableDescriptorList();

            // Only unfold into list swap move selector for the basic scenario with 1 entity and 1 list variable.
            if (entityDescriptors.Count == 1 && variableDescriptorList.Count == 1 && variableDescriptorList[0].IsListVariable())
            {
                // No childMoveSelectorConfig.inherit() because of unfoldedMoveSelectorConfig.inheritFolded()
                var childMoveSelectorConfig = BuildListSwapMoveSelectorConfig(variableDescriptorList[0], false);
                moveSelectorConfigList.Add(childMoveSelectorConfig);
            }
            else
            {
                // More complex scenarios do not support unfolding into list swap => fail fast if there is any list variable.
                foreach (var entityDescriptor in entityDescriptors)
                {

                    // No childMoveSelectorConfig.inherit() because of unfoldedMoveSelectorConfig.inheritFolded()
                    SwapMoveSelectorConfig childMoveSelectorConfig = new SwapMoveSelectorConfig();
                    EntitySelectorConfig childEntitySelectorConfig = new EntitySelectorConfig(swapConfig.GetEntitySelectorConfig());
                    if (childEntitySelectorConfig.GetMimicSelectorRef() == null)
                    {
                        childEntitySelectorConfig.SetEntityClass(entityDescriptor.EntityClass);
                    }
                    childMoveSelectorConfig.SetEntitySelectorConfig(childEntitySelectorConfig);
                    if (swapConfig.GetSecondaryEntitySelectorConfig() != null)
                    {
                        EntitySelectorConfig childSecondaryEntitySelectorConfig =
                                new EntitySelectorConfig(swapConfig.GetSecondaryEntitySelectorConfig());
                        if (childSecondaryEntitySelectorConfig.GetMimicSelectorRef() == null)
                        {
                            childSecondaryEntitySelectorConfig.SetEntityClass(entityDescriptor.EntityClass);
                        }
                        childMoveSelectorConfig.SetSecondaryEntitySelectorConfig(childSecondaryEntitySelectorConfig);
                    }
                    childMoveSelectorConfig.SetVariableNameIncludeList(swapConfig.GetVariableNameIncludeList());
                    moveSelectorConfigList.Add(childMoveSelectorConfig);
                }
            }

            AbstractMoveSelectorConfig unfoldedMoveSelectorConfig;
            if (moveSelectorConfigList.Count == 1)
            {
                unfoldedMoveSelectorConfig = moveSelectorConfigList[0];
            }
            else
            {
                unfoldedMoveSelectorConfig = new UnionMoveSelectorConfig(moveSelectorConfigList);
            }
            unfoldedMoveSelectorConfig.MoveSelectorConfigImpl.InheritFolded<AbstractMoveSelectorConfig>(swapConfig);
            return unfoldedMoveSelectorConfig;
        }

        private AbstractMoveSelectorConfig BuildListSwapMoveSelectorConfig(VariableDescriptor variableDescriptor, bool inheritFoldedConfig)
        {
            throw new NotImplementedException();
            /*ListSwapMoveSelectorConfig listSwapMoveSelectorConfig = new ListSwapMoveSelectorConfig();
            ValueSelectorConfig childValueSelectorConfig = new ValueSelectorConfig(
                    new ValueSelectorConfig(variableDescriptor.GetVariableName()));
            listSwapMoveSelectorConfig.SetValueSelectorConfig(childValueSelectorConfig);
            if (inheritFoldedConfig)
            {
                listSwapMoveSelectorConfig.InheritFolded(swapConfig);
            }
            return listSwapMoveSelectorConfig;*/
        }


        public SwapMoveSelectorFactory(SwapMoveSelectorConfig moveSelectorConfig)
                : base(moveSelectorConfig)
        {
        }
    }
}
