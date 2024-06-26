using System.Net.NetworkInformation;
using System.Threading;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public sealed class DynamicPropagationQueue<Tuple_, Carrier_> : PropagationQueue<Carrier_>
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

        public void Insert(Carrier_ carrier)
        {
            int positionInDirtyList = carrier.PositionInDirtyList;
            if (positionInDirtyList < 0)
            {
                MakeDirty(carrier, insertQueue);
            }
            else
            {
                switch (carrier.GetState())
                {
                    case TupleState.UPDATING: insertQueue.Set(positionInDirtyList); break;
                    case TupleState.ABORTING:
                    case TupleState.DYING: {
                            retractQueue.Clear(positionInDirtyList);
                            insertQueue.Set(positionInDirtyList);
                            break;
                        }
                    default:
                    throw new Exception("Impossible state: Cannot insert (" + carrier + "), already inserting.");
                }
            }
            carrier.SetState(TupleState.CREATING);
        }

        private void MakeDirty(Carrier_ carrier, BitSet queue)
        {
            dirtyList.Add(carrier);
            int position = dirtyList.Count - 1;
            queue.Set(position);
            carrier.PositionInDirtyList = position;
        }

        public void PropagateEverything()
        {
            PropagateRetracts();
            PropagateUpdates();
            PropagateInserts();
        }

        public void PropagateInserts()
        {
            if (!insertQueue.IsEmpty())
            {
                int i = insertQueue.NextSetBit(0);
                while (i != -1)
                {
                    PropagateInsertOrUpdate(dirtyList[i], insertPropagator);
                    i = insertQueue.NextSetBit(i + 1);
                }
                insertQueue.Clear();
            }
            retractQueue.Clear();
            dirtyList.Clear();
        }

        public void PropagateRetracts()
        {
            if (retractQueue.IsEmpty())
            {
                return;
            }
            int i = retractQueue.NextSetBit(0);
            while (i != -1)
            {
                Carrier_ carrier = dirtyList[i];
                TupleState state = carrier.GetState();
                switch (state)
                {
                    case TupleState.DYING: 
                        Propagate(carrier, retractPropagator, TupleState.DEAD); 
                        break;
                    case TupleState.ABORTING:
                        Clean(carrier, TupleState.DEAD); 
                        break;
                }
                i = retractQueue.NextSetBit(i + 1);
            }
        }

        private static void Propagate(Carrier_ carrier, Action<Tuple_> propagator, TupleState tupleState)
        {
            Clean(carrier, tupleState); // Hide original state from the next node by doing this before propagation.
            propagator.Invoke(carrier.GetTuple());
        }

        private static void Clean(AbstractPropagationMetadataCarrier<Tuple_> carrier, TupleState tupleState)
        {
            carrier.SetState(tupleState);
            carrier.PositionInDirtyList = -1;
        }

        private static BitSet BuildInsertAndRetractQueue(BitSet insertQueue, BitSet retractQueue)
        {
            bool noInserts = insertQueue.IsEmpty();
            bool noRetracts = retractQueue.IsEmpty();
            if (noInserts && noRetracts)
            {
                return null;
            }
            else if (noInserts)
            {
                return retractQueue;
            }
            else if (noRetracts)
            {
                return insertQueue;
            }
            else
            {
                BitSet updateQueue = new BitSet();
                updateQueue.Or(insertQueue);
                updateQueue.Or(retractQueue);
                return updateQueue;
            }
        }

        private void PropagateInsertOrUpdate(Carrier_ carrier, Action<Tuple_> propagator)
        {
            if (preprocessor != null)
            {
                preprocessor.Invoke(carrier);
            }
            Propagate(carrier, propagator, TupleState.OK);
        }

        public void PropagateUpdates()
        {
            BitSet insertAndRetractQueue = BuildInsertAndRetractQueue(insertQueue, retractQueue);
            if (insertAndRetractQueue == null)
            { // Iterate over the entire list more efficiently.
                foreach (var carrier in dirtyList)
                {
                    PropagateInsertOrUpdate(carrier, updatePropagator);
                }
            }
            else
            { // The gaps in the queue are the updates.
                int dirtyListSize = dirtyList.Count;
                int i = insertAndRetractQueue.NextClearBit(0);
                while (i != -1 && i < dirtyListSize)
                {
                    PropagateInsertOrUpdate(dirtyList[i], updatePropagator);
                    i = insertAndRetractQueue.NextClearBit(i + 1);
                }
            }
        }

        public void Update(Carrier_ carrier)
        {
            int positionInDirtyList = carrier.PositionInDirtyList;
            if (positionInDirtyList < 0)
            {
                dirtyList.Add(carrier);
                carrier.PositionInDirtyList = dirtyList.Count - 1;
            }
            else
            {
                switch (carrier.GetState())
                {
                    case TupleState.CREATING :insertQueue.Clear(positionInDirtyList); break;
                    case TupleState.ABORTING: case TupleState.DYING:  retractQueue.Clear(positionInDirtyList); break;
                    default: {
                            break;
                        }
                }
            }
            carrier.SetState(TupleState.UPDATING);
        }

        public void Retract(Carrier_ carrier, TupleState state)
        {
            if (TupleStateHelper.IsActive(state) || state == TupleState.DEAD)
            {
                throw new Exception("Impossible state: The state (" + state + ") is not a valid retract state.");
            }
            int positionInDirtyList = carrier.PositionInDirtyList;
            if (positionInDirtyList < 0)
            {
                MakeDirty(carrier, retractQueue);
            }
            else
            {
                switch (carrier.GetState())
                {
                    case TupleState.CREATING: {
                            insertQueue.Clear(positionInDirtyList);
                            retractQueue.Set(positionInDirtyList);
                            break;
                        }
                    case TupleState.UPDATING: retractQueue.Set(positionInDirtyList); break;
                    default:
                    throw new Exception("Impossible state: Cannot retract (" + carrier + "), already retracting.");

                }
            }
            carrier.SetState(state);
        }

    }
}
