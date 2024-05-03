using System.Numerics;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public abstract class BavetAbstractBiConstraintStream<A, B> : BavetAbstractConstraintStream, InnerBiConstraintStream<A, B>
    {
        protected BavetAbstractBiConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractConstraintStream parent)
            : base(constraintFactory, parent)
        {

        }

        protected BavetAbstractBiConstraintStream(BavetConstraintFactory constraintFactory, RetrievalSemantics retrievalSemantics)
            : base(constraintFactory, retrievalSemantics)
        {

        }

        public BiConstraintBuilder<A, B> Penalize(Score constraintWeight)
        {
            return Penalize(constraintWeight, (a, b) => 1);
        }

        public BiConstraintBuilder<A, B> Penalize(Score constraintWeight, Func<A, B, int> matchWeigher)
        {
            return InnerImpact(constraintWeight, matchWeigher, ScoreImpactType.PENALTY);
        }


        public BiConstraintBuilder<A, B> InnerImpact(Score constraintWeight, Func<A, B, int> matchWeigher, ScoreImpactType scoreImpactType)
        {
            var stream = ShareAndAddChild(new BavetScoringBiConstraintStream<A, B>(constraintFactory, this, matchWeigher));
            return NewTerminator(stream, scoreImpactType, constraintWeight);
        }



        /*public BiConstraintBuilder<A, B> Penalize(IAbstractScore constraintWeight)
        {
            return Penalize(constraintWeight, (a, b)=> 1);
        }*/



        public BiConstraintBuilder<A, B> InnerImpact(Score constraintWeight, Func<A, B, BigInteger> matchWeigher, ScoreImpactType scoreImpactType)
        {
            var stream = ShareAndAddChild(new BavetScoringBiConstraintStream<A, B>(constraintFactory, this, matchWeigher));
            return NewTerminator(stream, scoreImpactType, constraintWeight);
        }

        private BiConstraintBuilderImpl<A, B> NewTerminator(BavetScoringConstraintStream stream, ScoreImpactType impactType, Score constraintWeight)
        {
            return new BiConstraintBuilderImpl<A, B>(
                    (constraintPackage, constraintName, constraintWeight_, impactType_, justificationMapping, indictedObjectsMapping)
                    => BuildConstraint(constraintPackage, constraintName, constraintWeight_, impactType_, justificationMapping, indictedObjectsMapping, stream),
                    impactType, constraintWeight);
        }

        protected override IndictedObjectsMapping_ GetDefaultIndictedObjectsMapping<IndictedObjectsMapping_>()
        {
            return default(IndictedObjectsMapping_);
        }

        protected override JustificationMapping_ GetDefaultJustificationMapping<JustificationMapping_>()
        {
            return default(JustificationMapping_);
        }

        public BiConstraintStream<A, B> Filter(Func<A, B, bool> predicate)
        {
            return ShareAndAddChild(new BavetFilterBiConstraintStream<A, B>(constraintFactory, this, predicate));
        }

        public BiConstraintBuilder<A, B> Reward(Score constraintWeight, Func<A, B, int> matchWeigher)
        {
            return InnerImpact(constraintWeight, matchWeigher, ScoreImpactType.REWARD);
        }

        public BiConstraintBuilder<A, B> Reward(Score constraintWeight)
        {
            return Reward(constraintWeight, (a, b) => 1);
        }
    }
}
