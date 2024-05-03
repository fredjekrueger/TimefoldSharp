using System.Collections;
using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public abstract class AbstractEntityPlacer : EntityPlacer
    {

        protected PhaseLifecycleSupport phaseLifecycleSupport = new PhaseLifecycleSupport();

        public abstract IEnumerator<Placement> GetEnumerator();

        public void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            phaseLifecycleSupport.FirePhaseEnded(phaseScope);
        }

        public void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            phaseLifecycleSupport.FirePhaseStarted(phaseScope);
        }

        public void SolvingEnded(SolverScope solverScope)
        {
            phaseLifecycleSupport.FireSolvingEnded(solverScope);
        }

        public void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void SolvingStarted(SolverScope solverScope)
        {
            phaseLifecycleSupport.FireSolvingStarted(solverScope);
        }

        public void StepEnded(AbstractStepScope stepScope)
        {
            phaseLifecycleSupport.FireStepEnded(stepScope);
        }

        public void StepStarted(AbstractStepScope stepScope)
        {
            phaseLifecycleSupport.FireStepStarted(stepScope);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
