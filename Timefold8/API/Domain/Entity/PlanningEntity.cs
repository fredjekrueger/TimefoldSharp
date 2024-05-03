using System.Collections;

namespace TimefoldSharp.Core.API.Domain.Entity
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlanningEntityAttribute : Attribute
    {
        public Type PinningFilter { get; set; } = typeof(NullPinningFilter);
        public Type DifficultyWeightFactoryClass { get; set; } = typeof(NullDifficultyWeightFactory);
        public Type DifficultyComparatorClass = typeof(NullDifficultyComparator);


    }

    public interface PinningFilter<Entity_> { }
    public interface NullPinningFilter : PinningFilter<object> { }
    public interface NullDifficultyComparator : IComparer { }
    public interface SelectionSorterWeightFactory { }
    public interface NullDifficultyWeightFactory : SelectionSorterWeightFactory { }
}
