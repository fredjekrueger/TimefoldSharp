using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator
{
    public sealed class CompositeSelectionFilter<T> : SelectionFilter<T>
    {

        public CompositeSelectionFilter(SelectionFilter<T>[] selectionFilterArray)
        {
            this.selectionFilterArray = selectionFilterArray;
            Accept = AcceptInt;
        }

        public static SelectionFilter<T> NOOP
        {
            get
            {
                return new SelectionFilter<T>() { Accept = (s, e) => true };
            }
        }

        public SelectionFilter<T>[] selectionFilterArray;

        public override Func<ScoreDirector, T, bool> Accept { get; set; }

        public bool AcceptInt(ScoreDirector scoreDirector, T selection)
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
