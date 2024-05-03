using TimefoldSharp.Core.Config.Score.Director;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public abstract class AbstractConstraintStreamScoreDirectorFactoryService : ScoreDirectorFactoryService
    {
        public abstract Func<AbstractScoreDirectorFactory> BuildScoreDirectorFactory(SolutionDescriptor solutionDescriptor, ScoreDirectorFactoryConfig config, EnvironmentMode environmentMode);

        public abstract ScoreDirectorType GetSupportedScoreDirectorType();
    }
}
