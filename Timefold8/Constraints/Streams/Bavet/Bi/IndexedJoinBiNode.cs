using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public class IndexedJoinBiNode<A, B> : AbstractIndexedJoinNode<UniTuple<A>, B, BiTuple<A, B>>
    {

        private readonly Func<A, IndexProperties> mappingA;
        private readonly Func<A, B, bool> filtering;
        private readonly int outputStoreSize;

        public IndexedJoinBiNode(Func<A, IndexProperties> mappingA, Func<B, IndexProperties> mappingB,
                int inputStoreIndexA, int inputStoreIndexEntryA, int inputStoreIndexOutTupleListA,
                int inputStoreIndexB, int inputStoreIndexEntryB, int inputStoreIndexOutTupleListB,
                TupleLifecycle nextNodesTupleLifecycle, Func<A, B, bool> filtering,
                int outputStoreSize,
                int outputStoreIndexOutEntryA, int outputStoreIndexOutEntryB,
                Indexer<UniTuple<A>> indexerA,
                Indexer<UniTuple<B>> indexerB)
            : base(mappingB,
                    inputStoreIndexA, inputStoreIndexEntryA, inputStoreIndexOutTupleListA,
                    inputStoreIndexB, inputStoreIndexEntryB, inputStoreIndexOutTupleListB,
                    nextNodesTupleLifecycle, filtering != null,
                    outputStoreIndexOutEntryA, outputStoreIndexOutEntryB,
                    indexerA, indexerB)
        {

            this.mappingA = mappingA;
            this.filtering = filtering;
            this.outputStoreSize = outputStoreSize;
        }

        protected override IndexProperties CreateIndexPropertiesLeft(ITuple leftTuple)
        {
            return mappingA.Invoke((A)((UniTuple<A>)leftTuple).factA);
        }

        protected override BiTuple<A, B> CreateOutTuple(UniTuple<A> leftTuple, UniTuple<B> rightTuple)
        {
            return new BiTuple<A, B>((A)leftTuple.factA, (B)rightTuple.factA, outputStoreSize);
        }

        protected override void SetOutTupleLeftFacts(BiTuple<A, B> outTuple, UniTuple<A> leftTuple)
        {
            outTuple.factA = (A)leftTuple.factA;
        }

        protected override void SetOutTupleRightFact(BiTuple<A, B> outTuple, UniTuple<B> rightTuple)
        {
            outTuple.factB = (B)rightTuple.factA;
        }

        protected override bool TestFiltering(UniTuple<A> leftTuple, UniTuple<B> rightTuple)
        {
            return filtering.Invoke((A)leftTuple.factA, (B)rightTuple.factA);
        }
    }
}
