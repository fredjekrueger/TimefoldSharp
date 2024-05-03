using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator.ListIteratble;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    public class AbstractOriginalSwapIterator : UpcomingSelectionIterator<Heurisitic.Move.Move>
    {

        protected ListIterable<object> leftSubSelector;
        protected ListIterable<object> rightSubSelector;
        protected bool leftEqualsRight;
        private IEnumerator<object> leftSubSelectionIterator;
        private IEnumerator<object> rightSubSelectionIterator;

        public AbstractOriginalSwapIterator(ListIterable<object> leftSubSelector, ListIterable<object> rightSubSelector)
        {
            this.leftSubSelector = leftSubSelector;
            this.rightSubSelector = rightSubSelector;
            leftEqualsRight = (leftSubSelector == rightSubSelector);
            leftSubSelectionIterator = leftSubSelector.ListIterator();
            rightSubSelectionIterator = new List<object>().GetEnumerator();
            // Don't do hasNext() in constructor (to avoid upcoming selections breaking mimic recording)
        }

        public static long GetSize(ListIterableSelector<object> leftSubSelector, ListIterableSelector<object> rightSubSelector)
        {
            if (leftSubSelector != rightSubSelector)
            {
                return leftSubSelector.GetSize() * rightSubSelector.GetSize();
            }
            else
            {
                long leftSize = leftSubSelector.GetSize();
                return leftSize * (leftSize - 1L) / 2L;
            }
        }

        protected override Heurisitic.Move.Move CreateUpcomingSelection()
        {
            throw new NotImplementedException();
        }

        public Func<object, object, Heurisitic.Move.Move> NewSwapSelection;
    }
}
