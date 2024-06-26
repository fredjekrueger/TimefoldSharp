using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class AbstractPropagationMetadataCarrier<Tuple_>
        where Tuple_ : AbstractTuple
    {
        public int PositionInDirtyList = -1;

        public abstract Tuple_ GetTuple();
        public abstract TupleState GetState();
        public abstract void SetState(TupleState state);
    }
}
