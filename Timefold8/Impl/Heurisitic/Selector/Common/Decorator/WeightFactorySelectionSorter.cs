using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public sealed class WeightFactorySelectionSorter<T> : SelectionSorter<T>
    {

        private readonly SelectionSorterWeightFactory<T> selectionSorterWeightFactory;
        private readonly Comparer<IComparable> appliedWeightComparator;

        public WeightFactorySelectionSorter(SelectionSorterWeightFactory<T> selectionSorterWeightFactory, SelectionSorterOrder selectionSorterOrder)
        {
            this.selectionSorterWeightFactory = selectionSorterWeightFactory;
            switch (selectionSorterOrder)
            {
                case SelectionSorterOrder.ASCENDING:
                    throw new NotImplementedException();
                    //this.appliedWeightComparator = Comparator.naturalOrder();
                    break;
                case SelectionSorterOrder.DESCENDING:
                    throw new NotImplementedException();
                    //this.appliedWeightComparator = Collections.reverseOrder();
                    break;
                default:
                    throw new Exception("The selectionSorterOrder (" + selectionSorterOrder
                            + ") is not implemented.");
            }
        }

        public void Sort(ScoreDirector scoreDirector, List<T> selectionList)
        {
            throw new NotImplementedException();
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
