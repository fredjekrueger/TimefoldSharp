using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public abstract class AbstractConstraint : Constraint
    {

        private readonly ConstraintFactory constraintFactory;
        private readonly string constraintPackage;
        private readonly string constraintName;
        private readonly string constraintId;
        private readonly Func<ISolution, Score> constraintWeightExtractor;
        private readonly ScoreImpactType scoreImpactType;
        private readonly bool isConstraintWeightConfigurable;
        // Constraint is not generic in uni/bi/..., therefore these can not be typed.
        private readonly object justificationMapping;
        private readonly object indictedObjectsMapping;

        protected AbstractConstraint(ConstraintFactory constraintFactory, String constraintPackage, String constraintName,
         Func<ISolution, Score> constraintWeightExtractor, ScoreImpactType scoreImpactType,
         bool isConstraintWeightConfigurable, Object justificationMapping, Object indictedObjectsMapping)
        {
            this.constraintFactory = constraintFactory;
            this.constraintPackage = constraintPackage;
            this.constraintName = constraintName;
            this.constraintId = constraintPackage + "/" + constraintName;
            this.constraintWeightExtractor = constraintWeightExtractor;
            this.scoreImpactType = scoreImpactType;
            this.isConstraintWeightConfigurable = isConstraintWeightConfigurable;
            this.justificationMapping = justificationMapping;
            this.indictedObjectsMapping = indictedObjectsMapping;
        }

        public Score ExtractConstraintWeight(ISolution workingSolution)
        {
            if (isConstraintWeightConfigurable && workingSolution == null)
            {
                /*
                 * In constraint verifier API, we allow for testing constraint providers without having a planning solution.
                 * However, constraint weights may be configurable and in that case the solution is required to read the
                 * weights from.
                 * For these cases, we set the constraint weight to the softest possible value, just to make sure that the
                 * constraint is not ignored.
                 * The actual value is not used in any way.
                 */
                return constraintFactory.GetSolutionDescriptor().GetScoreDefinition().GetOneSoftestScore();
            }
            var constraintWeight = constraintWeightExtractor(workingSolution);

            switch (scoreImpactType)
            {
                case ScoreImpactType.PENALTY:
                    return constraintWeight.Negate();
                case ScoreImpactType.REWARD:
                    return constraintWeight;
                case ScoreImpactType.MIXED:
                    return constraintWeight;
            };
            throw new NotImplementedException();
        }

        public void AssertCorrectImpact(int impact)
        {
            if (impact >= 0)
            {
                return;
            }
            if (scoreImpactType != ScoreImpactType.MIXED)
            {
                throw new Exception("Negative match weight (" + impact + ") for constraint ("
                        + GetConstraintId() + "). " +
                        "Check constraint provider implementation.");
            }
        }

        public ConstraintFactory GetConstraintFactory()
        {
            return constraintFactory;
        }

        public JustificationMapping_ GetJustificationMapping<JustificationMapping_>()
        {
            return (JustificationMapping_)justificationMapping;
        }

        public IndictedObjectsMapping_ GetIndictedObjectsMapping<IndictedObjectsMapping_>()
        {
            return (IndictedObjectsMapping_)indictedObjectsMapping;
        }

        public string GetConstraintName()
        {
            return constraintName;
        }

        public string GetConstraintId()
        {
            return constraintId;
        }
    }
}