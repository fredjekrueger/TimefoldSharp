using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider
{
    public class LocalSearchDecider
    {
        protected readonly string logIndentation;
        protected readonly Termination termination;
        protected readonly MoveSelector moveSelector;
        protected readonly Acceptor.Acceptor acceptor;
        protected readonly LocalSearchForager forager;

        protected bool assertMoveScoreFromScratch = false;
        protected bool assertExpectedUndoMoveScore = false;

        public LocalSearchDecider(String logIndentation, Termination termination,
            MoveSelector moveSelector, Acceptor.Acceptor acceptor, LocalSearchForager forager)
        {
            this.logIndentation = logIndentation;
            this.termination = termination;
            this.moveSelector = moveSelector;
            this.acceptor = acceptor;
            this.forager = forager;
        }
        public void DecideNextStep(LocalSearchStepScope stepScope)
        {
            InnerScoreDirector scoreDirector = stepScope.GetScoreDirector();
            scoreDirector.SetAllChangesWillBeUndoneBeforeStepEnds(true);
            int moveIndex = 0;
            foreach (var move in moveSelector)
            {
                var moveScope = new LocalSearchMoveScope(stepScope, moveIndex, move);
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
            scoreDirector.SetAllChangesWillBeUndoneBeforeStepEnds(false);
            PickMove(stepScope);
        }

        protected void DoMove(LocalSearchMoveScope moveScope)
        {
            InnerScoreDirector scoreDirector = moveScope.GetScoreDirector();
            if (!moveScope.GetMove().IsMoveDoable(scoreDirector))
            {
                throw new Exception("Impossible state: Local search move selector (" + moveSelector
                        + ") provided a non-doable move (" + moveScope.GetMove() + ").");
            }
            scoreDirector.DoAndProcessMove(moveScope.GetMove(), assertMoveScoreFromScratch, score =>
            {
                moveScope.SetScore(score);
                bool accepted = acceptor.IsAccepted(moveScope);
                moveScope.SetAccepted(accepted);
                forager.AddMove(moveScope);
            });
            if (assertExpectedUndoMoveScore)
            {
                scoreDirector.AssertExpectedUndoMoveScore(moveScope.GetMove(),
                        moveScope.GetStepScope().GetPhaseScope().GetLastCompletedStepScope().GetScore());
            }
        }

        protected void PickMove(LocalSearchStepScope stepScope)
        {
            LocalSearchMoveScope pickedMoveScope = forager.PickMove(stepScope);
            if (pickedMoveScope != null)
            {
                Move step = pickedMoveScope.GetMove();
                stepScope.SetStep(step);

                stepScope.SetScore(pickedMoveScope.GetScore());
            }
        }

        public void SolvingEnded(SolverScope solverScope)
        {
            moveSelector.SolvingEnded(solverScope);
            acceptor.SolvingEnded(solverScope);
            forager.SolvingEnded(solverScope);
        }

        public void SetAssertMoveScoreFromScratch(bool assertMoveScoreFromScratch)
        {
            this.assertMoveScoreFromScratch = assertMoveScoreFromScratch;
        }

        public void SetAssertExpectedUndoMoveScore(bool assertExpectedUndoMoveScore)
        {
            this.assertExpectedUndoMoveScore = assertExpectedUndoMoveScore;
        }

        public void StepStarted(LocalSearchStepScope stepScope)
        {
            moveSelector.StepStarted(stepScope);
            acceptor.StepStarted(stepScope);
            forager.StepStarted(stepScope);
        }

        public void SolvingStarted(SolverScope solverScope)
        {
            moveSelector.SolvingStarted(solverScope);
            acceptor.SolvingStarted(solverScope);
            forager.SolvingStarted(solverScope);
        }

        public void PhaseStarted(LocalSearchPhaseScope phaseScope)
        {
            moveSelector.PhaseStarted(phaseScope);
            acceptor.PhaseStarted(phaseScope);
            forager.PhaseStarted(phaseScope);
        }

        public void StepEnded(LocalSearchStepScope stepScope)
        {
            moveSelector.StepEnded(stepScope);
            acceptor.StepEnded(stepScope);
            forager.StepEnded(stepScope);
        }

        public void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
            moveSelector.PhaseEnded(phaseScope);
            acceptor.PhaseEnded(phaseScope);
            forager.PhaseEnded(phaseScope);
        }
    }
}