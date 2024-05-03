using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Scope
{
    public class LocalSearchPhaseScope : AbstractPhaseScope
    {

        private LocalSearchStepScope lastCompletedStepScope;

        public LocalSearchPhaseScope(SolverScope solverScope)
                : base(solverScope)
        {
            lastCompletedStepScope = new LocalSearchStepScope(this, -1);
            lastCompletedStepScope.SetTimeGradient(0.0);
        }

        public override AbstractStepScope GetLastCompletedStepScope()
        {
            return lastCompletedStepScope;
        }

        public void SetLastCompletedStepScope(LocalSearchStepScope lastCompletedStepScope)
        {
            this.lastCompletedStepScope = lastCompletedStepScope;
        }
    }
}
