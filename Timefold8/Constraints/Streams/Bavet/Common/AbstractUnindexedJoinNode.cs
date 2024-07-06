using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class AbstractUnindexedJoinNode<LeftTuple_, Right_, OutTuple_>
        : AbstractJoinNode<LeftTuple_, Right_, OutTuple_>, LeftTupleLifecycle, RightTupleLifecycle
        where LeftTuple_ : AbstractTuple
        where OutTuple_ : AbstractTuple
    {

        private readonly int inputStoreIndexLeftEntry;
        private readonly int inputStoreIndexRightEntry;
        private readonly ElementAwareList<LeftTuple_> leftTupleList = new ElementAwareList<LeftTuple_>();
        private readonly ElementAwareList<UniTuple<Right_>> rightTupleList = new ElementAwareList<UniTuple<Right_>>();

        protected AbstractUnindexedJoinNode(int inputStoreIndexLeftEntry, int inputStoreIndexLeftOutTupleList,
                int inputStoreIndexRightEntry, int inputStoreIndexRightOutTupleList,
                TupleLifecycle nextNodesTupleLifecycle, bool isFiltering, int outputStoreIndexLeftOutEntry,
                int outputStoreIndexRightOutEntry)
                : base(inputStoreIndexLeftOutTupleList, inputStoreIndexRightOutTupleList, nextNodesTupleLifecycle, isFiltering,
                    outputStoreIndexLeftOutEntry, outputStoreIndexRightOutEntry)
        {

            this.inputStoreIndexLeftEntry = inputStoreIndexLeftEntry;
            this.inputStoreIndexRightEntry = inputStoreIndexRightEntry;
        }

        public override void InsertLeft(ITuple tuple)
        {
            throw new NotImplementedException();
        }

        public override void RetractRight(ITuple rightTuple)
        {
            ElementAwareListEntry<Right_> rightEntry = (ElementAwareListEntry<Right_>)rightTuple.RemoveStore(inputStoreIndexRightEntry);
            if (rightEntry == null)
            {
                // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
                return;
            }
            ElementAwareList<OutTuple_> outTupleListRight = (ElementAwareList<OutTuple_>)rightTuple.RemoveStore(inputStoreIndexRightOutTupleList);
            rightEntry.Remove();
            outTupleListRight.ForEach(o => RetractOutTuple((OutTuple_)o));
        }

        public override void RetractLeft(ITuple tuple)
        {
            throw new NotImplementedException();
        }

        public override void UpdateLeft(ITuple tuple)
        {
            throw new NotImplementedException();
        }

        public override void InsertRight(ITuple rt)
        {
            UniTuple<Right_> rightTuple = (UniTuple<Right_>)rt;
            var rightEntry = rightTupleList.Add(rightTuple);
            rightTuple.SetStore(inputStoreIndexRightEntry, rightEntry);
            var outTupleListRight = new ElementAwareList<OutTuple_>();
            rightTuple.SetStore(inputStoreIndexRightOutTupleList, outTupleListRight);
            leftTupleList.ForEach(leftTuple => InsertOutTupleFiltered(leftTuple, rightTuple));
        }
    }
}
