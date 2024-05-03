using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Impl.Domain.ConstraintWeight.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public abstract class AbstractConstraintStream : ConstraintStream
    {
        private readonly RetrievalSemantics retrievalSemantics;

        protected AbstractConstraintStream(RetrievalSemantics retrievalSemantics)
        {
            this.retrievalSemantics = retrievalSemantics;

        }
        public Constraint Penalize(string constraintName, Score constraintWeight)
        {
            throw new NotImplementedException();
        }

        public RetrievalSemantics GetRetrievalSemantics()
        {
            return retrievalSemantics;
        }

        public abstract ConstraintFactory GetConstraintFactory();

        protected Func<ISolution, Score> BuildConstraintWeightExtractor(string constraintPackage, string constraintName)
        {
            SolutionDescriptor solutionDescriptor = GetConstraintFactory().GetSolutionDescriptor();
            ConstraintConfigurationDescriptor configurationDescriptor = solutionDescriptor.GetConstraintConfigurationDescriptor();
            if (configurationDescriptor == null)
            {
                throw new Exception("The constraint .penalize()/reward()instead of penalizeConfigurable()/rewardConfigurable.");
            }
            ConstraintWeightDescriptor weightDescriptor = configurationDescriptor.FindConstraintWeightDescriptor(constraintPackage, constraintName);
            if (weightDescriptor == null)
            {
                throw new Exception("The constraint  member for it.");
            }
            return weightDescriptor.CreateExtractor();
        }

        protected Func<ISolution, Score> BuildConstraintWeightExtractor(string constraintPackage, string constraintName, Score constraintWeight)
        {
            return solution => constraintWeight;
        }

        protected abstract JustificationMapping_ GetDefaultJustificationMapping<JustificationMapping_>();

        protected abstract IndictedObjectsMapping_ GetDefaultIndictedObjectsMapping<IndictedObjectsMapping_>();
    }
}
