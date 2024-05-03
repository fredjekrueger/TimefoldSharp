using TimefoldSharp.Core.Config.LocalSearch.Decider.Forager;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager.Finalist;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager
{
    public class AcceptedLocalSearchForager : AbstractLocalSearchForager
    {
        protected readonly FinalistPodium finalistPodium;
        protected readonly LocalSearchPickEarlyType pickEarlyType;
        protected readonly int acceptedCountLimit;
        protected readonly bool breakTieRandomly;
        protected LocalSearchMoveScope earlyPickedMoveScope;

        protected long selectedMoveCount;
        protected long acceptedMoveCount;

        public AcceptedLocalSearchForager(FinalistPodium finalistPodium,
            LocalSearchPickEarlyType pickEarlyType, int acceptedCountLimit, bool breakTieRandomly)
        {
            this.finalistPodium = finalistPodium;
            this.pickEarlyType = pickEarlyType;
            this.acceptedCountLimit = acceptedCountLimit;
            if (acceptedCountLimit < 1)
            {
                throw new Exception("The acceptedCountLimit (" + acceptedCountLimit
                        + ") cannot be negative or zero.");
            }
            this.breakTieRandomly = breakTieRandomly;
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public override void AddMove(LocalSearchMoveScope moveScope)
        {
            selectedMoveCount++;
            if (moveScope.GetAccepted().Value)
            {
                acceptedMoveCount++;
                CheckPickEarly(moveScope);
            }
            finalistPodium.AddMove(moveScope);
        }

        protected void CheckPickEarly(LocalSearchMoveScope moveScope)
        {
            switch (pickEarlyType)
            {
                case LocalSearchPickEarlyType.NEVER:
                    break;
                case LocalSearchPickEarlyType.FIRST_BEST_SCORE_IMPROVING:
                    API.Score.Score bestScore = moveScope.GetStepScope().GetPhaseScope().GetBestScore();
                    if (moveScope.GetScore().CompareTo(bestScore) > 0)
                    {
                        earlyPickedMoveScope = moveScope;
                    }
                    break;
                case LocalSearchPickEarlyType.FIRST_LAST_STEP_SCORE_IMPROVING:
                    API.Score.Score lastStepScore = moveScope.GetStepScope().GetPhaseScope()
                            .GetLastCompletedStepScope().GetScore();
                    if (moveScope.GetScore().CompareTo(lastStepScore) > 0)
                    {
                        earlyPickedMoveScope = moveScope;
                    }
                    break;
                default:
                    throw new Exception("The pickEarlyType (" + pickEarlyType + ") is not implemented.");
            }
        }

        public override bool SupportsNeverEndingMoveSelector()
        {
            return acceptedCountLimit < int.MaxValue;
        }

        public override bool IsQuitEarly()
        {
            return earlyPickedMoveScope != null || acceptedMoveCount >= acceptedCountLimit;
        }

        public override void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            finalistPodium.PhaseEnded(phaseScope);
            selectedMoveCount = 0L;
            acceptedMoveCount = 0L;
            earlyPickedMoveScope = null;
        }

        public override void PhaseStarted(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            finalistPodium.PhaseStarted(phaseScope);
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            base.SolvingEnded(solverScope);
            finalistPodium.SolvingEnded(solverScope);
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            base.SolvingStarted(solverScope);
            finalistPodium.SolvingStarted(solverScope);
        }

        public override void StepEnded(LocalSearchStepScope stepScope)
        {
            base.StepEnded(stepScope);
            finalistPodium.StepEnded(stepScope);
        }

        public override void StepStarted(LocalSearchStepScope stepScope)
        {
            base.StepStarted(stepScope);
            finalistPodium.StepStarted(stepScope);
            selectedMoveCount = 0L;
            acceptedMoveCount = 0L;
            earlyPickedMoveScope = null;
        }

        public override LocalSearchMoveScope PickMove(LocalSearchStepScope stepScope)
        {
            stepScope.SetSelectedMoveCount(selectedMoveCount);
            stepScope.SetAcceptedMoveCount(acceptedMoveCount);
            if (earlyPickedMoveScope != null)
            {
                return earlyPickedMoveScope;
            }
            List<LocalSearchMoveScope> finalistList = finalistPodium.GetFinalistList();
            if (finalistList.Count == 0)
            {
                return null;
            }
            if (finalistList.Count == 1 || !breakTieRandomly)
            {
                return finalistList[0];
            }
            int randomIndex = stepScope.GetWorkingRandom().Next(finalistList.Count);
            return finalistList[randomIndex];
        }
    }
}
