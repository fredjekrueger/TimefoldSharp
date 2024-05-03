namespace TimefoldSharp.Core.Impl.Heurisitic.Selector
{
    public interface IterableSelector<T> : Selector, IEnumerable<T>
    {
        long GetSize();
    }
}