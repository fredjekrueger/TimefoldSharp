using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public class BavetFilterBiConstraintStream<A, B> : BavetAbstractBiConstraintStream<A, B>
    {
        private Func<A, B, bool> predicate;

        public BavetFilterBiConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractBiConstraintStream<A, B> parent,
                Func<A, B, bool> predicate) : base(constraintFactory, parent)
        {
            this.predicate = predicate;
            if (predicate == null)
            {
                throw new Exception("The predicate (null) cannot be null.");
            }
        }

        public override string ToString()
        {
            return "Filter() with " + childStreamList.Count + " children";
        }

        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(parent, predicate);
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            else if (o is BavetFilterBiConstraintStream<A, B> other)
            {
                return parent == other.parent && predicate == other.predicate;
            }
            else
            {
                return false;
            }
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            buildHelper.PutInsertUpdateRetract(this, childStreamList, tupleLifecycle => new ConditionalBiTupleLifecycle<A, B>(predicate, tupleLifecycle));
        }
    }
}
