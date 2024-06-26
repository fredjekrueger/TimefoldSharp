using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public sealed class ExistsCounter<Tuple_> : AbstractPropagationMetadataCarrier<Tuple_>
    where Tuple_ : AbstractTuple
    {
        readonly Tuple_ leftTuple;
        TupleState state = TupleState.DEAD; // It's the node's job to mark a new instance as CREATING.
        int countRight = 0;

        public ExistsCounter(Tuple_ leftTuple)
        {
            this.leftTuple = leftTuple;
        }

        public override TupleState GetState()
        {
            return state;
        }

        public override Tuple_ GetTuple()
        {
            return leftTuple;
        }

        public override void SetState(TupleState state)
        {
            this.state = state;
        }
        public override string ToString()
        {
            return "Counter(" + leftTuple + ")";
        }
    }
}
