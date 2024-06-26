namespace TimefoldSharp.Core.API.Domain.ValueRange
{
    public interface CountableValueRange<T> : ValueRange<T>
    {
        IEnumerator<T> CreateOriginalIterator();
        long GetSize();
        T Get(long index);
    }
}
