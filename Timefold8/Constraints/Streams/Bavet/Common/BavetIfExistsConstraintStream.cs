namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public interface BavetIfExistsConstraintStream
    {
        BavetAbstractConstraintStream GetLeftParent();
        BavetAbstractConstraintStream GetRightParent();
    }
}
