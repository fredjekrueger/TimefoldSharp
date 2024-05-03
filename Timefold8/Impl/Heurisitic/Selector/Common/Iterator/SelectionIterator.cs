using System.Collections;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    public abstract class SelectionIterator<S> : IEnumerator<S>
    {
        public abstract S Current { get; }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public abstract bool MoveNext();

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
