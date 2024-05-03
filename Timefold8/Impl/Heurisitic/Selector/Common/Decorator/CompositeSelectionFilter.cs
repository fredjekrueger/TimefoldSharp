using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public sealed class CompositeSelectionFilter<T> : SelectionFilter<T>
    {

        public CompositeSelectionFilter(SelectionFilter<T>[] selectionFilterArray)
        {
            this.selectionFilterArray = selectionFilterArray;
        }

        public static SelectionFilter<T> NOOP => throw new NotImplementedException();

        public SelectionFilter<T>[] selectionFilterArray;

        public override bool Accept(ScoreDirector scoreDirector, T selection)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
