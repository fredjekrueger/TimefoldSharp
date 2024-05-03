using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public abstract class SelectionFilter<T> //was interface
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
            switch (distinctFilterArray.Length)
            {
                case 0:
                    return CompositeSelectionFilter<T>.NOOP;
                case 1:
                    return distinctFilterArray[0];
                default:
                    return new CompositeSelectionFilter<T>(distinctFilterArray);
            }
        }

        public abstract bool Accept(ScoreDirector scoreDirector, T selection);
    }
}
