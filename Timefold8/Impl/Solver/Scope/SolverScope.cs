using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Config.Solver.Monitoring;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Solver.Change;

namespace TimefoldSharp.Core.Impl.Solver.Scope
{
    public class SolverScope
    {
        public Tags MonitoringTags { get; set; } = new Tags();
        public HashSet<SolverMetric> SolverMetricSet { get; set; }
        public InnerScoreDirector ScoreDirector { get; set; }
        public DefaultProblemChangeDirector ProblemChangeDirector { get; set; }

        protected volatile ISolution bestSolution;
        protected API.Score.Score bestScore; //was volatile
        protected API.Score.Score startingInitializedScore;
        protected long bestSolutionTimeMillis;
        protected long startingSystemTimeMillis; //was volatile
        protected long endingSystemTimeMillis; //was volatile
        protected int startingSolverCount;
        private System.Random workingRandom;
        protected long childThreadsScoreCalculationCount = 0;

        Semaphore runnableThreadSemaphore = null;

        public System.Random GetWorkingRandom()
        {
            return workingRandom;
        }

        public void AssertScoreFromScratch(ISolution solution)
        {
            ScoreDirector.GetScoreDirectorFactory().AssertScoreFromScratch(solution);
        }

        public bool IsMetricEnabled(SolverMetric solverMetric)
        {
            return SolverMetricSet.Contains(solverMetric);
        }

        public void SetBestSolution(ISolution bestSolution)
        {
            this.bestSolution = bestSolution;
        }

        public API.Score.Score CalculateScore()
        {
            return ScoreDirector.CalculateScore();
        }

        public void CheckYielding()
        {
            if (runnableThreadSemaphore != null)
            {
                runnableThreadSemaphore.Release();
                try
                {
                    runnableThreadSemaphore.WaitOne();
                }
                catch (Exception)
                {
                    // The BasicPlumbingTermination will terminate the solver.
                    Thread.CurrentThread.Interrupt();
                }
            }
        }

        public SolutionDescriptor GetSolutionDescriptor()
        {
            return ScoreDirector.GetSolutionDescriptor();
        }

        public void SetBestScore(API.Score.Score bestScore)
        {
            this.bestScore = bestScore;
        }

        public API.Score.Score GetBestScore()
        {
            return bestScore;
        }

        public void SetBestSolutionTimeMillis(long bestSolutionTimeMillis)
        {
            this.bestSolutionTimeMillis = bestSolutionTimeMillis;
        }

        public ISolution GetWorkingSolution()
        {
            return ScoreDirector.GetWorkingSolution();
        }

        public long GetBestSolutionTimeMillisSpent()
        {
            return bestSolutionTimeMillis - startingSystemTimeMillis;
        }

        public bool IsBestSolutionInitialized()
        {
            return bestScore.IsSolutionInitialized();
        }

        public ISolution GetBestSolution()
        {
            return bestSolution;
        }

        public void SetStartingInitializedScore(API.Score.Score startingInitializedScore)
        {
            this.startingInitializedScore = startingInitializedScore;
        }

        public void SetWorkingSolutionFromBestSolution()
        {
            // The workingSolution must never be the same instance as the bestSolution.
            ScoreDirector.SetWorkingSolution(ScoreDirector.CloneSolution(bestSolution));
        }

        public void SetStartingSolverCount(int startingSolverCount)
        {
            this.startingSolverCount = startingSolverCount;
        }

        public void SetWorkingRandom(System.Random workingRandom)
        {
            this.workingRandom = workingRandom;
        }

        public int GetStartingSolverCount()
        {
            return startingSolverCount;
        }

        public void EndingNow()
        {
            endingSystemTimeMillis = DateTime.UtcNow.Ticks;
        }

        public void StartingNow()
        {
            startingSystemTimeMillis = DateTime.UtcNow.Ticks;
            endingSystemTimeMillis = -1;
        }

        public long CalculateTimeMillisSpentUpToNow()
        {
            long now = DateTime.UtcNow.Ticks;
            return (now - startingSystemTimeMillis) / TimeSpan.TicksPerMillisecond;
        }

        internal long GetTimeMillisSpent()
        {
            return (endingSystemTimeMillis - startingSystemTimeMillis) / TimeSpan.TicksPerMillisecond;
        }

        internal object GetScoreCalculationSpeed()
        {
            long timeMillisSpent = GetTimeMillisSpent();
            // Avoid divide by zero exception on a fast CPU
            return GetScoreCalculationCount() * 1000L / (timeMillisSpent == 0L ? 1L : timeMillisSpent);
        }

        public long GetScoreCalculationCount()
        {
            return ScoreDirector.GetCalculationCount() + childThreadsScoreCalculationCount;
        }
    }

    public class Tags : List<Tag>
    {

    }

    public class Tag
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
