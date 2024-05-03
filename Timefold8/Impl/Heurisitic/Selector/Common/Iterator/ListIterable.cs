namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    public interface ListIterable<T> : IEnumerable<T>
    {
        IEnumerator<T> ListIterator();
    }
}
