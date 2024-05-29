using System.Numerics;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class BavetScoringUniConstraintStream<A> : BavetAbstractUniConstraintStream<A>, BavetScoringConstraintStream
    {


        private readonly Func<A, int> intMatchWeigher;
        private readonly Func<A, long> longMatchWeigher;
        private readonly Func<A, BigInteger> bigDecimalMatchWeigher;
        private BavetConstraint constraint;

        public BavetScoringUniConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractUniConstraintStream<A> parent, Func<A, int> intMatchWeigher)
            : this(constraintFactory, parent, intMatchWeigher, null, null)
        {
            if (intMatchWeigher == null)
            {
                throw new Exception("The matchWeigher (null) cannot be null.");
            }
        }

        public BavetScoringUniConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractUniConstraintStream<A> parent, Func<A, long> longMatchWeigher)
            : this(constraintFactory, parent, null, longMatchWeigher, null)
        {
            if (longMatchWeigher == null)
            {
                throw new Exception("The matchWeigher (null) cannot be null.");
            }
        }

        private BavetScoringUniConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractUniConstraintStream<A> parent, Func<A, int> intMatchWeigher,
          Func<A, long> longMatchWeigher, Func<A, BigInteger> bigDecimalMatchWeigher) : base(constraintFactory, parent)
        {
            this.intMatchWeigher = intMatchWeigher;
            this.longMatchWeigher = longMatchWeigher;
            this.bigDecimalMatchWeigher = bigDecimalMatchWeigher;
        }



        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            var constraintMatchEnabled = buildHelper.GetScoreInliner().IsConstraintMatchEnabled();
            var scoreImpacter = constraintMatchEnabled ? BuildScoreImpacterWithConstraintMatch() : BuildScoreImpacter();
            var weightedScoreImpacter = buildHelper.GetScoreInliner().BuildWeightedScoreImpacter(constraint);
            var scorer = new UniScorer<A>(weightedScoreImpacter, scoreImpacter, buildHelper.ReserveTupleStoreIndex(parent.GetTupleSource()));
            buildHelper.PutInsertUpdateRetract(this, scorer);
        }

        private Func<IWeightedScoreImpacter, A, UndoScoreImpacter> BuildScoreImpacter()
        {
            if (intMatchWeigher != null)
            {
                return (impacter, a) =>
                {
                    int matchWeight = intMatchWeigher(a);
                    return impacter.ImpactScore(matchWeight, null);
                };
            }
            else if (longMatchWeigher != null)
            {
                return (impacter, a) =>
                {
                    long matchWeight = longMatchWeigher(a);
                    return impacter.ImpactScore(matchWeight, null);
                };
            }
            else if (bigDecimalMatchWeigher != null)
            {
                return (impacter, a) =>
                {
                    BigInteger matchWeight = bigDecimalMatchWeigher(a);
                    return impacter.ImpactScore(matchWeight, null);
                };
            }
            else
            {
                throw new Exception("Impossible state: neither of the supported match weighers provided.");
            }
        }

        private Func<IWeightedScoreImpacter, A, UndoScoreImpacter> BuildScoreImpacterWithConstraintMatch()
        {
            /*if (intMatchWeigher != null)
             {
                 return (impacter, a)=> {
                     int matchWeight = intMatchWeigher(a);
                     return ImpactWithConstraintMatch(impacter, matchWeight, a);
                 };
             }
             else if (longMatchWeigher != null)
             {
                 return (impacter, a)=> {
                     long matchWeight = longMatchWeigher(a);
                     return ImpactWithConstraintMatch(impacter, matchWeight, a);
                 };
             }
             else if (bigDecimalMatchWeigher != null)
             {
                 return (impacter, a)=> {
                     BigInteger matchWeight = bigDecimalMatchWeigher(a);
                     return ImpactWithConstraintMatch(impacter, matchWeight, a);
                 };
             }
             else
             {
                 throw new Exception("Impossible state: neither of the supported match weighers provided.");
             }*/
            throw new NotImplementedException();
        }

        public void SetConstraint(BavetConstraint constraint)
        {
            this.constraint = constraint;
        }
    }
}
