using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common
{
    public sealed class SelectionCacheLifecycleBridge : PhaseLifecycleListener
    {
        private readonly SelectionCacheType? cacheType;
        private readonly SelectionCacheLifecycleListener selectionCacheLifecycleListener;

        public SelectionCacheLifecycleBridge(SelectionCacheType? cacheType, SelectionCacheLifecycleListener selectionCacheLifecycleListener)
        {
            this.cacheType = cacheType;
            this.selectionCacheLifecycleListener = selectionCacheLifecycleListener;
            if (cacheType == null)
            {
                throw new Exception("The cacheType (" + cacheType
                        + ") for selectionCacheLifecycleListener (" + selectionCacheLifecycleListener
                        + ") should have already been resolved.");
            }
        }

        public void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public void SolvingEnded(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void SolvingStarted(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public void StepEnded(AbstractStepScope stepScope)
        {
            throw new NotImplementedException();
        }

        public void StepStarted(AbstractStepScope stepScope)
        {
            throw new NotImplementedException();
        }
    }
}
