using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.API.Solver.Event
{
    public interface SolverEventListener : EventListenerTF
    {
        void BestSolutionChanged(BestSolutionChangedEvent e);
    }

    public class BestSolutionChangedEvent : EventArgs
    {
        public ISolution NewBestSolution { get; set; }
        private API.Score.Score newBestScore;

        public BestSolutionChangedEvent(ISolution newBestSolution)
        {
            NewBestSolution = newBestSolution;
        }

        private readonly Solver solver;
        private readonly long timeMillisSpent;

        public BestSolutionChangedEvent(Solver solver, long timeMillisSpent, ISolution newBestSolution, API.Score.Score newBestScore)
        {
            this.solver = solver;
            this.timeMillisSpent = timeMillisSpent;
            this.NewBestSolution = newBestSolution;
            this.newBestScore = newBestScore;
        }
    }
}
