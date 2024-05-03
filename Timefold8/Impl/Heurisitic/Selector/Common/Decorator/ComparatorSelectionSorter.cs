using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public sealed class ComparatorSelectionSorter<T> : SelectionSorter<T>
    {
        public void Sort(ScoreDirector scoreDirector, List<T> selectionList)
        {
            throw new NotImplementedException();
        }

        private readonly Comparer<T> appliedComparator;

        public ComparatorSelectionSorter(Comparer<T> comparator, SelectionSorterOrder selectionSorterOrder)
        {
            switch (selectionSorterOrder)
            {
                case SelectionSorterOrder.ASCENDING:
                    this.appliedComparator = comparator;
                    break;
                case SelectionSorterOrder.DESCENDING:
                    throw new NotImplementedException();
                    //this.appliedComparator = Collections.reverseOrder(comparator);
                    break;
                default:
                    throw new Exception("The selectionSorterOrder (" + selectionSorterOrder
                            + ") is not implemented.");
            }
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
