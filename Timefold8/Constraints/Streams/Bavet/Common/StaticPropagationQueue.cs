using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public class StaticPropagationQueue : PropagationQueue<ITuple>
    {

        private readonly List<ITuple> retractQueue;
        private readonly List<ITuple> updateQueue;
        private readonly List<ITuple> insertQueue;
        private readonly Action<ITuple> retractPropagator;
        private readonly Action<ITuple> updatePropagator;
        private readonly Action<ITuple> insertPropagator;

        public StaticPropagationQueue(TupleLifecycle nextNodesTupleLifecycle)
            : this(nextNodesTupleLifecycle, 1000)
        {

        }

        public StaticPropagationQueue(TupleLifecycle nextNodesTupleLifecycle, int size)
        {
            // Guesstimate that updates are dominant.
            this.retractQueue = new List<ITuple>(size / 20);
            this.updateQueue = new List<ITuple>((size / 20) * 18);
            this.insertQueue = new List<ITuple>(size / 20);
            // Don't create these lambdas over and over again.
            this.retractPropagator = (t) => nextNodesTupleLifecycle.Retract(t);
            this.updatePropagator = (t) => nextNodesTupleLifecycle.Update(t);
            this.insertPropagator = (t) => nextNodesTupleLifecycle.Insert(t);
        }

        public void Insert(ITuple carrier)
        {
            if (carrier.State == TupleState.CREATING)
            {
                throw new Exception("Impossible state: The tuple (" + carrier + ") is already in the insert queue.");
            }
            carrier.State = TupleState.CREATING;
            insertQueue.Add(carrier);
        }



        public void PropagateInserts()
        {
            ProcessAndClear(insertQueue, insertPropagator);
            if (retractQueue.Count > 0)
            {
                throw new Exception("Impossible state: The retract queue (" + retractQueue + ") is not empty.");
            }
            else if (updateQueue.Count > 0)
            {
                throw new Exception("Impossible state: The update queue (" + updateQueue + ") is not empty.");
            }
        }

        public void PropagateRetracts()
        {
            if (retractQueue.Count == 0)
            {
                return;
            }
            foreach (var tuple in retractQueue)
            {
                switch (tuple.State)
                {
                    case TupleState.DYING:
                        Propagate(tuple, retractPropagator, TupleState.DEAD);
                        break;
                    case TupleState.ABORTING:
                        tuple.State = TupleState.DEAD;
                        break;
                }
            }
            retractQueue.Clear();
        }

        private void Propagate(ITuple tuple, Action<ITuple> propagator, TupleState tupleState)
        {
            // Change state before propagation, so that the next node can't make decisions on the original state.
            tuple.State = tupleState;
            propagator.Invoke(tuple);
        }

        public void PropagateUpdates()
        {
            ProcessAndClear(updateQueue, updatePropagator);
        }

        private void ProcessAndClear(List<ITuple> dirtyQueue, Action<ITuple> propagator)
        {
            if (dirtyQueue.Count == 0)
            {
                return;
            }
            foreach (var tuple in dirtyQueue)
            {
                if (tuple.State == TupleState.DEAD)
                {
                    /*
                     * DEAD signifies the tuple was both in insert/update and retract queues.
                     * This happens when a tuple was inserted/updated and subsequently retracted, all before propagation.
                     * We can safely ignore the later insert/update,
                     * as by this point the more recent retract has already been processed,
                     * setting the state to DEAD.
                     */
                    continue;
                }
                Propagate(tuple, propagator, TupleState.OK);
            }
            dirtyQueue.Clear();
        }

        public void Update(ITuple carrier)
        {
            if (carrier.State == TupleState.UPDATING)
            { // Skip double updates.
                return;
            }
            carrier.State = TupleState.UPDATING;
            updateQueue.Add(carrier);
        }

        public void Retract(ITuple carrier, TupleState state)
        {
            if (carrier.State == state)
            { // Skip double retracts.
                return;
            }
            if (TupleStateHelper.IsActive(state) || state == TupleState.DEAD)
            {
                throw new Exception("Impossible state: The state (" + state + ") is not a valid retract state.");
            }
            else if (carrier.State == TupleState.ABORTING || carrier.State == TupleState.DYING)
            {
                throw new Exception("Impossible state: The tuple (" + carrier + ") is already in the retract queue.");
            }
            carrier.State = state;
            retractQueue.Add(carrier);
        }

        public void PropagateEverything()
        {
            PropagateRetracts();
            PropagateUpdates();
            PropagateInserts();
        }
    }
}
