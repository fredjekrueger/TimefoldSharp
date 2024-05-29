using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Bi;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;
using TimefoldSharp.Core.Constraints.Streams.Common.Uni;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public abstract class BavetAbstractUniConstraintStream<A> : BavetAbstractConstraintStream, InnerUniConstraintStream<A>
    {
        protected BavetAbstractUniConstraintStream(BavetConstraintFactory constraintFactory, RetrievalSemantics retrievalSemantics) : base(constraintFactory, retrievalSemantics)
        {

        }

        protected BavetAbstractUniConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractConstraintStream parent) : base(constraintFactory, parent)
        {

        }

        public BiConstraintStream<A, B> Join<B>(UniConstraintStream<B> otherStream, BiJoinerComber<A, B> joinerComber)
        {
            var other = (BavetAbstractUniConstraintStream<B>)otherStream;
            var leftBridge = new BavetForeBridgeUniConstraintStream<A>(constraintFactory, this);
            var rightBridge = new BavetForeBridgeUniConstraintStream<B>(constraintFactory, other);
            var joinStream = new BavetJoinBiConstraintStream<A, B>(constraintFactory, leftBridge, rightBridge,
                    joinerComber.GetMergedJoiner(), joinerComber.GetMergedFiltering());
            return constraintFactory.Share(joinStream, joinStream_ =>
            {
                // Connect the bridges upstream, as it is an actual new join.
                GetChildStreamList().Add(leftBridge);
                other.GetChildStreamList().Add(rightBridge);
            });
        }

        public List<BavetAbstractConstraintStream> GetChildStreamList()
        {
            return childStreamList;
        }

        protected override IndictedObjectsMapping_ GetDefaultIndictedObjectsMapping<IndictedObjectsMapping_>()
        {
            return default(IndictedObjectsMapping_);
        }

        protected override JustificationMapping_ GetDefaultJustificationMapping<JustificationMapping_>()
        {
            return default(JustificationMapping_);
        }

        public BiConstraintStream<A, B> Join<B>(Type otherClass, BiJoiner<A, B> joiner1, BiJoiner<A, B> joiner2)
        {
            return Join<B>(otherClass, new BiJoiner<A, B>[] { joiner1, joiner2 });
        }

        public BiConstraintStream<A, B> Join<B>(Type otherClass, params BiJoiner<A, B>[] joiners)
        {
            if (GetRetrievalSemantics() == RetrievalSemantics.STANDARD)
            {
                var a = GetConstraintFactory().ForEach<B>(otherClass);
                return Join<B>(a, joiners);
            }
            else
            {
                return null;
                //return Join(GetConstraintFactory().From(otherClass), joiners);
            }
        }

        public BiConstraintStream<A, B> Join<B>(UniConstraintStream<B> otherStream, params BiJoiner<A, B>[] joiners)
        {
            BiJoinerComber<A, B> joinerComber = BiJoinerComber<A, B>.Comb(joiners);
            return Join(otherStream, joinerComber);
        }

        public UniConstraintStream<A> Filter(Func<A, bool> predicate)
        {
            return ShareAndAddChild(new BavetFilterUniConstraintStream<A>(constraintFactory, this, predicate));
        }

        public UniConstraintBuilder<A> Penalize(Score constraintWeight)
        {
            return Penalize(constraintWeight, a => 1);
        }

        public UniConstraintBuilder<A> Penalize(Score constraintWeight, Func<A, int> matchWeigher)
        {
            return InnerImpact(constraintWeight, matchWeigher, ScoreImpactType.PENALTY);
        }

        public UniConstraintBuilder<A> InnerImpact(Score constraintWeight, Func<A, int> matchWeigher, ScoreImpactType scoreImpactType)
        {
            var stream = ShareAndAddChild(new BavetScoringUniConstraintStream<A>(constraintFactory, this, matchWeigher));
            return NewTerminator(stream, constraintWeight, scoreImpactType);
        }

        private UniConstraintBuilderImpl<A> NewTerminator(BavetScoringConstraintStream stream, Score constraintWeight, ScoreImpactType impactType)
        {
            return new UniConstraintBuilderImpl<A>(
                    (constraintPackage, constraintName, constraintWeight_, impactType_, justificationMapping, indictedObjectsMapping)
                    => BuildConstraint(constraintPackage, constraintName, constraintWeight_, impactType_, justificationMapping, indictedObjectsMapping, stream),
                    impactType, constraintWeight);
        }

    }
}
