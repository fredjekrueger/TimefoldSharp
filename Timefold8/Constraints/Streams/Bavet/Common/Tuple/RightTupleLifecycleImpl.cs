namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public class RightTupleLifecycleImpl : TupleLifecycle
    {
        private readonly RightTupleLifecycle rightTupleLifecycle;

        public void Insert(ITuple tuple)
        {
            rightTupleLifecycle.InsertRight(tuple);
        }

        public void Update(ITuple tuple)
        {
            rightTupleLifecycle.UpdateRight(tuple);
        }

        public void Retract(ITuple tuple)
        {
            rightTupleLifecycle.RetractRight(tuple);
        }

        public RightTupleLifecycleImpl(RightTupleLifecycle rightTupleLifecycle)
        {
            this.rightTupleLifecycle = rightTupleLifecycle;
        }
    }
}
