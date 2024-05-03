using TimefoldSharp.Core.API.Solver;
using TimefoldSharp.Core.Config.ConstructHeuristic;
using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Config.LocalSearch;
using TimefoldSharp.Core.Config.Score.Director;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Solver.Monitoring;
using TimefoldSharp.Core.Config.Solver.Random;
using TimefoldSharp.Core.Config.Solver.Termination;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.ConstructionHeuristic;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic;
using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Solver.Change;
using TimefoldSharp.Core.Impl.Solver.Random;
using TimefoldSharp.Core.Impl.Solver.Recaller;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.Solver
{
    //OC
    public class DefaultSolverFactory : SolverFactory
    {
        private readonly SolverConfig solverConfig;
        private readonly SolutionDescriptor solutionDescriptor;
        private readonly InnerScoreDirectorFactory scoreDirectorFactory;

        private static long DEFAULT_RANDOM_SEED = 0L;

        public DefaultSolverFactory(SolverConfig solverConfig)
        {
            this.solverConfig = solverConfig;
            this.solutionDescriptor = BuildSolutionDescriptor();
            // Caching score director factory as it potentially does expensive things.
            this.scoreDirectorFactory = BuildScoreDirectorFactory();
        }

        private SolutionDescriptor BuildSolutionDescriptor()
        {
            if (solverConfig.SolutionClass == null)
            {
                throw new Exception("The solver configuration must have a solutionClass (" +

                        "). If you're using the Quarkus extension or Spring Boot starter, it should have been filled in " +
                        "already.");
            }
            if (solverConfig.EntityClassList == null || solverConfig.EntityClassList.Count == 0)
            {
                throw new Exception("The solver configuration must have at least 1 entityClass (" +
                        solverConfig.EntityClassList + "). If you're using the Quarkus extension or Spring Boot starter, " +
                        "it should have been filled in already.");
            }
            SolutionDescriptor solutionDescriptor =
                    SolutionDescriptor.BuildSolutionDescriptor(solverConfig.DetermineDomainAccessType(),
                            solverConfig.SolutionClass,
                            solverConfig.GizmoMemberAccessorMap,
                            /*solverConfig.GizmoSolutionClonerMap,*/ null, //prob met generic
                            solverConfig.EntityClassList);
            EnvironmentMode environmentMode = solverConfig.DetermineEnvironmentMode();
            if (EnvironmentModeEnumHelper.IsAsserted(environmentMode))
            {
                solutionDescriptor.AssertModelForCloning = true;
            }
            return solutionDescriptor;
        }

        public override API.Solver.Solver BuildSolver()
        {
            bool daemon_ = solverConfig.Daemon ?? false;

            SolverScope solverScope = new SolverScope();
            MonitoringConfig monitoringConfig = solverConfig.DetermineMetricConfig();
            solverScope.MonitoringTags = new Tags();
            if (monitoringConfig.SolverMetricList.Count > 0)
            {
                solverScope.SolverMetricSet = new HashSet<SolverMetric>(monitoringConfig.SolverMetricList);
            }
            else
            {
                solverScope.SolverMetricSet = new HashSet<SolverMetric>();
            }

            EnvironmentMode environmentMode_ = solverConfig.DetermineEnvironmentMode();
            InnerScoreDirector innerScoreDirector = scoreDirectorFactory.BuildScoreDirector(true, EnvironmentModeEnumHelper.IsAsserted(environmentMode_));
            solverScope.ScoreDirector = innerScoreDirector;
            solverScope.ProblemChangeDirector = new DefaultProblemChangeDirector(innerScoreDirector);


            int? moveThreadCount_ = MoveThreadCountResolver.ResolveMoveThreadCount(solverConfig.MoveThreadCount);
            BestSolutionRecaller bestSolutionRecaller = BestSolutionRecallerFactory.Create().BuildBestSolutionRecaller(environmentMode_);
            HeuristicConfigPolicy configPolicy = new HeuristicConfigPolicy(new HeuristicConfigPolicy.Builder(
                    environmentMode_,
                    moveThreadCount_,
                    solverConfig.MoveThreadBufferSize,
                    solverConfig.ThreadFactoryClass,
                    scoreDirectorFactory.GetInitializingScoreTrend(),
                    solutionDescriptor,
                    ClassInstanceCache.Create()));

            TerminationConfig terminationConfig_ = solverConfig.TerminationConfig ?? new TerminationConfig();
            BasicPlumbingTermination basicPlumbingTermination = new BasicPlumbingTermination(daemon_);
            Solver.Termination.Termination termination = TerminationFactory.Create(terminationConfig_)
                    .BuildTermination(configPolicy, basicPlumbingTermination);

            List<Phase.Phase> phaseList = BuildPhaseList(configPolicy, bestSolutionRecaller, termination);

            RandomFactory randomFactory = BuildRandomFactory(environmentMode_);
            return new DefaultSolver(environmentMode_, randomFactory, bestSolutionRecaller, basicPlumbingTermination,
                    termination, phaseList, solverScope, moveThreadCount_ == null ? SolverConfig.MOVE_THREAD_COUNT_NONE : moveThreadCount_.ToString());
        }

        private RandomFactory BuildRandomFactory(EnvironmentMode environmentMode_)
        {
            RandomFactory randomFactory;
            if (solverConfig.RandomFactoryClass != null)
            {
                if (solverConfig.RandomType != null || solverConfig.RandomSeed != null)
                {
                    throw new Exception(
                            "The solverConfig with randomFactoryClass (" + solverConfig.RandomFactoryClass
                                    + ") has a non-null randomType (" + solverConfig.RandomType
                                    + ") or a non-null randomSeed (" + solverConfig.RandomSeed + ").");
                }
                randomFactory = ConfigUtils.NewInstance<RandomFactory>(solverConfig, "randomFactoryClass", solverConfig.RandomFactoryClass);
            }
            else
            {
                RandomType randomType_ = solverConfig.RandomType ?? RandomType.JDK;
                long? randomSeed_ = solverConfig.RandomSeed;
                if (solverConfig.RandomSeed == null && environmentMode_ != EnvironmentMode.NON_REPRODUCIBLE)
                {
                    randomSeed_ = DEFAULT_RANDOM_SEED;
                }
                randomFactory = new DefaultRandomFactory(randomType_, randomSeed_);
            }
            return randomFactory;
        }

        private InnerScoreDirectorFactory BuildScoreDirectorFactory()
        {
            EnvironmentMode environmentMode = solverConfig.DetermineEnvironmentMode();
            ScoreDirectorFactoryConfig scoreDirectorFactoryConfig_ = solverConfig.ScoreDirectorFactoryConfig ?? new ScoreDirectorFactoryConfig();
            ScoreDirectorFactoryFactory scoreDirectorFactoryFactory = new ScoreDirectorFactoryFactory(scoreDirectorFactoryConfig_);
            return scoreDirectorFactoryFactory.BuildScoreDirectorFactory(/*solverConfig.getClassLoader(),*/ environmentMode, solutionDescriptor);
        }

        private List<Phase.Phase> BuildPhaseList(HeuristicConfigPolicy configPolicy, BestSolutionRecaller bestSolutionRecaller, Solver.Termination.Termination termination)
        {
            List<AbstractPhaseConfig> phaseConfigList_ = solverConfig.GetPhaseConfigList();
            if (phaseConfigList_ == null || phaseConfigList_.Count == 0)
            {
                List<EntityDescriptor> genuineEntityDescriptors = configPolicy.BuilderInfo.SolutionDescriptor.GetGenuineEntityDescriptors();
                Dictionary<Type, List<ListVariableDescriptor>> entityClassToListVariableDescriptorListMap =
                        configPolicy.BuilderInfo.SolutionDescriptor
                                .ListVariableDescriptors
                                .GroupBy(
                    listVariableDescriptor => listVariableDescriptor.EntityDescriptor.EntityClass,
                    listVariableDescriptor => listVariableDescriptor)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList());

                phaseConfigList_ = new List<AbstractPhaseConfig>(genuineEntityDescriptors.Count + 1);
                foreach (var genuineEntityDescriptor in genuineEntityDescriptors)
                {
                    ConstructionHeuristicPhaseConfig constructionHeuristicPhaseConfig = new ConstructionHeuristicPhaseConfig();
                    IAbstractEntityPlacerConfig entityPlacerConfig = null;

                    if (entityClassToListVariableDescriptorListMap.ContainsKey(genuineEntityDescriptor.EntityClass))
                    {
                        List<ListVariableDescriptor> listVariableDescriptorList =
                                entityClassToListVariableDescriptorListMap[genuineEntityDescriptor.EntityClass];
                        if (listVariableDescriptorList.Count != 1)
                        {
                            // TODO: Do multiple Construction Heuristics for each list variable descriptor?
                            throw new Exception(
                                    "Construction Heuristic phase does not support multiple list variables ("
                                            + listVariableDescriptorList + ") for planning entity (" +
                                            genuineEntityDescriptor.EntityClass + ").");
                        }
                        entityPlacerConfig =
                                DefaultConstructionHeuristicPhaseFactory.BuildListVariableQueuedValuePlacerConfig(configPolicy,
                                        listVariableDescriptorList[0]);
                    }
                    else
                    {
                        var qeConfig = new QueuedEntityPlacerConfig();
                        var qeWithConfig = qeConfig.WithEntitySelectorConfig(AbstractFromConfigFactory<QueuedEntityPlacerConfig>.GetDefaultEntitySelectorConfigForEntity(configPolicy, genuineEntityDescriptor));
                        entityPlacerConfig = qeWithConfig;
                    }

                    constructionHeuristicPhaseConfig.EntityPlacerConfig = entityPlacerConfig;
                    phaseConfigList_.Add(constructionHeuristicPhaseConfig);
                }
                phaseConfigList_.Add(new LocalSearchPhaseConfig());
            }
            return PhaseFactory.BuildPhases(phaseConfigList_, configPolicy, bestSolutionRecaller, termination);
        }
    }

    public static class MoveThreadCountResolver
    {
        public static int? ResolveMoveThreadCount(string moveThreadCount)
        {
            int availableProcessorCount = GetAvailableProcessors();
            int resolvedMoveThreadCount;
            if (moveThreadCount == null || moveThreadCount.Equals(SolverConfig.MOVE_THREAD_COUNT_NONE))
            {
                return null;
            }
            else if (moveThreadCount.Equals(SolverConfig.MOVE_THREAD_COUNT_AUTO))
            {
                // Leave one for the Operating System and 1 for the solver thread, take the rest
                resolvedMoveThreadCount = (availableProcessorCount - 2);
                // A moveThreadCount beyond 4 is currently typically slower
                // TODO remove limitation after fixing https://issues.redhat.com/browse/PLANNER-2449
                if (resolvedMoveThreadCount > 4)
                {
                    resolvedMoveThreadCount = 4;
                }
                if (resolvedMoveThreadCount <= 1)
                {
                    // Fall back to single threaded solving with no move threads.
                    // To deliberately enforce 1 moveThread, set the moveThreadCount explicitly to 1.
                    return null;
                }
            }
            else
            {
                resolvedMoveThreadCount = ConfigUtils.ResolvePoolSize("moveThreadCount", moveThreadCount,
                        SolverConfig.MOVE_THREAD_COUNT_NONE, SolverConfig.MOVE_THREAD_COUNT_AUTO);
            }
            if (resolvedMoveThreadCount < 1)
            {
                throw new Exception("resolvedMoveThreadCount < 1");
            }

            return resolvedMoveThreadCount;
        }

        public static int GetAvailableProcessors()
        {
            return Environment.ProcessorCount;
        }
    }
}
