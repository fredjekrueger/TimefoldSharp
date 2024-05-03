using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager.Finalist
{
    public class HighestScoreFinalistPodium : AbstractFinalistPodium
    {
        protected API.Score.Score finalistScore;

        public override void AddMove(LocalSearchMoveScope moveScope)
        {
            bool accepted = moveScope.GetAccepted().Value;
            if (finalistIsAccepted && !accepted)
            {
                return;
            }
            if (accepted && !finalistIsAccepted)
            {
                finalistIsAccepted = true;
                finalistScore = null;
            }
            var moveScore = moveScope.GetScore();
            int scoreComparison = DoComparison(moveScore);
            if (scoreComparison > 0)
            {
                finalistScore = moveScore;
                ClearAndAddFinalist(moveScope);
            }
            else if (scoreComparison == 0)
            {
                AddFinalist(moveScope);
            }
        }

        private int DoComparison(API.Score.Score moveScore)
        {
            if (finalistScore == null)
            {
                return 1;
            }
            return moveScore.CompareTo(finalistScore);
        }

        public override void StepStarted(LocalSearchStepScope stepScope)
        {
            base.StepStarted(stepScope);
            finalistScore = null;
        }
        public override void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
        }
    }
}
