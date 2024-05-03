using System.Collections;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.AbstractSelectors
{
    public abstract class AbstractMoveSelector : AbstractSelector, MoveSelector
    {
        public abstract IEnumerator<Heurisitic.Move.Move> GetEnumerator();

        public abstract long GetSize();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
