using System.Collections;

namespace TimefoldSharp.Core.API.Domain.Variable
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class PlanningVariableAttribute : Attribute
    {
        public string[] ValueRangeProviderRefs { get; set; } = Array.Empty<string>();
        public bool Nullable { get; set; } = false;
        public PlanningVariableGraphType GraphType { get; set; } = PlanningVariableGraphType.NONE;
        public Type StrengthComparatorClass { get; set; } = typeof(NullStrengthComparator);
        public Type StrengthWeightFactoryClass { get; set; } = typeof(NullStrengthWeightFactory);

        public interface NullStrengthComparator : IComparer { }
        public interface SelectionSorterWeightFactory { }
        public interface NullStrengthWeightFactory : SelectionSorterWeightFactory { }
    }
    public enum PlanningVariableGraphType { NONE, CHAINED }
}
