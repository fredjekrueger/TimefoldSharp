using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public class SelectionFilter<T> //was interface
    {

        public static SelectionFilter<T> Compose(List<SelectionFilter<T>> filterList)
        {
            var distinctFilterArray = filterList.SelectMany(filter =>
            {
                if (filter == CompositeSelectionFilter<T>.NOOP)
                {
                    return Enumerable.Empty<SelectionFilter<T>>();
                }
                else if (filter is CompositeSelectionFilter<T>)
                {
                    // Decompose composites if necessary; avoids needless recursion.
                    return ((CompositeSelectionFilter<T>)filter).selectionFilterArray.AsEnumerable();
                }
                else
                {
                    return Enumerable.Repeat(filter, 1);
                }
            })
        .Distinct()
        .ToArray();
            return distinctFilterArray.Length switch
            {
                0 => CompositeSelectionFilter<T>.NOOP,
                1 => distinctFilterArray[0],
                _ => new CompositeSelectionFilter<T>(distinctFilterArray),
            };
        }

        public virtual Func<ScoreDirector, T, bool> Accept { get; set; }
    }
}
