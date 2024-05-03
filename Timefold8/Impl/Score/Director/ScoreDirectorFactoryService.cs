using TimefoldSharp.Core.Config.Score.Director;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Impl.Score.Director
{
    public interface ScoreDirectorFactoryService
    {
        Func<AbstractScoreDirectorFactory> BuildScoreDirectorFactory(
            SolutionDescriptor solutionDescriptor, ScoreDirectorFactoryConfig config,
            EnvironmentMode environmentMode);

        ScoreDirectorType GetSupportedScoreDirectorType();
    }
}
