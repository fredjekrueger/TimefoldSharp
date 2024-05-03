using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public abstract class AbstractConstraintStreamScoreDirectorFactory : AbstractScoreDirectorFactory
    {
        protected AbstractConstraintStreamScoreDirectorFactory(SolutionDescriptor solutionDescriptor) : base(solutionDescriptor)
        {
        }
    }
}

