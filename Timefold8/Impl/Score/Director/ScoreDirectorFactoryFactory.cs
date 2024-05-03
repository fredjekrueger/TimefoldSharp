using TimefoldSharp.Core.Config.Score.Director;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Trend;

namespace TimefoldSharp.Core.Impl.Score.Director
{
    internal class ScoreDirectorFactoryFactory
    {
        private readonly ScoreDirectorFactoryConfig config;

        public ScoreDirectorFactoryFactory(ScoreDirectorFactoryConfig config)
        {
            this.config = config;
        }

        public InnerScoreDirectorFactory BuildScoreDirectorFactory(/*ClassLoader classLoader,*/
            EnvironmentMode environmentMode, SolutionDescriptor solutionDescriptor)
        {
            AbstractScoreDirectorFactory scoreDirectorFactory =
                    DecideMultipleScoreDirectorFactories(solutionDescriptor, environmentMode);
            if (config.AssertionScoreDirectorFactory != null)
            {
                if (config.AssertionScoreDirectorFactory.AssertionScoreDirectorFactory != null)
                {
                    throw new Exception("A assertionScoreDirectorFactory (" + ") cannot have a non-null assertionScoreDirectorFactory ("
                           + ").");
                }
                if (environmentMode < EnvironmentMode.FAST_ASSERT) //kan ook omgekeerd zijn
                {
                    throw new Exception("A non-null assertionScoreDirectorFactory () requires an environmentMode ("
                            + environmentMode + ") of " + EnvironmentMode.FAST_ASSERT + " or lower.");
                }
                ScoreDirectorFactoryFactory assertionScoreDirectorFactoryFactory =
                        new ScoreDirectorFactoryFactory(config.AssertionScoreDirectorFactory);
                scoreDirectorFactory.AssertionScoreDirectorFactory = assertionScoreDirectorFactoryFactory
                        .BuildScoreDirectorFactory(/*classLoader, */EnvironmentMode.NON_REPRODUCIBLE, solutionDescriptor);
            }
            scoreDirectorFactory.InitializingScoreTrend = (InitializingScoreTrend.ParseTrend(
                    config.InitializingScoreTrend == null ? InitializingScoreTrendLevel.ANY.ToString()
                            : config.InitializingScoreTrend, solutionDescriptor.GetScoreDefinition().GetLevelsSize()));
            if (EnvironmentModeEnumHelper.IsNonIntrusiveFullAsserted(environmentMode))
            {
                scoreDirectorFactory.AssertClonedSolution = true;
            }
            return scoreDirectorFactory;

        }

        static bool IsSubclassOf(Type type, Type baseType)
        {
            while (type != null)
            {
                if (type == baseType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }

        protected AbstractScoreDirectorFactory DecideMultipleScoreDirectorFactories(
            /*ClassLoader classLoader,*/ SolutionDescriptor solutionDescriptor, EnvironmentMode environmentMode)
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<ScoreDirectorFactoryService> implementingTypes = new List<ScoreDirectorFactoryService>();
            foreach (var assembly in assemblies)
            {

                var types = assembly.GetTypes()
                    .Where(type => type.GetInterfaces().Contains(typeof(ScoreDirectorFactoryService)) && !type.IsAbstract && !type.IsInterface);
                foreach (var type in types)
                {
                    implementingTypes.Add((ScoreDirectorFactoryService)Activator.CreateInstance(type));
                }
            }

            Dictionary<ScoreDirectorType, Func<AbstractScoreDirectorFactory>> scoreDirectorFactorySupplierMap = new Dictionary<ScoreDirectorType, Func<AbstractScoreDirectorFactory>>();

            foreach (var service in implementingTypes)
            {
                Func<AbstractScoreDirectorFactory> factory = service.BuildScoreDirectorFactory(solutionDescriptor, config, environmentMode);
                if (factory != null)
                {
                    scoreDirectorFactorySupplierMap.Add(service.GetSupportedScoreDirectorType(), factory);
                }
            }

            Func<AbstractScoreDirectorFactory> easyScoreDirectorFactorySupplier = scoreDirectorFactorySupplierMap.GetValueOrDefault(ScoreDirectorType.EASY);
            Func<AbstractScoreDirectorFactory> constraintStreamScoreDirectorFactorySupplier = scoreDirectorFactorySupplierMap.GetValueOrDefault(ScoreDirectorType.CONSTRAINT_STREAMS);
            Func<AbstractScoreDirectorFactory> incrementalScoreDirectorFactorySupplier = scoreDirectorFactorySupplierMap.GetValueOrDefault(ScoreDirectorType.INCREMENTAL);


            // Every non-null supplier means that ServiceLoader successfully loaded and configured a score director factory.

            if (easyScoreDirectorFactorySupplier != null)
            {
                return easyScoreDirectorFactorySupplier.Invoke();
            }
            else if (incrementalScoreDirectorFactorySupplier != null)
            {
                return incrementalScoreDirectorFactorySupplier.Invoke();
            }
            if (constraintStreamScoreDirectorFactorySupplier != null)
            {
                return constraintStreamScoreDirectorFactorySupplier.Invoke();
            }

            throw new Exception("The scoreDirectorFactory lacks configuration for "
                    + "either constraintProviderClass, easyScoreCalculatorClass or incrementalScoreCalculatorClass.");
        }


    }
}