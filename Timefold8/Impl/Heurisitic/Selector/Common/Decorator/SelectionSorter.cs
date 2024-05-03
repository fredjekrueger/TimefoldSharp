using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public interface SelectionSorter<T>
    {

        /**
         * @param scoreDirector never null, the {@link ScoreDirector}
         *        which has the {@link ScoreDirector#getWorkingSolution()} to which the selections belong or apply to
         * @param selectionList never null, a {@link List}
         *        of {@link PlanningEntity}, planningValue, {@link Move} or {@link Selector}
         */
        void Sort(ScoreDirector scoreDirector, List<T> selectionList);

    }
}