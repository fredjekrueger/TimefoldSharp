using System.Collections.ObjectModel;
using System.Numerics;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public sealed class BavetScoringBiConstraintStream<A, B> : BavetAbstractBiConstraintStream<A, B>, BavetScoringConstraintStream
    {

        private readonly Func<A, B, int> intMatchWeigher;
        private readonly Func<A, B, long> longMatchWeigher;
        private readonly Func<A, B, BigInteger> bigDecimalMatchWeigher;
        private BavetConstraint constraint;

        public BavetScoringBiConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractBiConstraintStream<A, B> parent, Func<A, B, int> intMatchWeigher)
            : this(constraintFactory, parent, intMatchWeigher, null, null)
        {
            if (intMatchWeigher == null)
            {
                throw new Exception("The matchWeigher (null) cannot be null.");
            }
        }

        public override string ToString()
        {
            return "Scoring(" + constraint.GetConstraintName() + ")";
        }

        public void SetConstraint(BavetConstraint constraint)
        {
            this.constraint = constraint;
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            var constraintMatchEnabled = buildHelper.GetScoreInliner().IsConstraintMatchEnabled();
            Func<IWeightedScoreImpacter, A, B, UndoScoreImpacter> scoreImpacter;
            if (constraintMatchEnabled)
                scoreImpacter = BuildScoreImpacterWithConstraintMatch();
            else
                scoreImpacter = BuildScoreImpacter();

            IWeightedScoreImpacter weightedScoreImpacter =
                buildHelper.GetScoreInliner().BuildWeightedScoreImpacter(constraint);
            var scorer = new BiScorer<A, B, ScoreContext>
                (weightedScoreImpacter, scoreImpacter, buildHelper.ReserveTupleStoreIndex(parent.GetTupleSource()));
            buildHelper.PutInsertUpdateRetract(this, scorer);
        }

        private Func<IWeightedScoreImpacter, A, B, UndoScoreImpacter> BuildScoreImpacter()
        {
            if (intMatchWeigher != null)
            {
                return (impacter, a, b) =>
                {
                    int matchWeight = intMatchWeigher(a, b);
                    return impacter.ImpactScore(matchWeight, null);
                };
            }
            else if (longMatchWeigher != null)
            {
                return (impacter, a, b) =>
                {
                    long matchWeight = longMatchWeigher(a, b);
                    return impacter.ImpactScore(matchWeight, null);
                };
            }
            else if (bigDecimalMatchWeigher != null)
            {
                return (impacter, a, b) =>
                {
                    BigInteger matchWeight = bigDecimalMatchWeigher(a, b);
                    return impacter.ImpactScore(matchWeight, null);
                };
            }
            else
            {
                throw new Exception("Impossible state: neither of the supported match weighers provided.");
            }
        }

        private static UndoScoreImpacter ImpactWithConstraintMatch(IWeightedScoreImpacter impacter, int matchWeight, A a, B b)
        {
            var constraint = impacter.GetContext().GetConstraint();
            var constraintMatchSupplier = Streams.Common.Inliner.ConstraintMatchSupplierHelper
                .Of(constraint.GetJustificationMapping<Func<A, B, Score, ConstraintJustification>>(), constraint.GetIndictedObjectsMapping<Func<A, B, Collection<object>>>(), a, b);
            return impacter.ImpactScore(matchWeight, constraintMatchSupplier);
        }

        private static UndoScoreImpacter ImpactWithConstraintMatch(IWeightedScoreImpacter impacter, long matchWeight, A a, B b)
        {
            var constraint = impacter.GetContext().GetConstraint();
            var constraintMatchSupplier = Streams.Common.Inliner.ConstraintMatchSupplierHelper
                .Of(constraint.GetJustificationMapping<Func<A, B, Score, ConstraintJustification>>(), constraint.GetIndictedObjectsMapping<Func<A, B, Collection<object>>>(), a, b);
            return impacter.ImpactScore(matchWeight, constraintMatchSupplier);
        }


        private static UndoScoreImpacter ImpactWithConstraintMatch(IWeightedScoreImpacter impacter, BigInteger matchWeight, A a, B b)
        {
            var constraint = impacter.GetContext().GetConstraint();
            var constraintMatchSupplier = Streams.Common.Inliner.ConstraintMatchSupplierHelper
                .Of(constraint.GetJustificationMapping<Func<A, B, Score, ConstraintJustification>>(), constraint.GetIndictedObjectsMapping<Func<A, B, Collection<object>>>(), a, b);
            return impacter.ImpactScore(matchWeight, constraintMatchSupplier);
        }

        private Func<IWeightedScoreImpacter, A, B, UndoScoreImpacter> BuildScoreImpacterWithConstraintMatch()
        {
            /*if (intMatchWeigher != null)
            {
                return (impacter, a, b)=> {
                    int matchWeight = intMatchWeigher(a, b);
                    return ImpactWithConstraintMatch(impacter, matchWeight, a, b);
                };
            }
            else if (longMatchWeigher != null)
            {
                return (impacter, a, b)=> {
                    long matchWeight = longMatchWeigher(a, b);
                    return ImpactWithConstraintMatch(impacter, matchWeight, a, b);
                };
            }
            else if (bigDecimalMatchWeigher != null)
            {
                return (impacter, a, b)=> {
                    BigInteger matchWeight = bigDecimalMatchWeigher(a, b);
                    return ImpactWithConstraintMatch(impacter, matchWeight, a, b);
                };
            }
            else
            {
                throw new Exception("Impossible state: neither of the supported match weighers provided.");
            }*/

            throw new NotImplementedException();
        }

        public BavetScoringBiConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractBiConstraintStream<A, B> parent, Func<A, B, BigInteger> bigDecimalMatchWeigher)
            : this(constraintFactory, parent, null, null, bigDecimalMatchWeigher)
        {
            if (bigDecimalMatchWeigher == null)
            {
                throw new Exception("The matchWeigher (null) cannot be null.");
            }
        }

        public BavetScoringBiConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractBiConstraintStream<A, B> parent, Func<A, B, long> longMatchWeigher)
            : this(constraintFactory, parent, null, longMatchWeigher, null)
        {

            if (longMatchWeigher == null)
            {
                throw new Exception("The matchWeigher (null) cannot be null.");
            }
        }

        private BavetScoringBiConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractBiConstraintStream<A, B> parent, Func<A, B, int> intMatchWeigher,
           Func<A, B, long> longMatchWeigher, Func<A, B, BigInteger> bigDecimalMatchWeigher)
            : base(constraintFactory, parent)
        {
            this.intMatchWeigher = intMatchWeigher;
            this.longMatchWeigher = longMatchWeigher;
            this.bigDecimalMatchWeigher = bigDecimalMatchWeigher;
        }
    }
}
