using TimefoldSharp.Core.Config.ConstructHeuristic;
using TimefoldSharp.Core.Config.ConstructHeuristic.Decider.Forager;
using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic.List;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.decider;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.decider.Forager;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic;
using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Solver.Recaller;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic
{
    public class DefaultConstructionHeuristicPhaseFactory : AbstractPhaseFactory<AbstractPhaseConfig>
    {

        ConstructionHeuristicPhaseConfig heuristicConfig => (ConstructionHeuristicPhaseConfig)phaseConfig;

        ConstructionHeuristicPhaseConfig constructionHeuristicPhaseConfig => (ConstructionHeuristicPhaseConfig)phaseConfig;
        public DefaultConstructionHeuristicPhaseFactory(ConstructionHeuristicPhaseConfig phaseConfig)
             : base(phaseConfig)
        {

        }

        public static IAbstractEntityPlacerConfig BuildListVariableQueuedValuePlacerConfig(HeuristicConfigPolicy configPolicy, ListVariableDescriptor variableDescriptor)
        {
            string mimicSelectorId = variableDescriptor.GetVariableName();

            // Prepare recording ValueSelector config.
            ValueSelectorConfig mimicRecordingValueSelectorConfig = new ValueSelectorConfig(variableDescriptor.GetVariableName())
                    .WithId(mimicSelectorId);
            if (ValueSelectorConfig.HasSorter(configPolicy.GetValueSorterManner(), variableDescriptor))
            {
                mimicRecordingValueSelectorConfig = mimicRecordingValueSelectorConfig.WithCacheType(SelectionCacheType.PHASE)
                        .WithSelectionOrder(SelectionOrder.SORTED)
                        .WithSorterManner(configPolicy.GetValueSorterManner());
            }
            // Prepare replaying ValueSelector config.
            ValueSelectorConfig mimicReplayingValueSelectorConfig = new ValueSelectorConfig()
                    .WithMimicSelectorRef(mimicSelectorId);

            // ListChangeMoveSelector uses the replaying ValueSelector.
            ListChangeMoveSelectorConfig listChangeMoveSelectorConfig = new ListChangeMoveSelectorConfig()
                    .WithValueSelectorConfig(mimicReplayingValueSelectorConfig);

            // Finally, QueuedValuePlacer uses the recording ValueSelector and a ListChangeMoveSelector.
            // The ListChangeMoveSelector's replaying ValueSelector mimics the QueuedValuePlacer's recording ValueSelector.
            return new QueuedValuePlacerConfig()
                    .WithValueSelectorConfig(mimicRecordingValueSelectorConfig)
                    .WithMoveSelectorConfig(listChangeMoveSelectorConfig);
        }

        public override Phase.Phase BuildPhase(int phaseIndex, HeuristicConfigPolicy solverConfigPolicy, BestSolutionRecaller bestSolutionRecaller, Termination solverTermination)
        {
            ConstructionHeuristicType constructionHeuristicType_ = constructionHeuristicPhaseConfig.GetConstructionHeuristicType() ?? ConstructionHeuristicType.ALLOCATE_ENTITY_FROM_QUEUE;
            EntitySorterManner entitySorterManner = constructionHeuristicPhaseConfig.GetEntitySorterManner() ?? ConstructionHeuristicTypeHelper.GetDefaultEntitySorterManner(constructionHeuristicType_);
            ValueSorterManner valueSorterManner = constructionHeuristicPhaseConfig.GetValueSorterManner() ?? ConstructionHeuristicTypeHelper.GetDefaultValueSorterManner(constructionHeuristicType_);
            HeuristicConfigPolicy phaseConfigPolicy = solverConfigPolicy.CloneBuilder()
                    .WithReinitializeVariableFilterEnabled(true)
                    .WithInitializedChainedValueFilterEnabled(true)
                    .WithUnassignedValuesAllowed(true)
                    .WithEntitySorterManner(entitySorterManner)
                    .WithValueSorterManner(valueSorterManner)
                    .Build();
            Termination phaseTermination = BuildPhaseTermination(phaseConfigPolicy, solverTermination);
            EntityPlacerConfig<IAbstractEntityPlacerConfig> entityPlacerConfig_ = GetValidEntityPlacerConfig() ?? BuildDefaultEntityPlacerConfig(phaseConfigPolicy, constructionHeuristicType_);

            EntityPlacer entityPlacer = EntityPlacerFactoryHelper.Create(entityPlacerConfig_).BuildEntityPlacer(phaseConfigPolicy);

            DefaultConstructionHeuristicPhase.Builder builder = new DefaultConstructionHeuristicPhase.Builder(
                    phaseIndex,
                    solverConfigPolicy.GetLogIndentation(),
                    phaseTermination,
                    entityPlacer,
                    BuildDecider(phaseConfigPolicy, phaseTermination));

            EnvironmentMode environmentMode = phaseConfigPolicy.BuilderInfo.EnvironmentMode;
            if (EnvironmentModeEnumHelper.IsNonIntrusiveFullAsserted(environmentMode))
            {
                builder.SetAssertStepScoreFromScratch(true);
            }
            if (EnvironmentModeEnumHelper.IsIntrusiveFastAsserted(environmentMode))
            {
                builder.SetAssertExpectedStepScore(true);
                builder.SetAssertShadowVariablesAreNotStaleAfterStep(true);
            }
            return builder.Build();
        }

        private ConstructionHeuristicDecider BuildDecider(HeuristicConfigPolicy configPolicy, Termination termination)
        {
            ConstructionHeuristicForagerConfig foragerConfig_ = heuristicConfig.GetForagerConfig() ?? new ConstructionHeuristicForagerConfig();
            ConstructionHeuristicForager forager =
                    ConstructionHeuristicForagerFactory.Create(foragerConfig_).BuildForager(configPolicy);
            EnvironmentMode environmentMode = configPolicy.BuilderInfo.EnvironmentMode;
            ConstructionHeuristicDecider decider;
            int? moveThreadCount = configPolicy.BuilderInfo.MoveThreadCount;
            if (moveThreadCount == null)
            {
                decider = new ConstructionHeuristicDecider(configPolicy.GetLogIndentation(), termination, forager);
            }
            else
            {
                throw new NotImplementedException();
                /*decider = MultithreadedSolvingEnterpriseService.load(moveThreadCount)
                        .BuildConstructionHeuristic(moveThreadCount, termination, forager, environmentMode, configPolicy);*/
            }
            if (EnvironmentModeEnumHelper.IsNonIntrusiveFullAsserted(environmentMode))
            {
                decider.SetAssertMoveScoreFromScratch(true);
            }
            if (EnvironmentModeEnumHelper.IsIntrusiveFastAsserted(environmentMode))
            {
                decider.SetAssertExpectedUndoMoveScore(true);
            }
            return decider;
        }

        private EntityPlacerConfig<IAbstractEntityPlacerConfig> BuildDefaultEntityPlacerConfig(HeuristicConfigPolicy configPolicy,
           ConstructionHeuristicType constructionHeuristicType)
        {
            var listVariableDescriptor = FindValidListVariableDescriptor(configPolicy.BuilderInfo.SolutionDescriptor);
            var config = BuildListVariableQueuedValuePlacerConfig(configPolicy, listVariableDescriptor);
            if (config != null)
                return config;
            else
            {
                return BuildUnfoldedEntityPlacerConfig(configPolicy, constructionHeuristicType);
            }
        }

        private EntityPlacerConfig<IAbstractEntityPlacerConfig> BuildUnfoldedEntityPlacerConfig(HeuristicConfigPolicy phaseConfigPolicy,
            ConstructionHeuristicType constructionHeuristicType)
        {
            switch (constructionHeuristicType)
            {
                case ConstructionHeuristicType.FIRST_FIT:
                case ConstructionHeuristicType.FIRST_FIT_DECREASING:
                case ConstructionHeuristicType.WEAKEST_FIT:
                case ConstructionHeuristicType.WEAKEST_FIT_DECREASING:
                case ConstructionHeuristicType.STRONGEST_FIT:
                case ConstructionHeuristicType.STRONGEST_FIT_DECREASING:
                case ConstructionHeuristicType.ALLOCATE_ENTITY_FROM_QUEUE:
                    if (!ConfigUtils.IsEmptyCollection(constructionHeuristicPhaseConfig.GetMoveSelectorConfigList()))
                    {
                        return QueuedEntityPlacerFactory.UnfoldNew(phaseConfigPolicy, constructionHeuristicPhaseConfig.GetMoveSelectorConfigList());
                    }
                    return new QueuedEntityPlacerConfig();
                case ConstructionHeuristicType.ALLOCATE_TO_VALUE_FROM_QUEUE:
                    if (!ConfigUtils.IsEmptyCollection(constructionHeuristicPhaseConfig.GetMoveSelectorConfigList()))
                    {
                        return QueuedValuePlacerFactory.UnfoldNew(CheckSingleMoveSelectorConfig());
                    }
                    return new QueuedValuePlacerConfig();
                case ConstructionHeuristicType.CHEAPEST_INSERTION:
                case ConstructionHeuristicType.ALLOCATE_FROM_POOL:
                    if (!ConfigUtils.IsEmptyCollection(constructionHeuristicPhaseConfig.GetMoveSelectorConfigList()))
                    {
                        return PooledEntityPlacerFactory.UnfoldNew(phaseConfigPolicy, CheckSingleMoveSelectorConfig());
                    }
                    return new PooledEntityPlacerConfig();
                default:
                    throw new Exception(
                            "The constructionHeuristicType (" + constructionHeuristicType + ") is not implemented.");
            }
        }

        private ListVariableDescriptor FindValidListVariableDescriptor(SolutionDescriptor solutionDescriptor)
        {
            List<ListVariableDescriptor> listVariableDescriptors = solutionDescriptor.ListVariableDescriptors;
            if (listVariableDescriptors.Count == 0)
            {
                return null;
            }
            if (listVariableDescriptors.Count > 1)
            {
                throw new Exception("Construction Heuristic phase does not support multiple list variables ("
                        + listVariableDescriptors + ").");
            }


            return listVariableDescriptors[0];
        }

        private EntityPlacerConfig<IAbstractEntityPlacerConfig> GetValidEntityPlacerConfig()
        {
            EntityPlacerConfig<IAbstractEntityPlacerConfig> entityPlacerConfig = constructionHeuristicPhaseConfig.GetEntityPlacerConfig();
            if (entityPlacerConfig == null)
            {
                return null;
            }
            if (constructionHeuristicPhaseConfig.GetConstructionHeuristicType() != null)
            {
                throw new Exception(
                        "The constructionHeuristicType (" + constructionHeuristicPhaseConfig.GetConstructionHeuristicType()
                                + ") must not be configured if the entityPlacerConfig (" + entityPlacerConfig
                                + ") is explicitly configured.");
            }
            if (constructionHeuristicPhaseConfig.GetMoveSelectorConfigList() != null)
            {
                throw new Exception("The moveSelectorConfigList (" + constructionHeuristicPhaseConfig.GetMoveSelectorConfigList()
                        + ") cannot be configured if the entityPlacerConfig (" + entityPlacerConfig
                        + ") is explicitly configured.");
            }
            return entityPlacerConfig;
        }

        private MoveSelectorConfig<AbstractMoveSelectorConfig> CheckSingleMoveSelectorConfig()
        {
            if (constructionHeuristicPhaseConfig.GetMoveSelectorConfigList().Count() != 1)
            {
                throw new Exception("For the constructionHeuristicType ( element to nest multiple MoveSelectors.");
            }

            return constructionHeuristicPhaseConfig.GetMoveSelectorConfigList()[0];
        }
    }
}
