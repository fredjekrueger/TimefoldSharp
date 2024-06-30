using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public sealed class BavetForEachUniConstraintStream<A> : BavetAbstractUniConstraintStream<A>, TupleSource
    {
        private readonly Func<A, bool> filter;

        public BavetForEachUniConstraintStream(BavetConstraintFactory constraintFactory, Func<A, bool> filter, RetrievalSemantics retrievalSemantics) 
            : base(constraintFactory, retrievalSemantics)
        {

            this.filter = filter;
        }

        public override void CollectActiveConstraintStreams(HashSet<BavetAbstractConstraintStream> constraintStreamSet)
        {
            constraintStreamSet.Add(this);
        }

        public override string ToString()
        {
            if (filter != null)
            {
                return "ForEach(" + nameof(A)+ ") with filter and " + childStreamList.Count + " children";
            }
            return "ForEach(" + nameof(A) + ") with " + childStreamList.Count + " children";
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            TupleLifecycle tupleLifecycle = buildHelper.GetAggregatedTupleLifecycle(childStreamList);
            int outputStoreSize = buildHelper.ExtractTupleStoreSize(this);
            if (filter == null)
                buildHelper.AddNode(new ForEachIncludingNullVarsUniNode<A>(tupleLifecycle, outputStoreSize), this, null);
            else
                buildHelper.AddNode(new ForEachExcludingNullVarsUniNode<A>(filter, tupleLifecycle, outputStoreSize), this, null);

        }

        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(typeof(A), filter);
        }

        public override bool Equals(object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }
            BavetForEachUniConstraintStream<A> that = (BavetForEachUniConstraintStream<A>)other;
            return ((filter == null && that.filter == null) || filter.Equals(that.filter));
        }

    }
}
