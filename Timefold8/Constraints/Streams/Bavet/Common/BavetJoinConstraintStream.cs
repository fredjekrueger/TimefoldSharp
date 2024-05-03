namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public interface BavetJoinConstraintStream : TupleSource
    {
        BavetAbstractConstraintStream GetLeftParent();
        BavetAbstractConstraintStream GetRightParent();
    }
}
