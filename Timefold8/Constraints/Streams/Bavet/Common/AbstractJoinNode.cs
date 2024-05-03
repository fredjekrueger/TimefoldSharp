using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class AbstractJoinNode<LeftTuple_, Right_, OutTuple_> : AbstractNode, LeftTupleLifecycle, RightTupleLifecycle
        where LeftTuple_ : AbstractTuple
        where OutTuple_ : AbstractTuple
    {

        protected readonly int inputStoreIndexLeftOutTupleList;
        protected readonly int inputStoreIndexRightOutTupleList;
        private readonly bool isFiltering;
        protected readonly int outputStoreIndexLeftOutEntry;
        protected readonly int outputStoreIndexRightOutEntry;
        protected readonly StaticPropagationQueue propagationQueue;

        protected AbstractJoinNode(int inputStoreIndexLeftOutTupleList, int inputStoreIndexRightOutTupleList,
          TupleLifecycle nextNodesTupleLifecycle, bool isFiltering,
          int outputStoreIndexLeftOutEntry, int outputStoreIndexRightOutEntry)
        {
            this.inputStoreIndexLeftOutTupleList = inputStoreIndexLeftOutTupleList;
            this.inputStoreIndexRightOutTupleList = inputStoreIndexRightOutTupleList;
            this.isFiltering = isFiltering;
            this.outputStoreIndexLeftOutEntry = outputStoreIndexLeftOutEntry;
            this.outputStoreIndexRightOutEntry = outputStoreIndexRightOutEntry;
            this.propagationQueue = new StaticPropagationQueue(nextNodesTupleLifecycle);
        }

        protected abstract bool TestFiltering(LeftTuple_ leftTuple, UniTuple<Right_> rightTuple);

        protected abstract OutTuple_ CreateOutTuple(LeftTuple_ leftTuple, UniTuple<Right_> rightTuple);

        protected void InsertOutTuple(LeftTuple_ leftTuple, UniTuple<Right_> rightTuple)
        {
            OutTuple_ outTuple = CreateOutTuple(leftTuple, rightTuple);
            ElementAwareList<OutTuple_> outTupleListLeft = (ElementAwareList<OutTuple_>)leftTuple.GetStore(inputStoreIndexLeftOutTupleList);
            ElementAwareListEntry<OutTuple_> outEntryLeft = outTupleListLeft.Add(outTuple);
            outTuple.SetStore(outputStoreIndexLeftOutEntry, outEntryLeft);
            ElementAwareList<OutTuple_> outTupleListRight = (ElementAwareList<OutTuple_>)rightTuple.GetStore(inputStoreIndexRightOutTupleList);
            ElementAwareListEntry<OutTuple_> outEntryRight = outTupleListRight.Add(outTuple);
            outTuple.SetStore(outputStoreIndexRightOutEntry, outEntryRight);
            propagationQueue.Insert(outTuple);
        }

        protected void InnerUpdateLeft(LeftTuple_ leftTuple, Action<Action<UniTuple<Right_>>> rightTupleConsumer)
        {
            // Prefer an update over retract-insert if possible
            var outTupleListLeft = (ElementAwareList<OutTuple_>)leftTuple.GetStore(inputStoreIndexLeftOutTupleList);
            // Propagate the update for downstream filters, matchWeighers, ...
            if (!isFiltering)
            {
                foreach (var outTuple in outTupleListLeft)
                {
                    UpdateOutTupleLeft((OutTuple_)outTuple, leftTuple);
                }
            }
            else
            {
                rightTupleConsumer.Invoke(rightTuple =>
                {
                    var rightOutList = (ElementAwareList<OutTuple_>)rightTuple.GetStore(inputStoreIndexRightOutTupleList);
                    ProcessOutTupleUpdate(leftTuple, rightTuple, rightOutList, outTupleListLeft, outputStoreIndexRightOutEntry);
                });
            }
        }

        protected abstract void SetOutTupleRightFact(OutTuple_ outTuple, UniTuple<Right_> rightTuple);

        protected void InnerUpdateRight(UniTuple<Right_> rightTuple, Action<Action<LeftTuple_>> leftTupleConsumer)
        {
            // Prefer an update over retract-insert if possible
            ElementAwareList<OutTuple_> outTupleListRight = (ElementAwareList<OutTuple_>)rightTuple.GetStore(inputStoreIndexRightOutTupleList);
            if (!isFiltering)
            {
                // Propagate the update for downstream filters, matchWeighers, ...
                foreach (var outTuple in outTupleListRight)
                {
                    SetOutTupleRightFact((OutTuple_)outTuple, rightTuple);
                    DoUpdateOutTuple((OutTuple_)outTuple);
                }
            }
            else
            {
                leftTupleConsumer.Invoke(leftTuple =>
                {
                    ElementAwareList<OutTuple_> leftOutList = (ElementAwareList<OutTuple_>)leftTuple.GetStore(inputStoreIndexLeftOutTupleList);
                    ProcessOutTupleUpdate(leftTuple, rightTuple, leftOutList, outTupleListRight, outputStoreIndexLeftOutEntry);
                });
            }
        }

        private OutTuple_ FindOutTuple(ElementAwareList<OutTuple_> outTupleList, ElementAwareList<OutTuple_> outList, int outputStoreIndexOutEntry)
        {
            // Hack: the outTuple has no left/right input tuple reference, use the left/right outList reference instead.
            foreach (OutTuple_ outTuple in outTupleList)
            {
                ElementAwareListEntry<OutTuple_> outEntry = (ElementAwareListEntry<OutTuple_>)outTuple.GetStore(outputStoreIndexOutEntry);
                ElementAwareList<OutTuple_> outEntryList = outEntry.GetList();
                if (outList == outEntryList)
                {
                    return outTuple;
                }
            }
            return null;
        }

        private void ProcessOutTupleUpdate(LeftTuple_ leftTuple, UniTuple<Right_> rightTuple, ElementAwareList<OutTuple_> outList, ElementAwareList<OutTuple_> outTupleList, int outputStoreIndexOutEntry)
        {
            OutTuple_ outTuple = FindOutTuple(outTupleList, outList, outputStoreIndexOutEntry);
            if (TestFiltering(leftTuple, rightTuple))
            {
                if (outTuple == null)
                {
                    InsertOutTuple(leftTuple, rightTuple);
                }
                else
                {
                    UpdateOutTupleLeft(outTuple, leftTuple);
                }
            }
            else
            {
                if (outTuple != null)
                {
                    RetractOutTuple(outTuple);
                }
            }
        }

        private void UpdateOutTupleLeft(OutTuple_ outTuple, LeftTuple_ leftTuple)
        {
            SetOutTupleLeftFacts(outTuple, leftTuple);
            DoUpdateOutTuple(outTuple);
        }

        private void DoUpdateOutTuple(OutTuple_ outTuple)
        {
            TupleState state = outTuple.State;
            if (!TupleStateHelper.IsActive(state))
            { // Impossible because they shouldn't linger in the indexes.
                throw new Exception("Impossible state: The tuple (" + outTuple.State + ") in node (" +
                        this + ") is in an unexpected state (" + outTuple.State + ").");
            }
            else if (state != TupleState.OK)
            { // Already in the queue in the correct state.
                return;
            }
            propagationQueue.Update(outTuple);
        }

        protected abstract void SetOutTupleLeftFacts(OutTuple_ outTuple, LeftTuple_ leftTuple);

        protected void RetractOutTuple(OutTuple_ outTuple)
        {
            ElementAwareListEntry<OutTuple_> outEntryLeft = (ElementAwareListEntry<OutTuple_>)outTuple.RemoveStore(outputStoreIndexLeftOutEntry);
            outEntryLeft.Remove();
            ElementAwareListEntry<OutTuple_> outEntryRight = (ElementAwareListEntry<OutTuple_>)outTuple.RemoveStore(outputStoreIndexRightOutEntry);
            outEntryRight.Remove();
            TupleState state = outTuple.State;
            if (!TupleStateHelper.IsActive(state))
            {
                // Impossible because they shouldn't linger in the indexes.
                throw new Exception("Impossible state: The tuple (" + outTuple.State + ") in node (" + this
                        + ") is in an unexpected state (" + outTuple.State + ").");
            }
            propagationQueue.Retract(outTuple, state == TupleState.CREATING ? TupleState.ABORTING : TupleState.DYING);
        }

        protected void InsertOutTupleFiltered(LeftTuple_ leftTuple, UniTuple<Right_> rightTuple)
        {
            if (!isFiltering || TestFiltering(leftTuple, rightTuple))
            {
                InsertOutTuple(leftTuple, rightTuple);
            }
        }

        public override Propagator GetPropagator()
        {
            return propagationQueue;
        }

        public abstract void InsertLeft(ITuple tuple);

        public abstract void RetractLeft(ITuple tuple);

        public abstract void UpdateLeft(ITuple tuple);

        public abstract void InsertRight(ITuple tuple);

        public abstract void RetractRight(ITuple tuple);

        public abstract void UpdateRight(ITuple tuple);
    }
}
