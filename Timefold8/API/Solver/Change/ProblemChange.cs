using TimefoldSharp.Core.API.Score;

namespace TimefoldSharp.Core.API.Solver.Change
{
    public delegate void ProblemChange(ISolution workingSolution, ProblemChangeDirector problemChangeDirector);

    public interface ProblemChangeDirector<Solution>
    {
        void DoChange(Solution workingSolution, ProblemChangeDirector<Solution> problemChangeDirector);
    }
}
