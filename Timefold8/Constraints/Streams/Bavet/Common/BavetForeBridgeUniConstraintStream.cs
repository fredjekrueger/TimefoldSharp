using TimefoldSharp.Core.Constraints.Streams.Bavet.Uni;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public sealed class BavetForeBridgeUniConstraintStream<A> : BavetAbstractUniConstraintStream<A>
    {
        public BavetForeBridgeUniConstraintStream(BavetConstraintFactory constraintFactory,
           BavetAbstractUniConstraintStream<A> parent) : base(constraintFactory, parent)
        {

        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            //do nothing
        }

        public override string ToString()
        {
            return "Generic bridge";
        }
    }
}
