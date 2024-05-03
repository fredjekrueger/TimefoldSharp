using Serilog;
using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.LocalSearch.Decider;
using TimefoldSharp.Core.Impl.LocalSearch.Event;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.LocalSearch
{
    public class DefaultLocalSearchPhase : AbstractPhase, LocalSearchPhase,
        LocalSearchPhaseLifecycleListener
    {

        protected readonly LocalSearchDecider decider;

        private DefaultLocalSearchPhase(Builder builder)
            : base(builder)
        {
            decider = builder.Decider;
        }


        public override void Solve(SolverScope solverScope)
        {
            LocalSearchPhaseScope phaseScope = new LocalSearchPhaseScope(solverScope);
            PhaseStarted(phaseScope);

            while (!phaseTermination.IsPhaseTerminated(phaseScope))
            {
                LocalSearchStepScope stepScope = new LocalSearchStepScope(phaseScope);
                stepScope.SetTimeGradient(phaseTermination.CalculatePhaseTimeGradient(phaseScope));
                StepStarted(stepScope);
                decider.DecideNextStep(stepScope);
                if (stepScope.GetStep() == null)
                {
                    if (phaseTermination.IsPhaseTerminated(phaseScope))
                    {

                    }
                    else if (stepScope.GetSelectedMoveCount() == 0L)
                    {

                    }
                    else
                    {
                        throw new Exception("The step index (" + stepScope.GetStepIndex()
                                + ") has accepted/selected move count /"
                                + stepScope.GetSelectedMoveCount()
                                + ") but failed to pick a nextStep (" + stepScope.GetStep() + ").");
                    }
                    // Although stepStarted has been called, stepEnded is not called for this step
                    break;
                }
                DoStep(stepScope);
                StepEnded(stepScope);
                phaseScope.SetLastCompletedStepScope(stepScope);
            }
            PhaseEnded(phaseScope);
        }

        public override void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            decider.PhaseStarted((LocalSearchPhaseScope)phaseScope);
        }

        public override void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            decider.PhaseEnded((LocalSearchPhaseScope)phaseScope);
            phaseScope.EndingNow();
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            base.SolvingStarted(solverScope);
            decider.SolvingStarted(solverScope);
        }

        public override void StepEnded(AbstractStepScope stepScope)
        {
            var ls = (LocalSearchStepScope)stepScope;
            base.StepEnded(stepScope);
            decider.StepEnded(ls);
            LocalSearchPhaseScope phaseScope = (LocalSearchPhaseScope)ls.GetPhaseScope();
        }

        public override void StepStarted(AbstractStepScope stepScope)
        {
            base.StepStarted(stepScope);
            decider.StepStarted((LocalSearchStepScope)stepScope);
        }

        protected void DoStep(LocalSearchStepScope stepScope)
        {
            Move step = stepScope.GetStep();
            Move undoStep = step.DoMove(stepScope.GetScoreDirector());
            stepScope.SetUndoStep(undoStep);
            PredictWorkingStepScore(stepScope, step);
            solver.GetBestSolutionRecaller().ProcessWorkingSolutionDuringStep(stepScope);
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            base.SolvingEnded(solverScope);
            decider.SolvingEnded(solverScope);
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void PhaseStarted(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            decider.PhaseStarted(phaseScope);
        }

        public void StepStarted(LocalSearchStepScope stepScope)
        {
            base.StepStarted(stepScope);
            decider.StepStarted(stepScope);
        }

        public void StepEnded(LocalSearchStepScope stepScope)
        {
            base.StepEnded(stepScope);
            decider.StepEnded(stepScope);
            //var phaseScope = stepScope.GetPhaseScope();
        }

        public void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            decider.PhaseEnded(phaseScope);
            phaseScope.EndingNow();
            Log.Debug($@"{logIndentation}Local Search phase ({phaseIndex}) ended: time spent ({phaseScope.CalculateSolverTimeMillisSpentUpToNow()}), best score ({phaseScope.GetBestScore()}),
                 score calculation speed ({phaseScope.GetPhaseScoreCalculationSpeed()}/sec), step total ({phaseScope.GetNextStepIndex()}).");
        }

        public class Builder : AbstrBuilder
        {

            public readonly LocalSearchDecider Decider;

            public Builder(int phaseIndex, String logIndentation, Termination phaseTermination, LocalSearchDecider decider)
                : base(phaseIndex, logIndentation, phaseTermination)
            {
                this.Decider = decider;
            }

            public override AbstractPhase Build()
            {
                return new DefaultLocalSearchPhase(this);
            }
        }

    }
}
