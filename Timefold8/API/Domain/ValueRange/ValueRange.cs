namespace TimefoldSharp.Core.API.Domain.ValueRange
{
    public interface ValueRange<T>
    {
        IEnumerator<T> CreateRandomIterator(Random workingRandom);
        bool IsEmpty();
        bool Contains(T value);
    }
}
