using TimefoldSharp.Core.Config.ConstructHeuristic.Decider.Forager;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.decider.Forager
{
    public class DefaultConstructionHeuristicForager : AbstractConstructionHeuristicForager
    {

        protected ConstructionHeuristicMoveScope maxScoreMoveScope;
        protected long selectedMoveCount;
        protected readonly ConstructionHeuristicPickEarlyType? pickEarlyType;
        protected ConstructionHeuristicMoveScope earlyPickedMoveScope;

        public DefaultConstructionHeuristicForager(ConstructionHeuristicPickEarlyType? pickEarlyType)
        {
            this.pickEarlyType = pickEarlyType;
        }

        public override void StepStarted(ConstructionHeuristicStepScope stepScope)
        {
            base.StepStarted(stepScope);
            selectedMoveCount = 0L;
            earlyPickedMoveScope = null;
            maxScoreMoveScope = null;
        }

        public override void StepEnded(ConstructionHeuristicStepScope stepScope)
        {
            base.StepEnded(stepScope);
            earlyPickedMoveScope = null;
            maxScoreMoveScope = null;
        }

        protected void CheckPickEarly(ConstructionHeuristicMoveScope moveScope)
        {
            switch (pickEarlyType)
            {
                case ConstructionHeuristicPickEarlyType.NEVER:
                    break;
                case ConstructionHeuristicPickEarlyType.FIRST_NON_DETERIORATING_SCORE:
                    API.Score.Score lastStepScore = moveScope.GetStepScope().GetPhaseScope()
                            .GetLastCompletedStepScope().GetScore();
                    if (moveScope.GetScore().WithInitScore(0).CompareTo(lastStepScore.WithInitScore(0)) >= 0)
                    {
                        earlyPickedMoveScope = moveScope;
                    }
                    break;
                case ConstructionHeuristicPickEarlyType.FIRST_FEASIBLE_SCORE:
                    if (moveScope.GetScore().WithInitScore(0).IsFeasible())
                    {
                        earlyPickedMoveScope = moveScope;
                    }
                    break;
                case ConstructionHeuristicPickEarlyType.FIRST_FEASIBLE_SCORE_OR_NON_DETERIORATING_HARD:
                    API.Score.Score lastStepScore2 = moveScope.GetStepScope().GetPhaseScope()
                            .GetLastCompletedStepScope().GetScore();
                    API.Score.Score lastStepScoreDifference = moveScope.GetScore().WithInitScore(0)
                            .Subtract(lastStepScore2.WithInitScore(0));
                    if (lastStepScoreDifference.IsFeasible())
                    {
                        earlyPickedMoveScope = moveScope;
                    }
                    break;
                default:
                    throw new Exception("The pickEarlyType (" + pickEarlyType + ") is not implemented.");
            }
        }

        public override void AddMove(ConstructionHeuristicMoveScope moveScope)
        {
            selectedMoveCount++;
            CheckPickEarly(moveScope);
            if (maxScoreMoveScope == null || moveScope.GetScore().CompareTo(maxScoreMoveScope.GetScore()) > 0)
            {
                maxScoreMoveScope = moveScope;
            }
        }

        public override bool IsQuitEarly()
        {
            return earlyPickedMoveScope != null;
        }

        public override ConstructionHeuristicMoveScope PickMove(ConstructionHeuristicStepScope stepScope)
        {
            stepScope.SetSelectedMoveCount(selectedMoveCount);
            if (earlyPickedMoveScope != null)
            {
                return earlyPickedMoveScope;
            }
            else
            {
                return maxScoreMoveScope;
            }
        }
    }
}
