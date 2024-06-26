using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector
{
    public abstract class AbstractSelector : Selector
    {
        protected PhaseLifecycleSupport phaseLifecycleSupport = new PhaseLifecycleSupport();
        protected Random workingRandom = null;

        public virtual SelectionCacheType GetCacheType()
        {
            return SelectionCacheType.JUST_IN_TIME;
        }

        public abstract bool IsNeverEnding();

        public abstract bool IsCountable();

        public virtual void SolvingEnded(SolverScope solverScope)
        {
            phaseLifecycleSupport.FireSolvingEnded(solverScope);
            workingRandom = null;
        }

        public void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public virtual void SolvingStarted(SolverScope solverScope)
        {
            workingRandom = solverScope.GetWorkingRandom();
            phaseLifecycleSupport.FireSolvingStarted(solverScope);
        }

        public virtual void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            phaseLifecycleSupport.FirePhaseStarted(phaseScope);
        }

        public virtual void StepStarted(AbstractStepScope stepScope)
        {
            phaseLifecycleSupport.FireStepStarted(stepScope);
        }

        public void StepEnded(AbstractStepScope stepScope)
        {
            phaseLifecycleSupport.FireStepEnded(stepScope);
        }

        public virtual void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            phaseLifecycleSupport.FirePhaseEnded(phaseScope);
        }
    }
}
