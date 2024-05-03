using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope
{
    public class ConstructionHeuristicPhaseScope : AbstractPhaseScope
    {

        private ConstructionHeuristicStepScope lastCompletedStepScope;

        public ConstructionHeuristicPhaseScope(SolverScope solverScope)
            : base(solverScope)
        {

            lastCompletedStepScope = new ConstructionHeuristicStepScope(this, -1);
        }

        public override AbstractStepScope GetLastCompletedStepScope()
        {
            return lastCompletedStepScope;
        }

        public void SetLastCompletedStepScope(ConstructionHeuristicStepScope lastCompletedStepScope)
        {
            this.lastCompletedStepScope = lastCompletedStepScope;
        }
    }
}
