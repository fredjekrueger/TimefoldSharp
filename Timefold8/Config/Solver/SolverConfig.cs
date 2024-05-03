using TimefoldSharp.Core.API.Domain.Common;
using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.Config.Score.Director;
using TimefoldSharp.Core.Config.Solver.Monitoring;
using TimefoldSharp.Core.Config.Solver.Random;
using TimefoldSharp.Core.Config.Solver.Termination;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;

namespace TimefoldSharp.Core.Config.Solver
{
    /*
     @XmlRootElement(name = SolverConfig.XML_ELEMENT_NAME)
@XmlType(name = SolverConfig.XML_TYPE_NAME, propOrder = {
        "environmentMode",
        "daemon",
        "randomType",
        "randomSeed",
        "randomFactoryClass",
        "moveThreadCount",
        "moveThreadBufferSize",
        "threadFactoryClass",
        "monitoringConfig",
        "solutionClass",
        "entityClassList",
        "domainAccessType",
        "scoreDirectorFactoryConfig",
        "terminationConfig",
        "phaseConfigList",
})
     */
    public class SolverConfig : AbstractConfig<SolverConfig>
    {
        public const string XML_ELEMENT_NAME = "solver";
        public const string XML_NAMESPACE = "https://timefold.ai/xsd/solver";
        public const string XML_TYPE_NAME = "solverConfig";

        public static string MOVE_THREAD_COUNT_NONE = "NONE";
        public static string MOVE_THREAD_COUNT_AUTO = "AUTO";

        public Type RandomFactoryClass { get; set; }
        public Type ThreadFactoryClass { get; set; }
        public Type SolutionClass { get; set; }
        public List<Type> EntityClassList { get; set; }
        public EnvironmentMode? EnvironmentMode { get; set; }

        public ScoreDirectorFactoryConfig ScoreDirectorFactoryConfig { get; set; }
        public TerminationConfig TerminationConfig { get; set; }
        protected List<AbstractPhaseConfig> phaseConfigList = null;
        public bool? Daemon { get; set; }
        public RandomType? RandomType { get; set; }
        public long? RandomSeed { get; set; }
        public string MoveThreadCount { get; set; }
        public int MoveThreadBufferSize { get; set; }
        public DomainAccessType? DomainAccessType { get; set; }
        public Dictionary<string, MemberAccessor> GizmoMemberAccessorMap { get; set; } = new Dictionary<string, MemberAccessor>();
        public Dictionary<string, SolutionCloner> GizmoSolutionClonerMap { get; set; } = new Dictionary<string, SolutionCloner>();
        public MonitoringConfig MonitoringConfig { get; set; }

        public SolverConfig()
        {
        }

        public List<AbstractPhaseConfig> GetPhaseConfigList()
        {
            return phaseConfigList;
        }

        public MonitoringConfig DetermineMetricConfig()
        {
            return MonitoringConfig ??
                    new MonitoringConfig().WithSolverMetricList(new List<SolverMetric> {
                        SolverMetric.GetInfo(SolverMetric.SolverMetricEnum.SOLVE_DURATION),
                        SolverMetric.GetInfo(SolverMetric.SolverMetricEnum.ERROR_COUNT),
                        SolverMetric.GetInfo(SolverMetric.SolverMetricEnum.SCORE_CALCULATION_COUNT)});
        }

        public SolverConfig(SolverConfig inheritedConfig)
        {
            Inherit(inheritedConfig);
        }

        public EnvironmentMode DetermineEnvironmentMode()
        {
            return EnvironmentMode ?? Solver.EnvironmentMode.REPRODUCIBLE;
        }

        public SolverConfig CopyConfig()
        {
            return new SolverConfig().Inherit(this);
        }
        public SolverConfig Inherit(SolverConfig inheritedConfig)
        {
            EnvironmentMode = ConfigUtils.InheritOverwritableProperty(EnvironmentMode, inheritedConfig.EnvironmentMode);
            Daemon = ConfigUtils.InheritOverwritableProperty(Daemon, inheritedConfig.Daemon);
            RandomType = ConfigUtils.InheritOverwritableProperty(RandomType, inheritedConfig.RandomType);
            RandomSeed = ConfigUtils.InheritOverwritableProperty(RandomSeed, inheritedConfig.RandomSeed);
            RandomFactoryClass = ConfigUtils.InheritOverwritableProperty(RandomFactoryClass, inheritedConfig.RandomFactoryClass);
            MoveThreadCount = ConfigUtils.InheritOverwritableProperty(MoveThreadCount, inheritedConfig.MoveThreadCount);
            MoveThreadBufferSize = ConfigUtils.InheritOverwritableProperty(MoveThreadBufferSize, inheritedConfig.MoveThreadBufferSize);
            ThreadFactoryClass = ConfigUtils.InheritOverwritableProperty(ThreadFactoryClass, inheritedConfig.ThreadFactoryClass);
            SolutionClass = ConfigUtils.InheritOverwritableProperty(SolutionClass, inheritedConfig.SolutionClass);
            EntityClassList = ConfigUtils.InheritMergeableListProperty(EntityClassList, inheritedConfig.EntityClassList);
            DomainAccessType = ConfigUtils.InheritOverwritableProperty(DomainAccessType, inheritedConfig.DomainAccessType);
            GizmoMemberAccessorMap = ConfigUtils.InheritMergeableMapProperty(GizmoMemberAccessorMap, inheritedConfig.GizmoMemberAccessorMap);
            GizmoSolutionClonerMap = ConfigUtils.InheritMergeableMapProperty(GizmoSolutionClonerMap, inheritedConfig.GizmoSolutionClonerMap);

            ScoreDirectorFactoryConfig = ConfigUtils.InheritConfig(ScoreDirectorFactoryConfig, inheritedConfig.ScoreDirectorFactoryConfig);
            TerminationConfig = ConfigUtils.InheritConfig(TerminationConfig, inheritedConfig.TerminationConfig);
            //phaseConfigList = ConfigUtils.InheritMergeableListConfig(phaseConfigList, inheritedConfig.GetPhaseConfigList());
            MonitoringConfig = ConfigUtils.InheritConfig(MonitoringConfig, inheritedConfig.MonitoringConfig);
            return this;
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            classVisitor.Invoke(RandomFactoryClass);
            classVisitor.Invoke(ThreadFactoryClass);
            classVisitor.Invoke(SolutionClass);
            if (EntityClassList != null)
            {
                foreach (var entityClass in EntityClassList)
                {
                    classVisitor.Invoke(entityClass);
                }
            }
            if (ScoreDirectorFactoryConfig != null)
            {
                ScoreDirectorFactoryConfig.VisitReferencedClasses(classVisitor);
            }
            if (TerminationConfig != null)
            {
                TerminationConfig.VisitReferencedClasses(classVisitor);
            }
            if (phaseConfigList != null)
            {
                /*/foreach (var phaseConfig in phaseConfigList)
                {
                    phaseConfig.VisitReferencedClasses(classVisitor);
                }*/
                throw new NotImplementedException();
            }
        }
        public SolverConfig WithSolutionClass(Type solutionClass)
        {
            this.SolutionClass = solutionClass;
            return this;
        }

        public SolverConfig WithEntityClasses(params Type[] entityClasses)
        {
            EntityClassList = entityClasses.ToList();
            return this;
        }

        public SolverConfig WithConstraintProviderClass(Type constraintProviderClass)
        {
            if (ScoreDirectorFactoryConfig == null)
            {
                ScoreDirectorFactoryConfig = new ScoreDirectorFactoryConfig();
            }
            ScoreDirectorFactoryConfig.ConstraintProviderClass = constraintProviderClass;
            return this;
        }

        public SolverConfig WithTerminationSpentLimit(TimeSpan spentLimit)
        {
            if (TerminationConfig == null)
            {
                TerminationConfig = new TerminationConfig();
            }
            TerminationConfig.SpentLimit = spentLimit;
            return this;
        }

        public DomainAccessType DetermineDomainAccessType()
        {
            return DomainAccessType.HasValue ? DomainAccessType.Value : API.Domain.Common.DomainAccessType.REFLECTION;
        }
    }
}
