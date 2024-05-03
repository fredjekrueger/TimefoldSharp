using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public sealed class BavetForEachUniConstraintStream<A> : BavetAbstractUniConstraintStream<A>, TupleSource
    {

        private readonly Type forEachClass;

        private readonly Func<A, bool> filter;

        public BavetForEachUniConstraintStream(BavetConstraintFactory constraintFactory, Type forEachClass,
            Func<A, bool> filter, RetrievalSemantics retrievalSemantics) : base(constraintFactory, retrievalSemantics)
        {

            this.forEachClass = forEachClass;
            if (forEachClass == null)
            {
                throw new Exception("The forEachClass (null) cannot be null.");
            }
            this.filter = filter;
        }

        public override void CollectActiveConstraintStreams(HashSet<BavetAbstractConstraintStream> constraintStreamSet)
        {
            constraintStreamSet.Add(this); //JDEF ADDIFNOTEXIST
        }

        public override string ToString()
        {
            if (filter != null)
            {
                return "ForEach(" + forEachClass.Name + ") with filter and " + childStreamList.Count() + " children";
            }
            return "ForEach(" + forEachClass.Name + ") with " + childStreamList.Count() + " children";
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            TupleLifecycle tupleLifecycle = buildHelper.GetAggregatedTupleLifecycle(childStreamList);
            int outputStoreSize = buildHelper.ExtractTupleStoreSize(this);
            if (filter == null)
                buildHelper.AddNode(new ForEachIncludingNullVarsUniNode<A>(forEachClass, tupleLifecycle, outputStoreSize), this, null);
            else
                buildHelper.AddNode(new ForEachExcludingNullVarsUniNode<A>(forEachClass, filter, tupleLifecycle, outputStoreSize), this, null);

        }

        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(forEachClass, filter);
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
            return forEachClass.Equals(that.forEachClass) && filter.Equals(that.filter);
        }

    }
}
