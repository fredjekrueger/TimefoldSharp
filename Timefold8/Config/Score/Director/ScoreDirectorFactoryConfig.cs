using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Util;

namespace TimefoldSharp.Core.Config.Score.Director
{
    public class ScoreDirectorFactoryConfig : AbstractConfig<ScoreDirectorFactoryConfig>
    {
        public Type ConstraintProviderClass { get; set; } = null;
        public Type EasyScoreCalculatorClass { get; set; } = null;
        public Dictionary<string, string> ConstraintProviderCustomProperties { get; set; } = null;
        public Dictionary<string, string> EasyScoreCalculatorCustomProperties { get; set; } = null;
        public ConstraintStreamImplType? ConstraintStreamImplType { get; set; }
        public Type IncrementalScoreCalculatorClass { get; set; } = null;
        public Dictionary<string, string> IncrementalScoreCalculatorCustomProperties { get; set; } = null;
        public List<string> ScoreDrlList { get; set; } = null;
        public string InitializingScoreTrend { get; set; } = null;
        public ScoreDirectorFactoryConfig AssertionScoreDirectorFactory { get; set; } = null;

        public ScoreDirectorFactoryConfig CopyConfig()
        {
            return new ScoreDirectorFactoryConfig().Inherit(this);
        }

        public ScoreDirectorFactoryConfig Inherit(ScoreDirectorFactoryConfig inheritedConfig)
        {
            EasyScoreCalculatorClass = ConfigUtils.InheritOverwritableProperty(EasyScoreCalculatorClass, inheritedConfig.EasyScoreCalculatorClass);
            EasyScoreCalculatorCustomProperties = ConfigUtils.InheritMergeableMapProperty(EasyScoreCalculatorCustomProperties, inheritedConfig.EasyScoreCalculatorCustomProperties);
            ConstraintProviderClass = ConfigUtils.InheritOverwritableProperty(ConstraintProviderClass, inheritedConfig.ConstraintProviderClass);
            ConstraintProviderCustomProperties = ConfigUtils.InheritMergeableMapProperty(ConstraintProviderCustomProperties, inheritedConfig.ConstraintProviderCustomProperties);
            ConstraintStreamImplType = ConfigUtils.InheritOverwritableProperty(ConstraintStreamImplType, inheritedConfig.ConstraintStreamImplType);
            IncrementalScoreCalculatorClass = ConfigUtils.InheritOverwritableProperty(IncrementalScoreCalculatorClass, inheritedConfig.IncrementalScoreCalculatorClass);
            IncrementalScoreCalculatorCustomProperties = ConfigUtils.InheritMergeableMapProperty(IncrementalScoreCalculatorCustomProperties, inheritedConfig.IncrementalScoreCalculatorCustomProperties);
            ScoreDrlList = ConfigUtils.InheritMergeableListProperty(ScoreDrlList, inheritedConfig.ScoreDrlList);
            InitializingScoreTrend = ConfigUtils.InheritOverwritableProperty(InitializingScoreTrend, inheritedConfig.InitializingScoreTrend);
            AssertionScoreDirectorFactory = ConfigUtils.InheritOverwritableProperty(AssertionScoreDirectorFactory, inheritedConfig.AssertionScoreDirectorFactory);
            return this;
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }
    }
}
