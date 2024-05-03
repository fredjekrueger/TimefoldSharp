using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public class UnindexedJoinBiNode<A, B> : AbstractUnindexedJoinNode<UniTuple<A>, B, BiTuple<A, B>>
    {
        private readonly Func<A, B, bool> filtering;
        private readonly int outputStoreSize;

        public UnindexedJoinBiNode(
                int inputStoreIndexLeftEntry, int inputStoreIndexLeftOutTupleList,
                int inputStoreIndexRightEntry, int inputStoreIndexRightOutTupleList,
                TupleLifecycle nextNodesTupleLifecycle, Func<A, B, bool> filtering,
                int outputStoreSize,
                int outputStoreIndexLeftOutEntry, int outputStoreIndexRightOutEntry)
            : base(inputStoreIndexLeftEntry, inputStoreIndexLeftOutTupleList,
                    inputStoreIndexRightEntry, inputStoreIndexRightOutTupleList,
                    nextNodesTupleLifecycle, filtering != null,
                    outputStoreIndexLeftOutEntry, outputStoreIndexRightOutEntry)
        {

            this.filtering = filtering;
            this.outputStoreSize = outputStoreSize;
        }

        public override Propagator GetPropagator()
        {
            throw new NotImplementedException();
        }

        public override void UpdateRight(ITuple tuple)
        {
            throw new NotImplementedException();
        }

        protected override BiTuple<A, B> CreateOutTuple(UniTuple<A> leftTuple, UniTuple<B> rightTuple)
        {
            throw new NotImplementedException();
        }

        protected override void SetOutTupleLeftFacts(BiTuple<A, B> outTuple, UniTuple<A> leftTuple)
        {
            throw new NotImplementedException();
        }

        protected override void SetOutTupleRightFact(BiTuple<A, B> outTuple, UniTuple<B> rightTuple)
        {
            throw new NotImplementedException();
        }

        protected override bool TestFiltering(UniTuple<A> leftTuple, UniTuple<B> rightTuple)
        {
            throw new NotImplementedException();
        }
    }
}
