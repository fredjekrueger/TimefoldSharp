using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public abstract class AbstractIndexedJoinNode<LeftTuple_, Right_, OutTuple_>
        : AbstractJoinNode<LeftTuple_, Right_, OutTuple_>, LeftTupleLifecycle, RightTupleLifecycle
        where LeftTuple_ : AbstractTuple
        where OutTuple_ : AbstractTuple
    {

        private readonly Func<Right_, IndexProperties> mappingRight;
        private readonly int inputStoreIndexLeftProperties;
        private readonly int inputStoreIndexLeftEntry;
        private readonly int inputStoreIndexRightProperties;
        private readonly int inputStoreIndexRightEntry;

        private readonly Indexer<LeftTuple_> indexerLeft;
        private readonly Indexer<UniTuple<Right_>> indexerRight;

        protected AbstractIndexedJoinNode(Func<Right_, IndexProperties> mappingRight, int inputStoreIndexLeftProperties,
            int inputStoreIndexLeftEntry, int inputStoreIndexLeftOutTupleList, int inputStoreIndexRightProperties,
            int inputStoreIndexRightEntry, int inputStoreIndexRightOutTupleList,
            TupleLifecycle nextNodesTupleLifecycle, bool isFiltering, int outputStoreIndexLeftOutEntry,
            int outputStoreIndexRightOutEntry, Indexer<LeftTuple_> indexerLeft, Indexer<UniTuple<Right_>> indexerRight)
            : base(inputStoreIndexLeftOutTupleList, inputStoreIndexRightOutTupleList, nextNodesTupleLifecycle, isFiltering,
                    outputStoreIndexLeftOutEntry, outputStoreIndexRightOutEntry)
        {
            this.mappingRight = mappingRight;
            this.inputStoreIndexLeftProperties = inputStoreIndexLeftProperties;
            this.inputStoreIndexLeftEntry = inputStoreIndexLeftEntry;
            this.inputStoreIndexRightProperties = inputStoreIndexRightProperties;
            this.inputStoreIndexRightEntry = inputStoreIndexRightEntry;
            this.indexerLeft = indexerLeft;
            this.indexerRight = indexerRight;
        }

        protected abstract IndexProperties CreateIndexPropertiesLeft(ITuple leftTuple);

        public override void RetractRight(ITuple rightTuple)
        {
            IndexProperties indexProperties = (IndexProperties)rightTuple.RemoveStore(inputStoreIndexRightProperties);
            if (indexProperties == null)
            {
                // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
                return;
            }
            ElementAwareListEntry<UniTuple<Right_>> rightEntry = (ElementAwareListEntry<UniTuple<Right_>>)rightTuple.RemoveStore(inputStoreIndexRightEntry);
            ElementAwareList<OutTuple_> outTupleListRight = (ElementAwareList<OutTuple_>)rightTuple.RemoveStore(inputStoreIndexRightOutTupleList);
            indexerRight.Remove(indexProperties, rightEntry);
            outTupleListRight.ForEach(o => RetractOutTuple((OutTuple_)o));
        }

        public override void UpdateRight(ITuple rt)
        {
            var rightTuple = (UniTuple<Right_>)rt;
            var oldIndexProperties = rightTuple.GetStore<IndexProperties>(inputStoreIndexRightProperties);
            if (oldIndexProperties == null)
            {
                // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
                InsertRight(rightTuple);
                return;
            }
            IndexProperties newIndexProperties = mappingRight.Invoke(rightTuple.factA);
            if (oldIndexProperties.Equals(newIndexProperties))
            {
                // No need for re-indexing because the index properties didn't change
                // Prefer an update over retract-insert if possible
                InnerUpdateRight(rightTuple, consumer => indexerLeft.ForEach(oldIndexProperties, consumer));
            }
            else
            {
                var rightEntry = (ElementAwareListEntry<UniTuple<Right_>>)rightTuple.GetStore<ElementAwareListEntry<UniTuple<Right_>>>(inputStoreIndexRightEntry);
                var outTupleListRight = (ElementAwareList<OutTuple_>)rightTuple.GetStore<ElementAwareList<OutTuple_>>(inputStoreIndexRightOutTupleList);
                indexerRight.Remove(oldIndexProperties, rightEntry);
                outTupleListRight.ForEach(RetractOutTuple);
                // outTupleListRight is now empty
                // No need for rightTuple.setStore(inputStoreIndexRightOutTupleList, outTupleListRight);
                IndexAndPropagateRight(rightTuple, newIndexProperties);
            }
        }


        public override void InsertLeft(ITuple leftTuple)
        {
            var left = (LeftTuple_)leftTuple;
            IndexProperties indexProperties = CreateIndexPropertiesLeft(left);

            ElementAwareList<OutTuple_> outTupleListLeft = new ElementAwareList<OutTuple_>();
            left.SetStore(inputStoreIndexLeftOutTupleList, outTupleListLeft);
            IndexAndPropagateLeft(left, indexProperties);
        }

        public override void InsertRight(ITuple rightTuple)
        {
            var right = (UniTuple<Right_>)rightTuple;
            IndexProperties indexProperties = mappingRight.Invoke((Right_)right.factA);

            ElementAwareList<OutTuple_> outTupleListRight = new ElementAwareList<OutTuple_>();
            rightTuple.SetStore(inputStoreIndexRightOutTupleList, outTupleListRight);
            IndexAndPropagateRight(right, indexProperties);
        }

        private void IndexAndPropagateRight(UniTuple<Right_> rightTuple, IndexProperties indexProperties)
        {
            rightTuple.SetStore(inputStoreIndexRightProperties, indexProperties);
            ElementAwareListEntry<UniTuple<Right_>> rightEntry = indexerRight.Put(indexProperties, rightTuple);
            rightTuple.SetStore(inputStoreIndexRightEntry, rightEntry);
            indexerLeft.ForEach(indexProperties, leftTuple => InsertOutTupleFiltered((LeftTuple_)leftTuple, rightTuple));
        }

        private void IndexAndPropagateLeft(LeftTuple_ leftTuple, IndexProperties indexProperties)
        {
            leftTuple.SetStore(inputStoreIndexLeftProperties, indexProperties);
            ElementAwareListEntry<LeftTuple_> leftEntry = indexerLeft.Put(indexProperties, leftTuple);
            leftTuple.SetStore(inputStoreIndexLeftEntry, leftEntry);
            indexerRight.ForEach(indexProperties, rightTuple => InsertOutTupleFiltered(leftTuple, rightTuple));
        }


        public override void RetractLeft(ITuple leftTuple)
        {
            IndexProperties indexProperties = (IndexProperties)leftTuple.RemoveStore(inputStoreIndexLeftProperties);
            if (indexProperties == null)
            {
                // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
                return;
            }
            ElementAwareListEntry<LeftTuple_> leftEntry = (ElementAwareListEntry<LeftTuple_>)leftTuple.RemoveStore(inputStoreIndexLeftEntry);
            ElementAwareList<OutTuple_> outTupleListLeft = (ElementAwareList<OutTuple_>)leftTuple.RemoveStore(inputStoreIndexLeftOutTupleList);
            indexerLeft.Remove(indexProperties, leftEntry);
            outTupleListLeft.ForEach(o => RetractOutTuple(o));
        }

        public override void UpdateLeft(ITuple leftTuple)
        {
            var oldIndexProperties = leftTuple.GetStore<IndexProperties>(inputStoreIndexLeftProperties);
            if (oldIndexProperties == null)
            {
                // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
                InsertLeft(leftTuple);
                return;
            }
            IndexProperties newIndexProperties = CreateIndexPropertiesLeft(leftTuple);
            if (oldIndexProperties.Equals(newIndexProperties))
            {
                // No need for re-indexing because the index properties didn't change
                // Prefer an update over retract-insert if possible
                InnerUpdateLeft((LeftTuple_)leftTuple, c => indexerRight.ForEach(oldIndexProperties, c));
            }
            else
            {
                var leftEntry = leftTuple.GetStore<ElementAwareListEntry<LeftTuple_>>(inputStoreIndexLeftEntry);
                var outTupleListLeft = leftTuple.GetStore<ElementAwareList<OutTuple_>>(inputStoreIndexLeftOutTupleList);
                indexerLeft.Remove(oldIndexProperties, leftEntry);
                outTupleListLeft.ForEach(RetractOutTuple);
                // outTupleListLeft is now empty
                // No need for leftTuple.setStore(inputStoreIndexLeftOutTupleList, outTupleListLeft);
                IndexAndPropagateLeft((LeftTuple_)leftTuple, newIndexProperties);
            }
        }
    }
}
