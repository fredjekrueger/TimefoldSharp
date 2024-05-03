using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ConstraintWeight.Descriptor
{
    public class ConstraintConfigurationDescriptor
    {
        public readonly Type ConstraintConfigurationClass;
        private readonly SolutionDescriptor solutionDescriptor;
        private readonly Dictionary<string, ConstraintWeightDescriptor> constraintWeightDescriptorMap;

        internal void ProcessAnnotations(DescriptorPolicy descriptorPolicy, ScoreDefinition scoreDefinition)
        {
            throw new NotImplementedException();
        }

        public ConstraintWeightDescriptor FindConstraintWeightDescriptor(string constraintPackage, string constraintName)
        {
            return constraintWeightDescriptorMap.Values.Where(
                    constraintWeightDescriptor => constraintWeightDescriptor.GetConstraintPackage() == constraintPackage
                            && constraintWeightDescriptor.GetConstraintName() == constraintName)
                    .FirstOrDefault();
        }

        public SolutionDescriptor GetSolutionDescriptor()
        {
            return solutionDescriptor;
        }
    }
}
