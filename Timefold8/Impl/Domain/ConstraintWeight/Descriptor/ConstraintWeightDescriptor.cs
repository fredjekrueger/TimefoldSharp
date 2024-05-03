using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ConstraintWeight.Descriptor
{
    public class ConstraintWeightDescriptor
    {

        private readonly string constraintPackage;
        private readonly string constraintName;
        private readonly ConstraintConfigurationDescriptor constraintConfigurationDescriptor;
        private readonly MemberAccessor memberAccessor;

        public string GetConstraintPackage()
        {
            return constraintPackage;
        }
        public string GetConstraintName()
        {
            return constraintName;
        }

        public Func<ISolution, API.Score.Score> CreateExtractor()
        {
            SolutionDescriptor solutionDescriptor = constraintConfigurationDescriptor.GetSolutionDescriptor();
            MemberAccessor constraintConfigurationMemberAccessor = solutionDescriptor.GetConstraintConfigurationMemberAccessor();
            return (ISolution solution) =>
            {
                Object constraintConfiguration = constraintConfigurationMemberAccessor.ExecuteGetter(solution);
                return (API.Score.Score)memberAccessor.ExecuteGetter(constraintConfiguration);
            };
        }
    }
}
