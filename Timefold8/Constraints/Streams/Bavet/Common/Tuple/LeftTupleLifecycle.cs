namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public interface LeftTupleLifecycle
    {
        void InsertLeft(ITuple tuple);
        void UpdateLeft(ITuple tuple);
        void RetractLeft(ITuple tuple);
    }
}
