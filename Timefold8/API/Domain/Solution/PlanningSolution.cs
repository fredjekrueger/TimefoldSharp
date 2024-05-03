namespace TimefoldSharp.Core.API.Domain.Solution
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlanningSolutionAttribute : Attribute
    {
        public AutoDiscoverMemberType AutoDiscoverMemberType { get; set; } = AutoDiscoverMemberType.NONE;
        public LookUpStrategyType LookUpStrategyType { get; set; } = LookUpStrategyType.PLANNING_ID_OR_NONE;
        public Type SolutionCloner { get; set; } = typeof(NullSolutionCloner);
    }

    interface NullSolutionCloner : SolutionCloner
    {
    }

    public enum AutoDiscoverMemberType
    {
        NONE, PROPERTY, GETTER
    }

    public enum LookUpStrategyType
    {
        PLANNING_ID_OR_NONE, PLANNING_ID_OR_FAIL_FAST, EQUALITY, NONE
    }
}
