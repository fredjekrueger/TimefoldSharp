namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public interface RightTupleLifecycle
    {
        void InsertRight(ITuple tuple);
        void RetractRight(ITuple tuple);
        void UpdateRight(ITuple tuple);
    }
}
