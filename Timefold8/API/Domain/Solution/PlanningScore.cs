using TimefoldSharp.Core.Impl.Domain.Score.Definition;

namespace TimefoldSharp.Core.API.Domain.Solution
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class PlanningScoreAttribute : Attribute
    {
        public int BendableHardLevelsSize { get; set; } = NO_LEVEL_SIZE;
        public int BendableSoftLevelsSize { get; set; } = NO_LEVEL_SIZE;

        public const int NO_LEVEL_SIZE = -1;

        public Type ScoreDefinitionClass { get; set; } = typeof(NullScoreDefinition);

        public interface ScoreDefinition { }

        public class NullScoreDefinition : ScoreDefinition { }
    }

    interface NullScoreDefinition : ScoreDefinition
    {
    }
}
