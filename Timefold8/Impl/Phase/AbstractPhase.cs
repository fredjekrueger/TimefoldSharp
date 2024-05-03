using TimefoldSharp.Core.Config.Solver.Monitoring;
using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Solver.Termination;
using static TimefoldSharp.Core.Config.Solver.Monitoring.SolverMetric;

namespace TimefoldSharp.Core.Impl.Phase
{
    public abstract class AbstractPhase : Phase
    {
        protected AbstractSolver solver;
        private bool assertStepScoreFromScratch = false;

        protected readonly int phaseIndex;
        protected readonly string logIndentation;

        // Called "phaseTermination" to clearly distinguish from "solverTermination" inside AbstractSolver.
        protected readonly Termination phaseTermination;

        protected readonly bool assertExpectedStepScore;
        protected readonly bool assertShadowVariablesAreNotStaleAfterStep;
        protected PhaseLifecycleSupport phaseLifecycleSupport = new PhaseLifecycleSupport();

        protected AbstractPhase(AbstrBuilder builder)
        {
            phaseIndex = builder.PhaseIndex;
            logIndentation = builder.LogIndentation;
            phaseTermination = builder.PhaseTermination;
            assertStepScoreFromScratch = builder.AssertStepScoreFromScratch;
            assertExpectedStepScore = builder.AssertExpectedStepScore;
            assertShadowVariablesAreNotStaleAfterStep = builder.AssertShadowVariablesAreNotStaleAfterStep;
        }

        protected void PredictWorkingStepScore(AbstractStepScope stepScope, object completedAction)
        {
            AbstractPhaseScope phaseScope = stepScope.GetPhaseScope();
            // There is no need to recalculate the score, but we still need to set it
            phaseScope.GetSolutionDescriptor().SetScore(phaseScope.GetWorkingSolution(), stepScope.GetScore());
            if (assertStepScoreFromScratch)
            {
                phaseScope.AssertPredictedScoreFromScratch(stepScope.GetScore(), completedAction);
            }
            if (assertExpectedStepScore)
            {
                phaseScope.AssertExpectedWorkingScore(stepScope.GetScore(), completedAction);
            }
            if (assertShadowVariablesAreNotStaleAfterStep)
            {
                phaseScope.AssertShadowVariablesAreNotStale(stepScope.GetScore(), completedAction);
            }
        }

        public void SetSolver(AbstractSolver solver)
        {
            this.solver = solver;
        }

        public virtual void SolvingStarted(SolverScope solverScope)
        {
            phaseTermination.SolvingStarted(solverScope);
            phaseLifecycleSupport.FireSolvingStarted(solverScope);
        }

        public virtual void SolvingEnded(SolverScope solverScope)
        {
            phaseTermination.SolvingEnded(solverScope);
            phaseLifecycleSupport.FireSolvingEnded(solverScope);
        }


        public abstract void Solve(SolverScope solverScope);

        public abstract void SolvingError(SolverScope solverScope, Exception exception);

        public virtual void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            phaseScope.StartingNow();
            phaseScope.Reset();
            solver.PhaseStarted(phaseScope);
            phaseTermination.PhaseStarted(phaseScope);
            phaseLifecycleSupport.FirePhaseStarted(phaseScope);
        }

        public virtual void StepStarted(AbstractStepScope stepScope)
        {
            solver.StepStarted(stepScope);
            phaseTermination.StepStarted(stepScope);
            phaseLifecycleSupport.FireStepStarted(stepScope);
        }

        public virtual void StepEnded(AbstractStepScope stepScope)
        {
            solver.StepEnded(stepScope);
            CollectMetrics(stepScope);
            phaseTermination.StepEnded(stepScope);
            phaseLifecycleSupport.FireStepEnded(stepScope);
        }

        private void CollectMetrics(AbstractStepScope stepScope)
        {
            SolverScope solverScope = stepScope.GetPhaseScope().GetSolverScope();
            if (solverScope.IsMetricEnabled(SolverMetric.GetInfo(SolverMetricEnum.STEP_SCORE)) && stepScope.GetScore().IsSolutionInitialized())
            {
                /*SolverMetric.RegisterScoreMetrics(SolverMetricEnum.STEP_SCORE,
                        solverScope.GetMonitoringTags(),
                        solverScope.GetScoreDefinition(),
                        solverScope.GetStepScoreMap(),
                        stepScope.GetScore());*/
            }
        }

        public virtual void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            solver.PhaseEnded(phaseScope);
            phaseTermination.PhaseEnded(phaseScope);
            phaseLifecycleSupport.FirePhaseEnded(phaseScope);
        }

        public abstract class AbstrBuilder
        {
            public readonly int PhaseIndex;
            public readonly string LogIndentation;
            public readonly Termination PhaseTermination;

            public bool AssertStepScoreFromScratch = false;
            public bool AssertExpectedStepScore = false;
            public bool AssertShadowVariablesAreNotStaleAfterStep = false;

            protected AbstrBuilder(int phaseIndex, String logIndentation, Termination phaseTermination)
            {
                this.PhaseIndex = phaseIndex;
                this.LogIndentation = logIndentation;
                this.PhaseTermination = phaseTermination;
            }

            public void SetAssertStepScoreFromScratch(bool assertStepScoreFromScratch)
            {
                this.AssertStepScoreFromScratch = assertStepScoreFromScratch;
            }

            public void SetAssertExpectedStepScore(bool assertExpectedStepScore)
            {
                this.AssertExpectedStepScore = assertExpectedStepScore;
            }

            public void SetAssertShadowVariablesAreNotStaleAfterStep(bool assertShadowVariablesAreNotStaleAfterStep)
            {
                this.AssertShadowVariablesAreNotStaleAfterStep = assertShadowVariablesAreNotStaleAfterStep;
            }

            public abstract AbstractPhase Build();
        }
    }
}