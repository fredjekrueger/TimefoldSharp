using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Change
{
    public interface ProblemChangeAdapter
    {
        void DoProblemChange(SolverScope solverScope);
    }
}
