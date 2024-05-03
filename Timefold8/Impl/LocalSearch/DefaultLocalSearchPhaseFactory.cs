using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic;
using TimefoldSharp.Core.Config.LocalSearch;
using TimefoldSharp.Core.Config.LocalSearch.Decider.Acceptor;
using TimefoldSharp.Core.Config.LocalSearch.Decider.Forager;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite;
using TimefoldSharp.Core.Impl.LocalSearch.Decider;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager;
using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Solver.Recaller;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.LocalSearch
{
    public class DefaultLocalSearchPhaseFactory : AbstractPhaseFactory<AbstractPhaseConfig>
    {

        LocalSearchPhaseConfig pConfig => (LocalSearchPhaseConfig)phaseConfig;

        public DefaultLocalSearchPhaseFactory(LocalSearchPhaseConfig phaseConfig)
            : base(phaseConfig)
        {

        }

        public override Phase.Phase BuildPhase(int phaseIndex, HeuristicConfigPolicy solverConfigPolicy, BestSolutionRecaller bestSolutionRecaller, Termination solverTermination)
        {
            HeuristicConfigPolicy phaseConfigPolicy = solverConfigPolicy.CreatePhaseConfigPolicy();
            Termination phaseTermination = BuildPhaseTermination(phaseConfigPolicy, solverTermination);
            DefaultLocalSearchPhase.Builder builder = new DefaultLocalSearchPhase.Builder(phaseIndex, solverConfigPolicy.GetLogIndentation(), phaseTermination,
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

        private LocalSearchDecider BuildDecider(HeuristicConfigPolicy configPolicy,
           Termination termination)
        {
            MoveSelector moveSelector = BuildMoveSelector(configPolicy);
            Acceptor acceptor = BuildAcceptor(configPolicy);
            LocalSearchForager forager = BuildForager(configPolicy);
            if (moveSelector.IsNeverEnding() && !forager.SupportsNeverEndingMoveSelector())
            {
                throw new Exception("The moveSelector (" + moveSelector
                        + ") has neverEnding (" + moveSelector.IsNeverEnding()
                        + "), but the forager (" + forager
                        + ") does not support it.\n"
                        + "Maybe configure the <forager> with an <acceptedCountLimit>.");
            }
            int? moveThreadCount = configPolicy.BuilderInfo.MoveThreadCount;
            EnvironmentMode environmentMode = configPolicy.BuilderInfo.EnvironmentMode;
            LocalSearchDecider decider;
            if (moveThreadCount == null)
            {
                decider = new LocalSearchDecider(configPolicy.BuilderInfo.LogIndentation, termination, moveSelector, acceptor, forager);
            }
            else
            {
                throw new NotImplementedException();
                //decider = MultithreadedSolvingEnterpriseService.load(moveThreadCount)
                //.buildLocalSearch(moveThreadCount, termination, moveSelector, acceptor, forager, environmentMode,
                //      configPolicy);
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

        protected LocalSearchForager BuildForager(HeuristicConfigPolicy configPolicy)
        {
            LocalSearchForagerConfig foragerConfig_;
            if (pConfig.GetForagerConfig() != null)
            {
                if (pConfig.GetLocalSearchType() != null)
                {
                    throw new Exception("The localSearchType (" + pConfig.GetLocalSearchType()
                            + ") must not be configured if the foragerConfig (" + pConfig.GetForagerConfig()
                            + ") is explicitly configured.");
                }
                foragerConfig_ = pConfig.GetForagerConfig();
            }
            else
            {
                LocalSearchType localSearchType_ = pConfig.GetLocalSearchType() ?? LocalSearchType.LATE_ACCEPTANCE;
                foragerConfig_ = new LocalSearchForagerConfig();
                switch (localSearchType_)
                {
                    case LocalSearchType.HILL_CLIMBING:
                        foragerConfig_.SetAcceptedCountLimit(1);
                        break;
                    case LocalSearchType.TABU_SEARCH:
                        // Slow stepping algorithm
                        foragerConfig_.SetAcceptedCountLimit(1000);
                        break;
                    case LocalSearchType.SIMULATED_ANNEALING:
                    case LocalSearchType.LATE_ACCEPTANCE:
                    case LocalSearchType.GREAT_DELUGE:
                        // Fast stepping algorithm
                        foragerConfig_.SetAcceptedCountLimit(1);
                        break;
                    case LocalSearchType.VARIABLE_NEIGHBORHOOD_DESCENT:
                        foragerConfig_.SetPickEarlyType(LocalSearchPickEarlyType.FIRST_LAST_STEP_SCORE_IMPROVING);
                        break;
                    default:
                        throw new Exception("The localSearchType (" + localSearchType_
                                + ") is not implemented.");
                }
            }
            return LocalSearchForagerFactory.Create(foragerConfig_).BuildForager();
        }

        protected Acceptor BuildAcceptor(HeuristicConfigPolicy configPolicy)
        {
            LocalSearchAcceptorConfig acceptorConfig_;
            if (pConfig.GetAcceptorConfig() != null)
            {
                if (pConfig.GetLocalSearchType() != null)
                {
                    throw new Exception("The localSearchType (" + pConfig.GetLocalSearchType()
                            + ") must not be configured if the acceptorConfig (" + pConfig.GetAcceptorConfig()
                            + ") is explicitly configured.");
                }
                acceptorConfig_ = pConfig.GetAcceptorConfig();
            }
            else
            {
                LocalSearchType localSearchType_ = pConfig.GetLocalSearchType() ?? LocalSearchType.LATE_ACCEPTANCE;
                acceptorConfig_ = new LocalSearchAcceptorConfig();
                switch (localSearchType_)
                {
                    case LocalSearchType.HILL_CLIMBING:
                    case LocalSearchType.VARIABLE_NEIGHBORHOOD_DESCENT:
                        acceptorConfig_.SetAcceptorTypeList(new List<AcceptorType>() { AcceptorType.HILL_CLIMBING });
                        break;
                    case LocalSearchType.TABU_SEARCH:
                        acceptorConfig_.SetAcceptorTypeList(new List<AcceptorType>() { AcceptorType.ENTITY_TABU });
                        break;
                    case LocalSearchType.SIMULATED_ANNEALING:
                        acceptorConfig_.SetAcceptorTypeList(new List<AcceptorType>() { AcceptorType.SIMULATED_ANNEALING });
                        break;
                    case LocalSearchType.LATE_ACCEPTANCE:
                        acceptorConfig_.SetAcceptorTypeList(new List<AcceptorType>() { AcceptorType.LATE_ACCEPTANCE });
                        break;
                    case LocalSearchType.GREAT_DELUGE:
                        acceptorConfig_.SetAcceptorTypeList(new List<AcceptorType>() { AcceptorType.GREAT_DELUGE });
                        break;
                    default:
                        throw new Exception("The localSearchType (" + localSearchType_
                                + ") is not implemented.");
                }
            }
            return AcceptorFactory.Create(acceptorConfig_).BuildAcceptor(configPolicy);
        }

        protected MoveSelector BuildMoveSelector(HeuristicConfigPolicy configPolicy)
        {
            MoveSelector moveSelector;
            SelectionCacheType defaultCacheType = SelectionCacheType.JUST_IN_TIME;
            SelectionOrder defaultSelectionOrder;
            if (pConfig.GetLocalSearchType() == LocalSearchType.VARIABLE_NEIGHBORHOOD_DESCENT)
            {
                defaultSelectionOrder = SelectionOrder.ORIGINAL;
            }
            else
            {
                defaultSelectionOrder = SelectionOrder.RANDOM;
            }
            if (pConfig.GetMoveSelectorConfig() == null)
            {
                moveSelector = new UnionMoveSelectorFactory(DetermineDefaultMoveSelectorConfig(configPolicy))
                        .BuildMoveSelector(configPolicy, defaultCacheType, defaultSelectionOrder, true);
            }
            else
            {
                throw new NotImplementedException();
                /*    moveSelector = MoveSelectorFactory.Create(pConfig.GetMoveSelectorConfig())
                            .buildMoveSelector(configPolicy, defaultCacheType, defaultSelectionOrder, true);*/
            }
            return moveSelector;
        }

        private UnionMoveSelectorConfig DetermineDefaultMoveSelectorConfig(HeuristicConfigPolicy configPolicy)
        {
            SolutionDescriptor solutionDescriptor = configPolicy.BuilderInfo.SolutionDescriptor;
            var basicVariableDescriptors = solutionDescriptor.GetEntityDescriptors().SelectMany(entityDescriptor => entityDescriptor.GetGenuineVariableDescriptorList())
    .Where(variableDescriptor => !variableDescriptor.IsListVariable()).Distinct().ToList();
            List<ListVariableDescriptor> listVariableDescriptors = solutionDescriptor.ListVariableDescriptors;
            if (basicVariableDescriptors.Count == 0)
            { // We only have list variables.
                throw new NotImplementedException();
                //return new UnionMoveSelectorConfig().WithMoveSelectors(new ListChangeMoveSelectorConfig(), new ListSwapMoveSelectorConfig());
            }
            else if (listVariableDescriptors.Count == 0)
            { // We only have basic variables.
                return new UnionMoveSelectorConfig().WithMoveSelectors(new List<AbstractMoveSelectorConfig>() { new ChangeMoveSelectorConfig(), new SwapMoveSelectorConfig() });
            }
            else
            {
                throw new NotImplementedException();
                //return new UnionMoveSelectorConfig().WithMoveSelectors(new ChangeMoveSelectorConfig(), new SwapMoveSelectorConfig());
            }
        }

    }
}
