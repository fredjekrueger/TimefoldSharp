using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public abstract class AbstractConstraintBuilder<A, B> : ConstraintBuilder
    {

        private readonly ConstraintConstructor<A, B> constraintConstructor;
        private readonly ScoreImpactType impactType;
        private readonly API.Score.Score constraintWeight;
        Func<string, string, API.Score.Score, ScoreImpactType, object, object, Constraint> factory;

        protected AbstractConstraintBuilder(ConstraintConstructor<A, B> constraintConstructor, ScoreImpactType impactType,
            API.Score.Score constraintWeight)
        {
            this.constraintConstructor = constraintConstructor;
            this.impactType = impactType;
            this.constraintWeight = constraintWeight;
        }

        protected AbstractConstraintBuilder(Func<string, string, API.Score.Score, ScoreImpactType, object, object, Constraint> factory, ScoreImpactType impactType,
            API.Score.Score constraintWeight)
        {
            this.factory = factory;
            this.impactType = impactType;
            this.constraintWeight = constraintWeight;
        }


        protected abstract JustificationMapping_ GetJustificationMapping<JustificationMapping_>();

        protected abstract IndictedObjectsMapping_ GetIndictedObjectsMapping<IndictedObjectsMapping_>();

        public Constraint AsConstraint(string constraintName)
        {
            if (constraintConstructor != null)
                return constraintConstructor.Apply(null, constraintName, constraintWeight, impactType, GetJustificationMapping<A>(), GetIndictedObjectsMapping<B>());
            else
                return factory(null, constraintName, constraintWeight, impactType, GetJustificationMapping<A>(), GetIndictedObjectsMapping<B>());
        }

        public Constraint AsConstraint(string constraintPackage, string constraintName)
        {
            throw new NotImplementedException();
        }
    }
}
