using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class ChangeMoveSelectorFactory : AbstractMoveSelectorFactory<AbstractMoveSelectorConfig>
    {

        public ChangeMoveSelectorConfig moveConfig => (ChangeMoveSelectorConfig)config;

        public ChangeMoveSelectorFactory(ChangeMoveSelectorConfig moveSelectorConfig)
            : base(moveSelectorConfig)
        {

        }

        protected override MoveSelector BuildBaseMoveSelector(HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, bool randomSelection)
        {
            SelectionOrder selectionOrder = SelectionOrderHelper.FromRandomSelectionBoolean(randomSelection);
            EntitySelector entitySelector = EntitySelectorFactory.Create(moveConfig.GetEntitySelectorConfig())
                    .BuildEntitySelector(configPolicy, minimumCacheType, selectionOrder);
            ValueSelector valueSelector = ValueSelectorFactory.Create(moveConfig.GetValueSelectorConfig())
                    .BuildValueSelector(configPolicy, entitySelector.GetEntityDescriptor(), minimumCacheType, selectionOrder);
            return new ChangeMoveSelector(entitySelector, valueSelector, randomSelection);
        }

        protected override AbstractMoveSelectorConfig BuildUnfoldedMoveSelectorConfig(HeuristicConfigPolicy configPolicy)
        {
            List<EntityDescriptor> entityDescriptors;
            EntityDescriptor onlyEntityDescriptor = moveConfig.GetEntitySelectorConfig() == null ? null
                    : EntitySelectorFactory.Create(moveConfig.GetEntitySelectorConfig())
                            .ExtractEntityDescriptor(configPolicy);
            if (onlyEntityDescriptor != null)
            {
                entityDescriptors = new List<EntityDescriptor>() { onlyEntityDescriptor };
            }
            else
            {
                entityDescriptors = configPolicy.BuilderInfo.SolutionDescriptor.GetGenuineEntityDescriptors();
            }
            List<GenuineVariableDescriptor> variableDescriptorList = new List<GenuineVariableDescriptor>();
            foreach (var entityDescriptor in entityDescriptors)
            {
                GenuineVariableDescriptor onlyVariableDescriptor = moveConfig.GetValueSelectorConfig() == null ? null
                        : ValueSelectorFactory.Create(moveConfig.GetValueSelectorConfig())
                                .ExtractVariableDescriptor(configPolicy, entityDescriptor);
                if (onlyVariableDescriptor != null)
                {
                    if (onlyEntityDescriptor != null)
                    {
                        if (onlyVariableDescriptor.IsListVariable())
                        {
                            return BuildListChangeMoveSelectorConfig((ListVariableDescriptor)onlyVariableDescriptor, true);
                        }
                        // No need for unfolding or deducing
                        return null;
                    }
                    variableDescriptorList.Add(onlyVariableDescriptor);
                }
                else
                {
                    variableDescriptorList.AddRange(entityDescriptor.GetGenuineVariableDescriptorList());
                }
            }
            return BuildUnfoldedMoveSelectorConfig(variableDescriptorList);
        }


        protected AbstractMoveSelectorConfig BuildUnfoldedMoveSelectorConfig(List<GenuineVariableDescriptor> variableDescriptorList)
        {
            List<AbstractMoveSelectorConfig> moveSelectorConfigList = new List<AbstractMoveSelectorConfig>(variableDescriptorList.Count);
            foreach (var variableDescriptor in variableDescriptorList)
            {
                if (variableDescriptor.IsListVariable())
                {
                    // No childMoveSelectorConfig.inherit() because of unfoldedMoveSelectorConfig.inheritFolded()
                    AbstractMoveSelectorConfig childMoveSelectorConfig = BuildListChangeMoveSelectorConfig((ListVariableDescriptor)variableDescriptor, false);
                    moveSelectorConfigList.Add(childMoveSelectorConfig);
                }
                else
                {
                    // No childMoveSelectorConfig.inherit() because of unfoldedMoveSelectorConfig.inheritFolded()
                    ChangeMoveSelectorConfig childMoveSelectorConfig = new ChangeMoveSelectorConfig();
                    // Different EntitySelector per child because it is a union
                    EntitySelectorConfig childEntitySelectorConfig = new EntitySelectorConfig(moveConfig.GetEntitySelectorConfig());
                    if (childEntitySelectorConfig.GetMimicSelectorRef() == null)
                    {
                        childEntitySelectorConfig.SetEntityClass(variableDescriptor.EntityDescriptor.EntityClass);
                    }
                    childMoveSelectorConfig.SetEntitySelectorConfig(childEntitySelectorConfig);
                    ValueSelectorConfig childValueSelectorConfig = new ValueSelectorConfig(moveConfig.GetValueSelectorConfig());
                    if (childValueSelectorConfig.GetMimicSelectorRef() == null)
                    {
                        childValueSelectorConfig.SetVariableName(variableDescriptor.GetVariableName());
                    }
                    childMoveSelectorConfig.SetValueSelectorConfig(childValueSelectorConfig);
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
            unfoldedMoveSelectorConfig.InheritFolded(config);
            return unfoldedMoveSelectorConfig;
        }

        private AbstractMoveSelectorConfig BuildListChangeMoveSelectorConfig(ListVariableDescriptor variableDescriptor, bool inheritFoldedConfig)
        {
            throw new NotImplementedException();
            /*ListChangeMoveSelectorConfig listChangeMoveSelectorConfig = ListChangeMoveSelectorFactory.BuildChildMoveSelectorConfig(
                    variableDescriptor, config.getValueSelectorConfig(),
                    new DestinationSelectorConfig()
                            .withEntitySelectorConfig(config.getEntitySelectorConfig())
                            .withValueSelectorConfig(config.getValueSelectorConfig()));
            if (inheritFoldedConfig)
            {
                listChangeMoveSelectorConfig.inheritFolded(config);
            }
            return listChangeMoveSelectorConfig;*/
        }
    }
}
