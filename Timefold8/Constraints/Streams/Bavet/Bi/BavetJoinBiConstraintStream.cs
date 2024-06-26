using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public sealed class BavetJoinBiConstraintStream<A, B> : BavetAbstractBiConstraintStream<A, B>, BavetJoinConstraintStream
    {

        private BavetForeBridgeUniConstraintStream<A> LeftParent { get; set; }
        private BavetForeBridgeUniConstraintStream<B> RightParent { get; set; }
        private readonly DefaultBiJoiner<A, B> joiner;
        private readonly Func<A, B, bool> filtering;

        public BavetJoinBiConstraintStream(BavetConstraintFactory constraintFactory, BavetForeBridgeUniConstraintStream<A> leftParent,
           BavetForeBridgeUniConstraintStream<B> rightParent, DefaultBiJoiner<A, B> joiner, Func<A, B, bool> filtering) :
            base(constraintFactory, leftParent.GetRetrievalSemantics())
        {

            this.LeftParent = leftParent;
            this.RightParent = rightParent;
            this.joiner = joiner;
            this.filtering = filtering;
        }

        public override string ToString()
        {
            return "BiJoin() with " + childStreamList.Count + " children";
        }

        public override void CollectActiveConstraintStreams(HashSet<BavetAbstractConstraintStream> constraintStreamSet)
        {
            LeftParent.CollectActiveConstraintStreams(constraintStreamSet);
            RightParent.CollectActiveConstraintStreams(constraintStreamSet);
            constraintStreamSet.Add(this);
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            int outputStoreSize = buildHelper.ExtractTupleStoreSize(this);
            TupleLifecycle downstream = buildHelper.GetAggregatedTupleLifecycle(childStreamList.OfType<ConstraintStream>().ToList());
            IndexerFactory indexerFactory = new IndexerFactory(joiner);
            if (indexerFactory.HasJoiners())
            {
                var node =
                         new IndexedJoinBiNode<A, B>(
                                JoinerUtils.CombineLeftMappings(joiner), JoinerUtils.CombineRightMappings(joiner),
                                buildHelper.ReserveTupleStoreIndex(LeftParent.GetTupleSource()),
                                buildHelper.ReserveTupleStoreIndex(LeftParent.GetTupleSource()),
                                buildHelper.ReserveTupleStoreIndex(LeftParent.GetTupleSource()),
                                buildHelper.ReserveTupleStoreIndex(RightParent.GetTupleSource()),
                                buildHelper.ReserveTupleStoreIndex(RightParent.GetTupleSource()),
                                buildHelper.ReserveTupleStoreIndex(RightParent.GetTupleSource()),
                                downstream, filtering, outputStoreSize + 2,
                                outputStoreSize, outputStoreSize + 1,
                                indexerFactory.BuildIndexer<UniTuple<A>>(true), indexerFactory.BuildIndexer<UniTuple<B>>(false));
                buildHelper.AddNode(node, this, LeftParent, RightParent);
            }
            else
            {
                var node = new UnindexedJoinBiNode<A, B>(
                        buildHelper.ReserveTupleStoreIndex(LeftParent.GetTupleSource()),
                        buildHelper.ReserveTupleStoreIndex(LeftParent.GetTupleSource()),
                        buildHelper.ReserveTupleStoreIndex(RightParent.GetTupleSource()),
                        buildHelper.ReserveTupleStoreIndex(RightParent.GetTupleSource()),
                        downstream, filtering, outputStoreSize + 2,
                        outputStoreSize, outputStoreSize + 1);
                buildHelper.AddNode(node, this, LeftParent, RightParent);
            }
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (o == null || GetType() != o.GetType())
            {
                return false;
            }
            BavetJoinBiConstraintStream<A, B> other = (BavetJoinBiConstraintStream<A, B>)o;
            /*
             * Bridge streams do not implement equality because their equals() would have to point back to this stream,
             * resulting in StackOverflowError.
             * Therefore we need to check bridge parents to see where this join node comes from.
             */
            return (LeftParent.GetParent().Equals(other.LeftParent.GetParent())
                    && RightParent.GetParent().Equals(other.RightParent.GetParent())
                    && joiner.Equals(other.joiner)
                    && filtering.Equals(other.filtering));
        }

        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(LeftParent.GetParent(), RightParent.GetParent(), joiner, filtering);
        }

        public BavetAbstractConstraintStream GetLeftParent()
        {
            return LeftParent;
        }

        public BavetAbstractConstraintStream GetRightParent()
        {
            return RightParent;
        }
    }
}
