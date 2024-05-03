using TimefoldSharp.Core.Impl.ConstructionHeuristic.decider.Forager;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope;
using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.decider
{
    public class ConstructionHeuristicDecider
    {

        protected readonly string logIndentation;
        protected readonly Termination termination;
        protected readonly ConstructionHeuristicForager forager;
        protected bool assertExpectedUndoMoveScore = false;
        protected bool assertMoveScoreFromScratch = false;

        public ConstructionHeuristicDecider(string logIndentation, Termination termination, ConstructionHeuristicForager forager)
        {
            this.logIndentation = logIndentation;
            this.termination = termination;
            this.forager = forager;
        }

        public void SolvingStarted(SolverScope solverScope)
        {
            forager.SolvingStarted(solverScope);
        }

        static int counter = 0;
        public void DecideNextStep(ConstructionHeuristicStepScope stepScope, Placement placement)
        {
            int moveIndex = 0;
            foreach (var move in placement)
            {
                counter++;
                ConstructionHeuristicMoveScope moveScope = new ConstructionHeuristicMoveScope(stepScope, moveIndex, move);
                moveIndex++;
                DoMove(moveScope);
                if (forager.IsQuitEarly())
                {
                    break;
                }
                stepScope.GetPhaseScope().GetSolverScope().CheckYielding();
                if (termination.IsPhaseTerminated(stepScope.GetPhaseScope()))
                {
                    break;
                }
            }
            PickMove(stepScope);
        }

        protected void PickMove(ConstructionHeuristicStepScope stepScope)
        {
            ConstructionHeuristicMoveScope pickedMoveScope = forager.PickMove(stepScope);
            if (pickedMoveScope != null)
            {
                Move step = pickedMoveScope.GetMove();
                stepScope.SetStep(step);

                stepScope.SetScore(pickedMoveScope.GetScore());
            }
        }

        protected void DoMove(ConstructionHeuristicMoveScope moveScope)
        {
            InnerScoreDirector scoreDirector = moveScope.GetScoreDirector();
            scoreDirector.DoAndProcessMove(moveScope.GetMove(), assertMoveScoreFromScratch, score =>
            {
                moveScope.SetScore(score);
                forager.AddMove(moveScope);
            });
            if (assertExpectedUndoMoveScore)
            {
                scoreDirector.AssertExpectedUndoMoveScore(moveScope.GetMove(), moveScope.GetStepScope().GetPhaseScope().GetLastCompletedStepScope().GetScore());
            }
        }

        public void StepStarted(ConstructionHeuristicStepScope stepScope)
        {
            forager.StepStarted(stepScope);
        }

        public void StepEnded(ConstructionHeuristicStepScope stepScope)
        {
            forager.StepEnded(stepScope);
        }

        public void PhaseEnded(ConstructionHeuristicPhaseScope phaseScope)
        {
            forager.PhaseEnded(phaseScope);
        }

        public void SolvingEnded(SolverScope solverScope)
        {
            forager.SolvingEnded(solverScope);
        }

        public void PhaseStarted(ConstructionHeuristicPhaseScope phaseScope)
        {
            forager.PhaseStarted(phaseScope);
        }

        public void SetAssertExpectedUndoMoveScore(bool assertExpectedUndoMoveScore)
        {
            this.assertExpectedUndoMoveScore = assertExpectedUndoMoveScore;
        }

        public void SetAssertMoveScoreFromScratch(bool assertMoveScoreFromScratch)
        {
            this.assertMoveScoreFromScratch = assertMoveScoreFromScratch;
        }
    }
}
