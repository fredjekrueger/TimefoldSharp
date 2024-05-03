using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager.Finalist
{
    public abstract class AbstractFinalistPodium : LocalSearchPhaseLifecycleListenerAdapter, FinalistPodium
    {
        protected static readonly int FINALIST_LIST_MAX_SIZE = 1_024_000;

        protected bool finalistIsAccepted;
        protected List<LocalSearchMoveScope> finalistList = new List<LocalSearchMoveScope>(1024);
        public abstract void AddMove(LocalSearchMoveScope moveScope);

        public List<LocalSearchMoveScope> GetFinalistList()
        {
            return finalistList;
        }

        public override void StepStarted(LocalSearchStepScope stepScope)
        {
            base.StepStarted(stepScope);
            finalistIsAccepted = false;
            finalistList.Clear();
        }

        protected void ClearAndAddFinalist(LocalSearchMoveScope moveScope)
        {
            finalistList.Clear();
            finalistList.Add(moveScope);
        }

        protected void AddFinalist(LocalSearchMoveScope moveScope)
        {
            if (finalistList.Count >= FINALIST_LIST_MAX_SIZE)
            {
                // Avoid unbounded growth and OutOfMemoryException
                return;
            }
            finalistList.Add(moveScope);
        }

        public override void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            finalistIsAccepted = false;
            finalistList.Clear();
        }
    }
}
