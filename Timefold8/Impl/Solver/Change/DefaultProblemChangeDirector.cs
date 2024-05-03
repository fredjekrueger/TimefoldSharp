using TimefoldSharp.Core.API.Solver.Change;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Solver.Change
{
    public sealed class DefaultProblemChangeDirector : ProblemChangeDirector

    {
        public readonly InnerScoreDirector ScoreDirector;

        public DefaultProblemChangeDirector(InnerScoreDirector scoreDirector)
        {
            this.ScoreDirector = scoreDirector;
        }
    }
}
