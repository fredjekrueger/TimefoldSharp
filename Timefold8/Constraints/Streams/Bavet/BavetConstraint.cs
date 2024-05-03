using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Common;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet
{
    public sealed class BavetConstraint : AbstractConstraint

    {

        private readonly BavetScoringConstraintStream scoringConstraintStream;

        public BavetConstraint(BavetConstraintFactory constraintFactory, String constraintPackage,
           String constraintName, Func<ISolution, Score> constraintWeightExtractor,
           ScoreImpactType scoreImpactType, Object justificationMapping, object indictedObjectsMapping, bool isConstraintWeightConfigurable, BavetScoringConstraintStream scoringConstraintStream)
            : base(constraintFactory, constraintPackage, constraintName, constraintWeightExtractor, scoreImpactType, isConstraintWeightConfigurable, justificationMapping, indictedObjectsMapping)
        {
            this.scoringConstraintStream = scoringConstraintStream;
        }

        public void CollectActiveConstraintStreams(HashSet<BavetAbstractConstraintStream> constraintStreamSet)
        {
            scoringConstraintStream.CollectActiveConstraintStreams(constraintStreamSet);
        }
        public override string ToString()
        {
            return "BavetConstraint(" + GetConstraintId() + ")";
        }

    }
}
