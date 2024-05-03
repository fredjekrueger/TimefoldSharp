using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Trend;

namespace TimefoldSharp.Core.Impl.Score.Director
{
    public abstract class AbstractScoreDirectorFactory : InnerScoreDirectorFactory
    {

        public AbstractScoreDirectorFactory(SolutionDescriptor solutionDescriptor)
        {
            this.solutionDescriptor = solutionDescriptor;
        }

        public InnerScoreDirectorFactory AssertionScoreDirectorFactory { get; set; }
        public InitializingScoreTrend InitializingScoreTrend;
        protected SolutionDescriptor solutionDescriptor;
        protected bool assertClonedSolution = false;

        public bool AssertClonedSolution { get; set; }

        public override InitializingScoreTrend GetInitializingScoreTrend()
        {
            return InitializingScoreTrend;
        }


        public bool IsAssertClonedSolution()
        {
            return assertClonedSolution;
        }
        public SolutionDescriptor GetSolutionDescriptor()
        {
            return solutionDescriptor;
        }

        public override void AssertScoreFromScratch(ISolution solution)
        {
            throw new NotImplementedException();
        }
    }
}
