namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public interface BavetScoringConstraintStream
    {
        void SetConstraint(BavetConstraint constraint);

        void CollectActiveConstraintStreams(HashSet<BavetAbstractConstraintStream> constraintStreamSet);
    }
}
