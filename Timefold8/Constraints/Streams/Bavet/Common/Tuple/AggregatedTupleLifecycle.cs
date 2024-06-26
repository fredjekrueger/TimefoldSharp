namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public class AggregatedTupleLifecycle : TupleLifecycle
    {

        private readonly TupleLifecycle[] lifecycles;

        public AggregatedTupleLifecycle(TupleLifecycle[] lifecycles)
        {
            this.lifecycles = lifecycles;
        }

        public void Insert(ITuple tuple)
        {
            foreach (var lifecycle in lifecycles)
            {
                lifecycle.Insert(tuple);
            }
        }

        public void Retract(ITuple tuple)
        {
            int count = 0;
            foreach (var lifecycle in lifecycles)
            {
                lifecycle.Retract(tuple);
                count++;
            }
        }

        public override string ToString()
        {
            return "size = " + lifecycles.Length;
        }

        public void Update(ITuple tuple)
        {
            foreach (var lifecycle in lifecycles)
            {
                lifecycle.Update(tuple);
            }
        }
    }
}
