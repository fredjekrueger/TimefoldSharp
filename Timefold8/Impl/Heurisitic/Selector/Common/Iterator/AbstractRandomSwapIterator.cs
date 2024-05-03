namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    public class AbstractRandomSwapIterator : UpcomingSelectionIterator<Heurisitic.Move.Move>
    {

        protected IEnumerable<object> leftSubSelector;
        protected IEnumerable<object> rightSubSelector;

        protected IEnumerator<object> leftSubSelectionIterator;
        protected IEnumerator<object> rightSubSelectionIterator;

        public AbstractRandomSwapIterator(IEnumerable<object> leftSubSelector, IEnumerable<object> rightSubSelector)
        {
            this.leftSubSelector = leftSubSelector;
            this.rightSubSelector = rightSubSelector;
            leftSubSelectionIterator = this.leftSubSelector.GetEnumerator();
            rightSubSelectionIterator = this.rightSubSelector.GetEnumerator();
            // Don't do hasNext() in constructor (to avoid upcoming selections breaking mimic recording)
        }

        public Func<object, object, Heurisitic.Move.Move> NewSwapSelection;

        protected override Heurisitic.Move.Move CreateUpcomingSelection()
        {
            if (!leftSubSelectionIterator.MoveNext())
            {
                leftSubSelectionIterator = leftSubSelector.GetEnumerator();
                if (!leftSubSelectionIterator.MoveNext())
                {
                    return NoUpcomingSelection();
                }
            }
            object leftSubSelection = leftSubSelectionIterator.Current;
            if (!rightSubSelectionIterator.MoveNext())
            {
                rightSubSelectionIterator = rightSubSelector.GetEnumerator();
                if (!rightSubSelectionIterator.MoveNext())
                {
                    return NoUpcomingSelection();
                }
            }
            object rightSubSelection = rightSubSelectionIterator.Current;
            return NewSwapSelection(leftSubSelection, rightSubSelection);
        }
    }
}
