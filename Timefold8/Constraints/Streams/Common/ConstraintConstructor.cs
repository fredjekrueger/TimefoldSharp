using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public interface ConstraintConstructor<JustificationMapping_, IndictedObjectsMapping_>
    {
        Constraint Apply(String constraintPackage, String constraintName, Score constraintWeight,
           ScoreImpactType impactType, JustificationMapping_ justificationMapping,
           IndictedObjectsMapping_ indictedObjectsMapping);
    }
}
