using System.Collections;
using TimefoldSharp.Core.API.Score;

namespace TimefoldSharp.Core.API.Domain.Entity
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlanningEntityAttribute : Attribute
    {
        public Type PinningFilter { get; set; } = typeof(NullPinningFilter);
        public Type DifficultyWeightFactoryClass { get; set; } = typeof(NullDifficultyWeightFactory);
        public Type DifficultyComparatorClass = typeof(NullDifficultyComparator);


    }

    public interface IPinningFilter
    {
        bool Accept(ISolution solution, object entity);
    }

    public interface NullPinningFilter : IPinningFilter { }
    public interface NullDifficultyComparator : IComparer { }
    public interface SelectionSorterWeightFactory { }
    public interface NullDifficultyWeightFactory : SelectionSorterWeightFactory { }
}
