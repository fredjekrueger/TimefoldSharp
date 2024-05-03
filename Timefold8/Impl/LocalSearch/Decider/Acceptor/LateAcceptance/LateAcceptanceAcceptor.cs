using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.LateAcceptance
{
    public class LateAcceptanceAcceptor : AbstractAcceptor
    {

        protected int lateAcceptanceSize = -1;
        protected bool hillClimbingEnabled = true;

        public void SetLateAcceptanceSize(int lateAcceptanceSize)
        {
            this.lateAcceptanceSize = lateAcceptanceSize;
        }

        public override bool IsAccepted(LocalSearchMoveScope moveScope)
        {
            var moveScore = moveScope.GetScore();
            var lateScore = previousScores[lateScoreIndex];
            if (moveScore.CompareTo(lateScore) >= 0)
            {
                return true;
            }
            if (hillClimbingEnabled)
            {
                var lastStepScore = moveScope.GetStepScope().GetPhaseScope().GetLastCompletedStepScope().GetScore();
                if (moveScore.CompareTo(lastStepScore) >= 0)
                {
                    return true;
                }
            }
            return false;
        }


        public override void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            previousScores = null;
            lateScoreIndex = -1;
        }

        private void Validate()
        {
            if (lateAcceptanceSize <= 0)
            {
                throw new Exception("The lateAcceptanceSize (" + lateAcceptanceSize + ") cannot be negative or zero.");
            }
        }

        protected API.Score.Score[] previousScores;
        protected int lateScoreIndex = -1;

        public override void PhaseStarted(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            Validate();
            previousScores = new API.Score.Score[lateAcceptanceSize];
            var initialScore = phaseScope.GetBestScore();
            for (int i = 0; i < previousScores.Length; i++)
            {
                previousScores[i] = initialScore;
            }
            lateScoreIndex = 0;
        }



        public override void StepEnded(LocalSearchStepScope stepScope)
        {
            base.StepEnded(stepScope);
            previousScores[lateScoreIndex] = stepScope.GetScore();
            lateScoreIndex = (lateScoreIndex + 1) % lateAcceptanceSize;
        }

    }
}
