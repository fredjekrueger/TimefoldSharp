using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Solver.Event;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Event
{
    public class SolverEventSupport : AbstractEventSupport<SolverEventListener>
    {
        private readonly API.Solver.Solver solver;

        public SolverEventSupport(API.Solver.Solver solver)
        {
            this.solver = solver;
        }

        public void FireBestSolutionChanged(SolverScope solverScope, ISolution newBestSolution)
        {
            IEnumerator<SolverEventListener> it = GetEventListeners().GetEnumerator();
            long timeMillisSpent = solverScope.GetBestSolutionTimeMillisSpent();
            API.Score.Score bestScore = solverScope.GetBestScore();
            if (it.MoveNext())
            {
                BestSolutionChangedEvent ev = new BestSolutionChangedEvent(solver, timeMillisSpent, newBestSolution, bestScore);
                do
                {
                    it.Current.BestSolutionChanged(ev);
                }
                while (it.MoveNext());
            }
        }
    }
}
