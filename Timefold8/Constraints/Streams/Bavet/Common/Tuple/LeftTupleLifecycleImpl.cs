namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public class LeftTupleLifecycleImpl : TupleLifecycle
    {
        private readonly LeftTupleLifecycle leftTupleLifecycle;

        public LeftTupleLifecycleImpl(LeftTupleLifecycle leftTupleLifecycle)
        {
            this.leftTupleLifecycle = leftTupleLifecycle;
        }

        public void Insert(ITuple tuple)
        {
            leftTupleLifecycle.InsertLeft(tuple);
        }

        public void Retract(ITuple tuple)
        {
            leftTupleLifecycle.RetractLeft(tuple);
        }

        public void Update(ITuple tuple)
        {
            leftTupleLifecycle.UpdateLeft(tuple);
        }

        public override string ToString()
        {
            return "left " + leftTupleLifecycle;
        }
    }
}
