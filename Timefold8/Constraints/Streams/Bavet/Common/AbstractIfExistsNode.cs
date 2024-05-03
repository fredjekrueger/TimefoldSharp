using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class AbstractIfExistsNode<LeftTuple_, Right_> : AbstractNode, LeftTupleLifecycle, RightTupleLifecycle
        where LeftTuple_ : AbstractTuple
    {
        protected readonly bool shouldExist;

        protected readonly int inputStoreIndexLeftTrackerList; // -1 if !isFiltering
        protected readonly int inputStoreIndexRightTrackerList; // -1 if !isFiltering
        private readonly DynamicPropagationQueue<LeftTuple_, ExistsCounter<LeftTuple_>> propagationQueue;
        protected readonly bool isFiltering;

        protected AbstractIfExistsNode(bool shouldExist, int inputStoreIndexLeftTrackerList, int inputStoreIndexRightTrackerList, TupleLifecycle nextNodesTupleLifecycle, bool isFiltering)
        {
            this.shouldExist = shouldExist;
            this.inputStoreIndexLeftTrackerList = inputStoreIndexLeftTrackerList;
            this.inputStoreIndexRightTrackerList = inputStoreIndexRightTrackerList;
            this.isFiltering = isFiltering;
            this.propagationQueue = new DynamicPropagationQueue<LeftTuple_, ExistsCounter<LeftTuple_>>(nextNodesTupleLifecycle);
        }

        public abstract void InsertLeft(ITuple tuple);

        public abstract void UpdateLeft(ITuple tuple);

        public abstract void RetractLeft(ITuple tuple);

        public abstract void InsertRight(ITuple tuple);

        public abstract void RetractRight(ITuple tuple);

        public void UpdateRight(ITuple tuple)
        {
            throw new NotImplementedException();
        }
    }
}
