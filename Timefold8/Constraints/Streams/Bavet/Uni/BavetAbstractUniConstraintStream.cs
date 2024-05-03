using TimefoldSharp.Core.API.Score.Stream.Bi;
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

        public BiConstraintStream<A, B> Join<B, Property_>(UniConstraintStream<B> otherStream, BiJoinerComber<A, B, Property_> joinerComber)
        {
            var other = (BavetAbstractUniConstraintStream<B>)otherStream;
            var leftBridge = new BavetForeBridgeUniConstraintStream<A>(constraintFactory, this);
            var rightBridge = new BavetForeBridgeUniConstraintStream<B>(constraintFactory, other);
            var joinStream = new BavetJoinBiConstraintStream<A, B, Property_>(constraintFactory, leftBridge, rightBridge,
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
            throw new NotImplementedException();
        }

        protected override JustificationMapping_ GetDefaultJustificationMapping<JustificationMapping_>()
        {
            throw new NotImplementedException();
        }

        public BiConstraintStream<A, B> Join<B, Property_>(Type otherClass, BiJoiner<A, B, Property_> joiner1, BiJoiner<A, B, Property_> joiner2)
        {
            return Join(otherClass, new BiJoiner<A, B, Property_>[] { joiner1, joiner2 });
        }

        public BiConstraintStream<A, B> Join<B, Property_>(Type otherClass, params BiJoiner<A, B, Property_>[] joiners)
        {
            if (GetRetrievalSemantics() == RetrievalSemantics.STANDARD)
            {
                var a = GetConstraintFactory().ForEach<B>(otherClass);
                return Join(a, joiners);
            }
            else
            {
                return null;
                //return Join(GetConstraintFactory().From(otherClass), joiners);
            }
        }

        public BiConstraintStream<A, B> Join<B, Property_>(UniConstraintStream<B> otherStream, params BiJoiner<A, B, Property_>[] joiners)
        {
            BiJoinerComber<A, B, Property_> joinerComber = BiJoinerComber<A, B, Property_>.Comb(joiners);
            return Join(otherStream, joinerComber);
        }
    }
}
