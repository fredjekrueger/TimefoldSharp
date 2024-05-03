namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public interface TupleLifecycle
    {
        void Insert(ITuple tuple);

        void Update(ITuple tuple);

        void Retract(ITuple tuple);
    }

    public static class TupleLifecycleHelper
    {
        public static TupleLifecycle OfLeft(LeftTupleLifecycle leftTupleLifecycle)
        {
            return new LeftTupleLifecycleImpl(leftTupleLifecycle);
        }

        public static TupleLifecycle OfRight(RightTupleLifecycle rightTupleLifecycle)
        {
            return new RightTupleLifecycleImpl(rightTupleLifecycle);
        }
    }
}
