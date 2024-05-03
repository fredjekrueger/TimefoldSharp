using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move;
using EntitySelectorConfig = TimefoldSharp.Core.Config.Heuristics.Selector.Entity.EntitySelectorConfig;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public class QueuedEntityPlacerFactory : AbstractEntityPlacerFactory<QueuedEntityPlacerConfig>
    {
        public QueuedEntityPlacerFactory(QueuedEntityPlacerConfig placerConfig) : base(placerConfig)
        {
        }

        public static QueuedEntityPlacerConfig UnfoldNew(HeuristicConfigPolicy configPolicy, List<AbstractMoveSelectorConfig> templateMoveSelectorConfigList)
        {
            throw new NotImplementedException();
        }

        public override EntityPlacer BuildEntityPlacer(HeuristicConfigPolicy configPolicy)
        {
            EntitySelectorConfig entitySelectorConfig_ = BuildEntitySelectorConfig(configPolicy);
            EntitySelector entitySelector = EntitySelectorFactory.Create(entitySelectorConfig_)
                            .BuildEntitySelector(configPolicy, SelectionCacheType.PHASE, SelectionOrder.ORIGINAL);

            List<AbstractMoveSelectorConfig> moveSelectorConfigList_;
            if (ConfigUtils.IsEmptyCollection(config.GetMoveSelectorConfigList()))
            {
                EntityDescriptor entityDescriptor = entitySelector.GetEntityDescriptor();
                List<GenuineVariableDescriptor> variableDescriptorList =
                        entityDescriptor.GetGenuineVariableDescriptorList();
                List<AbstractMoveSelectorConfig> subMoveSelectorConfigList = new List<AbstractMoveSelectorConfig>(variableDescriptorList.Count);
                foreach (var variableDescriptor in variableDescriptorList)
                {
                    subMoveSelectorConfigList.Add(BuildChangeMoveSelectorConfig(configPolicy, entitySelectorConfig_.GetId(), variableDescriptor));
                }
                AbstractMoveSelectorConfig subMoveSelectorConfig;
                if (subMoveSelectorConfigList.Count > 1)
                {
                    // Default to cartesian product (not a queue) of planning variables.
                    subMoveSelectorConfig = new CartesianProductMoveSelectorConfig(subMoveSelectorConfigList);
                }
                else
                {
                    subMoveSelectorConfig = subMoveSelectorConfigList[0];
                }
                moveSelectorConfigList_ = new List<AbstractMoveSelectorConfig>() { subMoveSelectorConfig };
            }
            else
            {
                moveSelectorConfigList_ = config.GetMoveSelectorConfigList();
            }
            List<MoveSelector> moveSelectorList = new List<MoveSelector>(moveSelectorConfigList_.Count);
            foreach (var moveSelectorConfig in moveSelectorConfigList_)
            {
                MoveSelector moveSelector = AbstractMoveSelectorFactory<AbstractMoveSelectorConfig>.Create(moveSelectorConfig)
                        .BuildMoveSelector(configPolicy, SelectionCacheType.JUST_IN_TIME, SelectionOrder.ORIGINAL, false);
                moveSelectorList.Add(moveSelector);
            }
            return new QueuedEntityPlacer(entitySelector, moveSelectorList);
        }

        public EntitySelectorConfig BuildEntitySelectorConfig(HeuristicConfigPolicy configPolicy)
        {
            EntitySelectorConfig entitySelectorConfig_;
            if (config.GetEntitySelectorConfig() == null)
            {
                EntityDescriptor entityDescriptor = GetTheOnlyEntityDescriptor(configPolicy.BuilderInfo.SolutionDescriptor);
                entitySelectorConfig_ = GetDefaultEntitySelectorConfigForEntity(configPolicy, entityDescriptor);
            }
            else
            {
                entitySelectorConfig_ = config.GetEntitySelectorConfig();
            }

            return entitySelectorConfig_;
        }
    }
}

