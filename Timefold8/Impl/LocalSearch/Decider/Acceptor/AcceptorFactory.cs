using TimefoldSharp.Core.Config.LocalSearch.Decider.Acceptor;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Heurisitic;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.HillClimbing;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.LateAcceptance;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.SimulatedAnnealing;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.StepCountingHillClimbing;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu.Size;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor
{
    public class AcceptorFactory
    {
        private static readonly double DEFAULT_WATER_LEVEL_INCREMENT_RATIO = 0.00_000_005;

        private readonly LocalSearchAcceptorConfig acceptorConfig;

        public AcceptorFactory(LocalSearchAcceptorConfig acceptorConfig)
        {
            this.acceptorConfig = acceptorConfig;
        }

        public static AcceptorFactory Create(LocalSearchAcceptorConfig acceptorConfig)
        {
            return new AcceptorFactory(acceptorConfig);
        }

        public Acceptor BuildAcceptor(HeuristicConfigPolicy configPolicy)
        {

            List<Acceptor> acceptorList = new List<Acceptor>
            {
                BuildHillClimbingAcceptor(),
                BuildStepCountingHillClimbingAcceptor(),
                BuildEntityTabuAcceptor(configPolicy),
                BuildValueTabuAcceptor(configPolicy),
                BuildMoveTabuAcceptor(configPolicy),
                BuildUndoMoveTabuAcceptor(configPolicy),
                BuildSimulatedAnnealingAcceptor(configPolicy),
                BuildLateAcceptanceAcceptor(),
                BuildGreatDelugeAcceptor(configPolicy)
            }.Where(acceptor => acceptor != null).ToList();

            if (acceptorList.Count() == 1)
            {
                return acceptorList[0];
            }
            else if (acceptorList.Count() > 1)
            {
                throw new NotImplementedException();
                //return new CompositeAcceptor<>(acceptorList);
            }
            else
            {
                throw new Exception(
                        "The acceptor does not specify any acceptorType  or other acceptor property.\n"
                                + "For a good starting values,"
                                + " see the docs section \"Which optimization algorithms should I use?\".");
            }
        }

        private HillClimbingAcceptor BuildHillClimbingAcceptor()
        {
            if (acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.HILL_CLIMBING))
            {
                HillClimbingAcceptor acceptor = new HillClimbingAcceptor();
                return acceptor;
            }
            return null;
        }

        private StepCountingHillClimbingAcceptor BuildStepCountingHillClimbingAcceptor()
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.STEP_COUNTING_HILL_CLIMBING))
                    || acceptorConfig.GetStepCountingHillClimbingSize() != null)
            {
                int stepCountingHillClimbingSize_ = acceptorConfig.GetStepCountingHillClimbingSize() ?? 400;
                StepCountingHillClimbingType stepCountingHillClimbingType_ = acceptorConfig.GetStepCountingHillClimbingType() ?? StepCountingHillClimbingType.STEP;
                StepCountingHillClimbingAcceptor acceptor = new StepCountingHillClimbingAcceptor(stepCountingHillClimbingSize_, stepCountingHillClimbingType_);
                return acceptor;
            }
            return null;
        }

        private EntityTabuAcceptor BuildEntityTabuAcceptor(HeuristicConfigPolicy configPolicy)
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.ENTITY_TABU))
                    || acceptorConfig.GetEntityTabuSize() != null || acceptorConfig.GetEntityTabuRatio() != null
                    || acceptorConfig.GetFadingEntityTabuSize() != null || acceptorConfig.GetFadingEntityTabuRatio() != null)
            {
                EntityTabuAcceptor acceptor = new EntityTabuAcceptor(configPolicy.GetLogIndentation());
                if (acceptorConfig.GetEntityTabuSize() != null)
                {
                    if (acceptorConfig.GetEntityTabuRatio() != null)
                    {
                        throw new Exception("The acceptor cannot have both acceptorConfig.getEntityTabuSize() ().");
                    }
                    acceptor.SetTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetEntityTabuSize().Value));
                }
                else if (acceptorConfig.GetEntityTabuRatio() != null)
                {
                    acceptor.SetTabuSizeStrategy(new EntityRatioTabuSizeStrategy(acceptorConfig.GetEntityTabuRatio().Value));
                }
                else if (acceptorConfig.GetFadingEntityTabuSize() == null && acceptorConfig.GetFadingEntityTabuRatio() == null)
                {
                    acceptor.SetTabuSizeStrategy(new EntityRatioTabuSizeStrategy(0.1));
                }
                if (acceptorConfig.GetFadingEntityTabuSize() != null)
                {
                    if (acceptorConfig.GetFadingEntityTabuRatio() != null)
                    {
                        throw new Exception(
                                "The acceptor cannot have both acceptorConfig.getFadingEntityTabuSize() ).");
                    }
                    acceptor.SetFadingTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetFadingEntityTabuSize().Value));
                }
                else if (acceptorConfig.GetFadingEntityTabuRatio() != null)
                {
                    acceptor.SetFadingTabuSizeStrategy(
                            new EntityRatioTabuSizeStrategy(acceptorConfig.GetFadingEntityTabuRatio().Value));
                }
                if (EnvironmentModeEnumHelper.IsNonIntrusiveFullAsserted(configPolicy.GetEnvironmentMode().Value))
                {
                    acceptor.SetAssertTabuHashCodeCorrectness(true);
                }
                return acceptor;
            }
            return null;
        }

        private ValueTabuAcceptor BuildValueTabuAcceptor(HeuristicConfigPolicy configPolicy)
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.VALUE_TABU))
                    || acceptorConfig.GetValueTabuSize() != null || acceptorConfig.GetValueTabuRatio() != null
                    || acceptorConfig.GetFadingValueTabuSize() != null || acceptorConfig.GetFadingValueTabuRatio() != null)
            {
                ValueTabuAcceptor acceptor = new ValueTabuAcceptor(configPolicy.GetLogIndentation());
                if (acceptorConfig.GetValueTabuSize() != null)
                {
                    if (acceptorConfig.GetValueTabuRatio() != null)
                    {
                        throw new Exception("The acceptor cannot have both acceptorConfig.getValueTabuSize() ("
                                + acceptorConfig.GetValueTabuSize() + ") and acceptorConfig.getValueTabuRatio() ("
                                + acceptorConfig.GetValueTabuRatio() + ").");
                    }
                    acceptor.SetTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetValueTabuSize().Value));
                }
                else if (acceptorConfig.GetValueTabuRatio() != null)
                {
                    acceptor.SetTabuSizeStrategy(new ValueRatioTabuSizeStrategy(acceptorConfig.GetValueTabuRatio().Value));
                }
                if (acceptorConfig.GetFadingValueTabuSize() != null)
                {
                    if (acceptorConfig.GetFadingValueTabuRatio() != null)
                    {
                        throw new Exception("The acceptor cannot have both acceptorConfig.getFadingValueTabuSize() ("
                                + acceptorConfig.GetFadingValueTabuSize() + ") and acceptorConfig.getFadingValueTabuRatio() ("
                                + acceptorConfig.GetFadingValueTabuRatio() + ").");
                    }
                    acceptor.SetFadingTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetFadingValueTabuSize().Value));
                }
                else if (acceptorConfig.GetFadingValueTabuRatio() != null)
                {
                    acceptor.SetFadingTabuSizeStrategy(new ValueRatioTabuSizeStrategy(acceptorConfig.GetFadingValueTabuRatio().Value));
                }

                if (acceptorConfig.GetValueTabuSize() != null)
                {
                    acceptor.SetTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetValueTabuSize().Value));
                }
                if (acceptorConfig.GetFadingValueTabuSize() != null)
                {
                    acceptor.SetFadingTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetFadingValueTabuSize().Value));
                }
                if (EnvironmentModeEnumHelper.IsNonIntrusiveFullAsserted(configPolicy.GetEnvironmentMode().Value))
                {
                    acceptor.SetAssertTabuHashCodeCorrectness(true);
                }
                return acceptor;
            }
            return null;
        }

        private MoveTabuAcceptor BuildMoveTabuAcceptor(HeuristicConfigPolicy configPolicy)
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.MOVE_TABU))
                    || acceptorConfig.GetMoveTabuSize() != null || acceptorConfig.GetFadingMoveTabuSize() != null)
            {
                MoveTabuAcceptor acceptor = new MoveTabuAcceptor(configPolicy.GetLogIndentation());
                acceptor.SetUseUndoMoveAsTabuMove(false);
                if (acceptorConfig.GetMoveTabuSize() != null)
                {
                    acceptor.SetTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetMoveTabuSize().Value));
                }
                if (acceptorConfig.GetFadingMoveTabuSize() != null)
                {
                    acceptor.SetFadingTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetFadingMoveTabuSize().Value));
                }
                if (EnvironmentModeEnumHelper.IsNonIntrusiveFullAsserted(configPolicy.GetEnvironmentMode().Value))
                {
                    acceptor.SetAssertTabuHashCodeCorrectness(true);
                }
                return acceptor;
            }
            return null;
        }

        private MoveTabuAcceptor BuildUndoMoveTabuAcceptor(HeuristicConfigPolicy configPolicy)
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.UNDO_MOVE_TABU))
                    || acceptorConfig.GetUndoMoveTabuSize() != null || acceptorConfig.GetFadingUndoMoveTabuSize() != null)
            {
                MoveTabuAcceptor acceptor = new MoveTabuAcceptor(configPolicy.GetLogIndentation());
                acceptor.SetUseUndoMoveAsTabuMove(true);
                if (acceptorConfig.GetUndoMoveTabuSize() != null)
                {
                    acceptor.SetTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetUndoMoveTabuSize().Value));
                }
                if (acceptorConfig.GetFadingUndoMoveTabuSize() != null)
                {
                    acceptor.SetFadingTabuSizeStrategy(new FixedTabuSizeStrategy(acceptorConfig.GetFadingUndoMoveTabuSize().Value));
                }
                if (EnvironmentModeEnumHelper.IsIntrusiveFastAsserted(configPolicy.GetEnvironmentMode().Value))
                {
                    acceptor.SetAssertTabuHashCodeCorrectness(true);
                }
                return acceptor;
            }
            return null;
        }

        private SimulatedAnnealingAcceptor BuildSimulatedAnnealingAcceptor(HeuristicConfigPolicy configPolicy)
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.SIMULATED_ANNEALING))
                    || acceptorConfig.GetSimulatedAnnealingStartingTemperature() != null)
            {
                SimulatedAnnealingAcceptor acceptor = new SimulatedAnnealingAcceptor();
                if (acceptorConfig.GetSimulatedAnnealingStartingTemperature() == null)
                {
                    // TODO Support SA without a parameter
                    throw new Exception("The acceptorType (" + AcceptorType.SIMULATED_ANNEALING
                            + ") currently requires a acceptorConfig.getSimulatedAnnealingStartingTemperature() ("
                            + acceptorConfig.GetSimulatedAnnealingStartingTemperature() + ").");
                }
                acceptor.SetStartingTemperature(
                        configPolicy.GetScoreDefinition().ParseScore(acceptorConfig.GetSimulatedAnnealingStartingTemperature()));
                return acceptor;
            }
            return null;
        }

        private LateAcceptanceAcceptor BuildLateAcceptanceAcceptor()
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.LATE_ACCEPTANCE))
                    || acceptorConfig.GetLateAcceptanceSize() != null)
            {
                LateAcceptanceAcceptor acceptor = new LateAcceptanceAcceptor();
                acceptor.SetLateAcceptanceSize(acceptorConfig.GetLateAcceptanceSize() ?? 400);
                return acceptor;
            }
            return null;
        }

        private GreatDelugeAcceptor BuildGreatDelugeAcceptor(HeuristicConfigPolicy configPolicy)
        {
            if ((acceptorConfig.GetAcceptorTypeList() != null
                    && acceptorConfig.GetAcceptorTypeList().Contains(AcceptorType.GREAT_DELUGE))
                    || acceptorConfig.GetGreatDelugeWaterLevelIncrementScore() != null
                    || acceptorConfig.GetGreatDelugeWaterLevelIncrementRatio() != null)
            {
                GreatDelugeAcceptor acceptor = new GreatDelugeAcceptor();
                if (acceptorConfig.GetGreatDelugeWaterLevelIncrementScore() != null)
                {
                    if (acceptorConfig.GetGreatDelugeWaterLevelIncrementRatio() != null)
                    {
                        throw new Exception("The acceptor cannot have both a "
                                + "acceptorConfig.getGreatDelugeWaterLevelIncrementScore() ("
                                + acceptorConfig.GetGreatDelugeWaterLevelIncrementScore()
                                + ") and a acceptorConfig.getGreatDelugeWaterLevelIncrementRatio() ("
                                + acceptorConfig.GetGreatDelugeWaterLevelIncrementRatio() + ").");
                    }
                    acceptor.SetWaterLevelIncrementScore(
                            configPolicy.GetScoreDefinition().ParseScore(acceptorConfig.GetGreatDelugeWaterLevelIncrementScore()));
                }
                else if (acceptorConfig.GetGreatDelugeWaterLevelIncrementRatio() != null)
                {
                    if (acceptorConfig.GetGreatDelugeWaterLevelIncrementRatio() <= 0.0)
                    {
                        throw new Exception("The acceptorConfig.getGreatDelugeWaterLevelIncrementRatio() ("
                                + acceptorConfig.GetGreatDelugeWaterLevelIncrementRatio()
                                + ") must be positive because the water level should increase.");
                    }
                    acceptor.SetWaterLevelIncrementRatio(acceptorConfig.GetGreatDelugeWaterLevelIncrementRatio());
                }
                else
                {
                    acceptor.SetWaterLevelIncrementRatio(DEFAULT_WATER_LEVEL_INCREMENT_RATIO);
                }
                return acceptor;
            }
            return null;
        }

    }
}
