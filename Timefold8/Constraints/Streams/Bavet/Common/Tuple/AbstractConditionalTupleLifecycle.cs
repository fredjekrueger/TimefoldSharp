namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public abstract class AbstractConditionalTupleLifecycle : TupleLifecycle
    {
        private TupleLifecycle tupleLifecycle;

        abstract protected bool Test(ITuple tuple);

        protected AbstractConditionalTupleLifecycle(TupleLifecycle tupleLifecycle)
        {
            this.tupleLifecycle = tupleLifecycle;
        }

        public void Insert(ITuple tuple)
        {
            if (Test(tuple))
            {
                tupleLifecycle.Insert(tuple);
            }
        }

        public void Retract(ITuple tuple)
        {
            tupleLifecycle.Retract(tuple);
        }

        public void Update(ITuple tuple)
        {
            if (Test(tuple))
            {
                tupleLifecycle.Update(tuple);
            }
            else
            {
                tupleLifecycle.Retract(tuple);
            }
        }

        public override string ToString()
        {
            return "Conditional " + tupleLifecycle;
        }
    }
}
