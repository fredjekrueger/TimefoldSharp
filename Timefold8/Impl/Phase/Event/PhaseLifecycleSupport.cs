using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Phase.Event
{
    public sealed class PhaseLifecycleSupport : AbstractEventSupport<PhaseLifecycleListener>
    {
        public void FireSolvingEnded(SolverScope solverScope)
        {
            foreach (var listener in GetEventListeners())
            {
                listener.SolvingEnded(solverScope);
            }
        }

        public void FireStepEnded(AbstractStepScope stepScope)
        {
            foreach (PhaseLifecycleListener listener in GetEventListeners())
            {
                listener.StepEnded(stepScope);
            }
        }

        public void FirePhaseEnded(AbstractPhaseScope phaseScope)
        {
            foreach (var listener in GetEventListeners())
            {
                listener.PhaseEnded(phaseScope);
            }
        }

        public void FireStepStarted(AbstractStepScope stepScope)
        {
            foreach (var listener in GetEventListeners())
            {
                listener.StepStarted(stepScope);
            }
        }

        public void fireStepEnded(AbstractStepScope stepScope)
        {
            foreach (var listener in GetEventListeners())
            {
                listener.StepEnded(stepScope);
            }
        }

        public void FirePhaseStarted(AbstractPhaseScope phaseScope)
        {
            foreach (var listener in GetEventListeners())
            {
                listener.PhaseStarted(phaseScope);
            }
        }

        public void FireSolvingError(SolverScope solverScope, Exception exception)
        {
            foreach (var listener in GetEventListeners())
            {
                listener.SolvingError(solverScope, exception);
            }
        }

        public void FireSolvingStarted(SolverScope solverScope)
        {
            foreach (var listener in GetEventListeners())
            {
                listener.SolvingStarted(solverScope);
            }
        }
    }
}
