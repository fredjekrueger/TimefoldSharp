using TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Event
{
    public class ConstructionHeuristicPhaseLifecycleListenerAdapter : SolverLifecycleListenerAdapter, ConstructionHeuristicPhaseLifecycleListener
    {
        public void PhaseEnded(ConstructionHeuristicPhaseScope phaseScope)
        {
        }

        public void PhaseStarted(ConstructionHeuristicPhaseScope phaseScope)
        {
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public virtual void StepEnded(ConstructionHeuristicStepScope stepScope)
        {
        }

        public virtual void StepStarted(ConstructionHeuristicStepScope stepScope)
        {
        }
    }
}
