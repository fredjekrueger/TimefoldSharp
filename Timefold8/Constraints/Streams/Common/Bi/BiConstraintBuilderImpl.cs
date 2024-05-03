using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Bi
{
    public sealed class BiConstraintBuilderImpl<A, B> : AbstractConstraintBuilder<A, B>, BiConstraintBuilder<A, B>
    {

        public BiConstraintBuilderImpl(BiConstraintConstructor<A, B> constraintConstructor, ScoreImpactType impactType, API.Score.Score constraintWeight)
            : base((ConstraintConstructor<A, B>)constraintConstructor, impactType, constraintWeight)
        {

        }

        public BiConstraintBuilderImpl(Func<string, string, API.Score.Score, ScoreImpactType, object, object, Constraint> factory, ScoreImpactType impactType, API.Score.Score constraintWeight)
            : base(factory, impactType, constraintWeight)
        {

        }

        protected override IndictedObjectsMapping_ GetIndictedObjectsMapping<IndictedObjectsMapping_>()
        {
            return default(IndictedObjectsMapping_);
        }

        protected override JustificationMapping_ GetJustificationMapping<JustificationMapping_>()
        {
            return default(JustificationMapping_);
        }
    }
}
