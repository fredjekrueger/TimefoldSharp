using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public sealed class DynamicPropagationQueue<Tuple_, Carrier_> : PropagationQueue
        where Tuple_ : AbstractTuple
        where Carrier_ : AbstractPropagationMetadataCarrier<Tuple_>
    {

        private readonly Action<Carrier_> preprocessor;
        private readonly List<Carrier_> dirtyList;
        private readonly BitSet retractQueue;
        private readonly BitSet insertQueue;
        private readonly Action<Tuple_> retractPropagator;
        private readonly Action<Tuple_> updatePropagator;
        private readonly Action<Tuple_> insertPropagator;

        private DynamicPropagationQueue(TupleLifecycle nextNodesTupleLifecycle, Action<Carrier_> preprocessor, int size)
        {
            this.preprocessor = preprocessor;
            /*
             * All dirty carriers are stored in a list, never moved, never removed unless after propagation.
             * Their unchanging position in the list is their index for the bitset-based queues.
             * This way, we can cheaply move them between the queues.
             */
            this.dirtyList = new List<Carrier_>(size);
            // Updates tend to be dominant; update queue isn't stored, it's deduced as neither insert nor retract.
            this.retractQueue = new BitSet(size);
            this.insertQueue = new BitSet(size);
            // Don't create these lambdas over and over again.
            this.retractPropagator = (i) => nextNodesTupleLifecycle.Retract(i);
            this.updatePropagator = (i) => nextNodesTupleLifecycle.Update(i);
            this.insertPropagator = (i) => nextNodesTupleLifecycle.Insert(i);
        }

        public DynamicPropagationQueue(TupleLifecycle nextNodesTupleLifecycle)
            : this(nextNodesTupleLifecycle, null)
        {
        }

        public DynamicPropagationQueue(TupleLifecycle nextNodesTupleLifecycle, Action<Carrier_> preprocessor)
            : this(nextNodesTupleLifecycle, preprocessor, 1000)
        {
        }

        public void Insert(ITuple item)
        {
            throw new NotImplementedException();
        }

        public void PropagateEverything()
        {
            throw new NotImplementedException();
        }

        public void PropagateInserts()
        {
            throw new NotImplementedException();
        }

        public void PropagateRetracts()
        {
            throw new NotImplementedException();
        }

        public void PropagateUpdates()
        {
            throw new NotImplementedException();
        }

        public void Update(ITuple item)
        {
            throw new NotImplementedException();
        }

        public void Retract(ITuple item, TupleState state)
        {
            throw new NotImplementedException();
        }
    }
}
