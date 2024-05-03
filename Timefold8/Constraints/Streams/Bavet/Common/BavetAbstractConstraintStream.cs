using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Constraints.Streams.Common;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class BavetAbstractConstraintStream : AbstractConstraintStream
    {

        protected readonly BavetConstraintFactory constraintFactory;
        protected readonly BavetAbstractConstraintStream parent;
        protected readonly List<BavetAbstractConstraintStream> childStreamList = new List<BavetAbstractConstraintStream>(2);

        protected BavetAbstractConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractConstraintStream parent)
            : base(parent.GetRetrievalSemantics())
        {

            this.constraintFactory = constraintFactory;
            this.parent = parent;
        }

        public BavetAbstractConstraintStream GetTupleSource()
        {
            if (this is TupleSource)
            {
                return this;
            }
            else if (parent == null)
            { // Maybe some stream forgot to override this?
                throw new Exception("Impossible state: the stream (" + this + ") does not have a parent.");
            }
            return parent.GetTupleSource();
        }

        public virtual void CollectActiveConstraintStreams(HashSet<BavetAbstractConstraintStream> constraintStreamSet)
        {
            if (parent == null)
            { // Maybe a join/ifExists/forEach forgot to override this?
                throw new Exception("Impossible state: the stream (" + this + ") does not have a parent.");
            }
            parent.CollectActiveConstraintStreams(constraintStreamSet);
            constraintStreamSet.Add(this); //JDEF ADDIFNOTEXIST
        }

        protected BavetAbstractConstraintStream(BavetConstraintFactory constraintFactory, RetrievalSemantics retrievalSemantics)
            : base(retrievalSemantics)
        {

            this.constraintFactory = constraintFactory;
            this.parent = null;
        }

        public override ConstraintFactory GetConstraintFactory()
        {
            return constraintFactory;
        }

        public abstract void BuildNode(NodeBuildHelper buildHelper);

        public Stream_ ShareAndAddChild<Stream_>(Stream_ stream) where Stream_ : BavetAbstractConstraintStream
        {
            return constraintFactory.Share(stream, childStreamList.Add);
        }

        public BavetAbstractConstraintStream GetParent()
        {
            return parent;
        }

        protected Constraint BuildConstraint(string constraintPackage, string constraintName, Score constraintWeight,
         ScoreImpactType impactType, object justificationFunction, object indictedObjectsMapping, BavetScoringConstraintStream stream)
        {
            var resolvedConstraintPackage = constraintPackage ?? constraintFactory.GetDefaultConstraintPackage();
            object resolvedJustificationMapping = null;
            if (justificationFunction != null)
            {
                resolvedJustificationMapping = justificationFunction;
            }
            else
            {
                resolvedJustificationMapping = GetDefaultJustificationMapping<object>();
            }
            object resolvedIndictedObjectsMapping = null;
            if (indictedObjectsMapping != null)
            {
                resolvedJustificationMapping = indictedObjectsMapping;
            }
            else
            {
                resolvedJustificationMapping = GetDefaultIndictedObjectsMapping<object>();
            }
            var isConstraintWeightConfigurable = constraintWeight == null;
            var constraintWeightExtractor = isConstraintWeightConfigurable
                    ? BuildConstraintWeightExtractor(resolvedConstraintPackage, constraintName)
                    : BuildConstraintWeightExtractor(resolvedConstraintPackage, constraintName, constraintWeight);
            var constraint =
                    new BavetConstraint(constraintFactory, resolvedConstraintPackage, constraintName, constraintWeightExtractor,
                            impactType, resolvedJustificationMapping, resolvedIndictedObjectsMapping,
                            isConstraintWeightConfigurable, stream);
            stream.SetConstraint(constraint);
            return constraint;
        }


    }
}
