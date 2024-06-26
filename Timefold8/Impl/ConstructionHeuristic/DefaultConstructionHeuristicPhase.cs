using Serilog;
using TimefoldSharp.Core.API.Score.Buildin.HardSoft;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.decider;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer;
using TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope;
using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic
{
    public class DefaultConstructionHeuristicPhase : AbstractPhase, ConstructionHeuristicPhase
    {

        protected readonly EntityPlacer entityPlacer;
        protected readonly ConstructionHeuristicDecider decider;

        private DefaultConstructionHeuristicPhase(Builder builder)
            : base(builder)
        {
            entityPlacer = builder.EntityPlacer;
            decider = builder.Decider;
        }

        public void StepStarted(ConstructionHeuristicStepScope stepScope)
        {
            base.StepStarted(stepScope);
            entityPlacer.StepStarted(stepScope);
            decider.StepStarted(stepScope);
        }

        public void PhaseStarted(ConstructionHeuristicPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            entityPlacer.PhaseStarted(phaseScope);
            decider.PhaseStarted(phaseScope);
        }

        private void DoStep(ConstructionHeuristicStepScope stepScope)
        {
            Move step = stepScope.GetStep();
            step.DoMoveOnly(stepScope.GetScoreDirector());
            PredictWorkingStepScore(stepScope, step);
            solver.GetBestSolutionRecaller().ProcessWorkingSolutionDuringConstructionHeuristicsStep(stepScope);
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            base.SolvingEnded(solverScope);
            entityPlacer.SolvingEnded(solverScope);
            decider.SolvingEnded(solverScope);
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            base.SolvingStarted(solverScope);
            entityPlacer.SolvingStarted(solverScope);
            decider.SolvingStarted(solverScope);
        }

        public override void Solve(SolverScope solverScope)
        {
            ConstructionHeuristicPhaseScope phaseScope = new ConstructionHeuristicPhaseScope(solverScope);
            PhaseStarted(phaseScope);

            foreach (var placement in entityPlacer)
            {

                ConstructionHeuristicStepScope stepScope = new ConstructionHeuristicStepScope(phaseScope);
                StepStarted(stepScope);

                decider.DecideNextStep(stepScope, placement);

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
                                + ") has selected move count (" + stepScope.GetSelectedMoveCount()
                                + ") but failed to pick a nextStep (" + stepScope.GetStep() + ").");
                    }
                    // Although stepStarted has been called, stepEnded is not called for this step
                    break;
                }
                DoStep(stepScope);
               
                StepEnded(stepScope);
                phaseScope.SetLastCompletedStepScope(stepScope);
                if (phaseTermination.IsPhaseTerminated(phaseScope))
                {
                    break;
                }
            }
            PhaseEnded(phaseScope);
        }

        public void PhaseEnded(ConstructionHeuristicPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            // Only update the best solution if the CH made any change.
            if (!phaseScope.GetStartingScore().Equals(phaseScope.GetBestScore()))
            {
                solver.GetBestSolutionRecaller().UpdateBestSolutionAndFire(phaseScope.GetSolverScope());
            }
            entityPlacer.PhaseEnded(phaseScope);
            decider.PhaseEnded(phaseScope);
            phaseScope.EndingNow();

            Log.Debug($@"{logIndentation}Construction Heuristic phase ({phaseIndex}) ended: time spent ({phaseScope.CalculateSolverTimeMillisSpentUpToNow()}), best score ({phaseScope.GetBestScore()}),
                 score calculation speed ({phaseScope.GetPhaseScoreCalculationSpeed()}/sec), step total ({phaseScope.GetNextStepIndex()}).");
        }

        public void StepEnded(ConstructionHeuristicStepScope stepScope)
        {
            base.StepEnded(stepScope);
            entityPlacer.StepEnded(stepScope);
            decider.StepEnded(stepScope);
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public class Builder : AbstrBuilder
        {
            public readonly EntityPlacer EntityPlacer;
            public readonly ConstructionHeuristicDecider Decider;


            public Builder(int phaseIndex, String logIndentation, Termination phaseTermination,
                EntityPlacer entityPlacer, ConstructionHeuristicDecider decider)
                : base(phaseIndex, logIndentation, phaseTermination)
            {

                this.EntityPlacer = entityPlacer;
                this.Decider = decider;
            }

            public override AbstractPhase Build()
            {
                return new DefaultConstructionHeuristicPhase(this);
            }
        }
    }
}
