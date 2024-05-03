using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Phase.Scope
{
    public abstract class AbstractPhaseScope
    {
        protected readonly SolverScope solverScope;

        protected API.Score.Score startingScore;

        protected long? endingSystemTimeMillis;
        protected long? endingScoreCalculationCount;
        protected long? startingSystemTimeMillis;
        protected long? startingScoreCalculationCount;
        protected int bestSolutionStepIndex;
        protected long childThreadsScoreCalculationCount = 0;

        public AbstractPhaseScope(SolverScope solverScope)
        {
            this.solverScope = solverScope;
        }

        public long GetPhaseScoreCalculationSpeed()
        {
            long timeMillisSpent = GetPhaseTimeMillisSpent();
            // Avoid divide by zero exception on a fast CPU
            var count = GetPhaseScoreCalculationCount();
            return count * 1000L / (timeMillisSpent == 0L ? 1L : timeMillisSpent);
        }

        public long GetPhaseScoreCalculationCount()
        {
            return endingScoreCalculationCount.Value - startingScoreCalculationCount.Value + childThreadsScoreCalculationCount;
        }

        public long GetPhaseTimeMillisSpent()
        {
            return (endingSystemTimeMillis.Value - startingSystemTimeMillis.Value) / TimeSpan.TicksPerMillisecond;
        }

        public Random GetWorkingRandom()
        {
            return GetSolverScope().GetWorkingRandom();
        }

        public long CalculateSolverTimeMillisSpentUpToNow()
        {
            return solverScope.CalculateTimeMillisSpentUpToNow();
        }

        public void StartingNow()
        {
            startingSystemTimeMillis = DateTime.UtcNow.Ticks;
            startingScoreCalculationCount = GetScoreDirector().GetCalculationCount();
        }

        public void SetBestSolutionStepIndex(int bestSolutionStepIndex)
        {
            this.bestSolutionStepIndex = bestSolutionStepIndex;
        }

        public void Reset()
        {
            bestSolutionStepIndex = -1;
            // solverScope.getBestScore() is null with an uninitialized score
            startingScore = solverScope.GetBestScore() == null ? solverScope.CalculateScore() : solverScope.GetBestScore();
            if (GetLastCompletedStepScope().GetStepIndex() < 0)
            {
                GetLastCompletedStepScope().SetScore(startingScore);
            }
        }

        public void EndingNow()
        {
            endingSystemTimeMillis = DateTime.UtcNow.Ticks;
            endingScoreCalculationCount = GetScoreDirector().GetCalculationCount();
        }

        public void AssertExpectedWorkingScore(API.Score.Score expectedWorkingScore, object completedAction)
        {
            InnerScoreDirector innerScoreDirector = GetScoreDirector();
            innerScoreDirector.AssertExpectedWorkingScore(expectedWorkingScore, completedAction);
        }

        public void AssertShadowVariablesAreNotStale(API.Score.Score workingScore, object completedAction)
        {
            InnerScoreDirector innerScoreDirector = GetScoreDirector();
            innerScoreDirector.AssertShadowVariablesAreNotStale(workingScore, completedAction);
        }

        public void AssertPredictedScoreFromScratch(API.Score.Score workingScore, object completedAction)
        {
            InnerScoreDirector innerScoreDirector = GetScoreDirector();
            innerScoreDirector.AssertPredictedScoreFromScratch(workingScore, completedAction);
        }

        public ISolution GetWorkingSolution()
        {
            return solverScope.GetWorkingSolution();
        }

        public SolverScope GetSolverScope()
        {
            return solverScope;
        }

        public API.Score.Score GetStartingScore()
        {
            return startingScore;
        }

        public API.Score.Score GetBestScore()
        {
            return solverScope.GetBestScore();
        }

        public int GetNextStepIndex()
        {
            return GetLastCompletedStepScope().GetStepIndex() + 1;
        }
        public abstract AbstractStepScope GetLastCompletedStepScope();

        public InnerScoreDirector GetScoreDirector()
        {
            return solverScope.ScoreDirector;
        }

        public SolutionDescriptor GetSolutionDescriptor()
        {
            return solverScope.GetSolutionDescriptor();
        }
    }
}
