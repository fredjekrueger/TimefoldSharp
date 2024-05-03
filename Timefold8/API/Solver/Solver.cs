using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Solver.Change;
using TimefoldSharp.Core.API.Solver.Event;

namespace TimefoldSharp.Core.API.Solver
{
    public interface Solver
    {

        ISolution Solve(ISolution problem);

        bool TerminateEarly();

        bool IsSolving();

        bool IsTerminateEarly();

        void AddProblemChange(ProblemChange problemChange);

        void AddProblemChanges(List<ProblemChange> problemChangeList);

        bool IsEveryProblemChangeProcessed();

        [Obsolete("forRemoval = true")]
        bool AddProblemFactChange(ProblemFactChange problemFactChange);

        [Obsolete("forRemoval = true")]
        bool AddProblemFactChanges(List<ProblemFactChange> problemFactChangeList);

        [Obsolete("forRemoval = true")]
        bool IsEveryProblemFactChangeProcessed();

        void AddEventListener(SolverEventListener eventListener);

        void RemoveEventListener(SolverEventListener eventListener);

        API.Score.Score GetBestScore();
    }
}