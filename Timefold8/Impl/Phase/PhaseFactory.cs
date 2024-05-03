using TimefoldSharp.Core.Config.ConstructHeuristic;
using TimefoldSharp.Core.Config.ExhaustiveSearch;
using TimefoldSharp.Core.Config.LocalSearch;
using TimefoldSharp.Core.Config.Phase;
using TimefoldSharp.Core.Config.Phase.Custom;
using TimefoldSharp.Core.Config.Solver.Termination;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.ConstructionHeuristic;
using TimefoldSharp.Core.Impl.Heurisitic;
using TimefoldSharp.Core.Impl.LocalSearch;
using TimefoldSharp.Core.Impl.Solver.Recaller;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.Phase
{
    public abstract class PhaseFactory
    {
        public static List<Phase> BuildPhases(List<AbstractPhaseConfig> phaseConfigList,
            HeuristicConfigPolicy configPolicy, BestSolutionRecaller bestSolutionRecaller,
            Termination termination)
        {
            List<Phase> phaseList = new List<Phase>(phaseConfigList.Count);
            for (int phaseIndex = 0; phaseIndex < phaseConfigList.Count; phaseIndex++)
            {
                AbstractPhaseConfig phaseConfig = phaseConfigList[phaseIndex];
                if (phaseIndex > 0)
                {
                    AbstractPhaseConfig previousPhaseConfig = phaseConfigList[phaseIndex - 1];
                    if (!CanTerminate(previousPhaseConfig))
                    {
                        throw new Exception("Solver configuration contains an unreachable phase. "
                                + "Phase #" + phaseIndex + " (" + phaseConfig + ") follows a phase "
                                + "without a configured termination (" + previousPhaseConfig + ").");
                    }
                }
                PhaseFactory phaseFactory = PhaseFactory.Create(phaseConfig);
                Phase phase = phaseFactory.BuildPhase(phaseIndex, configPolicy, bestSolutionRecaller, termination);
                phaseList.Add(phase);
            }
            return phaseList;
        }

        public abstract Phase BuildPhase(int phaseIndex, HeuristicConfigPolicy solverConfigPolicy,
           BestSolutionRecaller bestSolutionRecaller, Termination solverTermination);

        static bool CanTerminate(PhaseConfig<AbstractPhaseConfig> phaseConfig)
        {
            if (phaseConfig is ConstructionHeuristicPhaseConfig
                || phaseConfig is ExhaustiveSearchPhaseConfig
                || phaseConfig is CustomPhaseConfig)
            { // Termination guaranteed.
                return true;
            }
            TerminationConfig terminationConfig = phaseConfig.GetTerminationConfig();
            return (terminationConfig != null && terminationConfig.IsConfigured());
        }

        static PhaseFactory Create(AbstractPhaseConfig phaseConfig)
        {
            if (typeof(LocalSearchPhaseConfig).IsAssignableFrom(phaseConfig.GetType()))
            {
                return new DefaultLocalSearchPhaseFactory((LocalSearchPhaseConfig)phaseConfig);
            }
            else if (typeof(ConstructionHeuristicPhaseConfig).IsAssignableFrom(phaseConfig.GetType()))
            {
                return new DefaultConstructionHeuristicPhaseFactory((ConstructionHeuristicPhaseConfig)phaseConfig);
            }
            /*else if (typeof(PartitionedSearchPhaseConfig).IsAssignableFrom(phaseConfig.GetType()))
            {
                return new DefaultPartitionedSearchPhaseFactory<>((PartitionedSearchPhaseConfig)phaseConfig);
            }
            else if (typeof(CustomPhaseConfig).IsAssignableFrom(phaseConfig.GetType()))
            {
                return new DefaultCustomPhaseFactory<>((CustomPhaseConfig)phaseConfig);
            }
            else if (typeof(ExhaustiveSearchPhaseConfig).IsAssignableFrom(phaseConfig.GetType()))
            {
                return new DefaultExhaustiveSearchPhaseFactory<>((ExhaustiveSearchPhaseConfig)phaseConfig);
            }
            else if (typeof(NoChangePhaseConfig).IsAssignableFrom(phaseConfig.GetType()))
            {
                return new NoChangePhaseFactory<>((NoChangePhaseConfig)phaseConfig);
            }*/
            else
            {
                throw new Exception("unknown type");
            }
        }
    }
}
