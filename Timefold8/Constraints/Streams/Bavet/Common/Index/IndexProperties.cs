namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public interface IndexProperties
    {
        object ToKey(int from, int to);
        object ToKey(int index);
    }
}
