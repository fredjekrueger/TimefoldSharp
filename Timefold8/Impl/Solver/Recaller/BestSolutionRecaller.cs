using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Solver.Event;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Recaller
{
    public class BestSolutionRecaller : PhaseLifecycleListenerAdapter
    {
        public bool AssertInitialScoreFromScratch { get; set; }
        public bool AssertShadowVariablesAreNotStale { get; set; }
        public bool AssertBestScoreIsUnmodified { get; set; }

        protected SolverEventSupport solverEventSupport;

        public void SetSolverEventSupport(SolverEventSupport solverEventSupport)
        {
            this.solverEventSupport = solverEventSupport;
        }

        public void ProcessWorkingSolutionDuringStep(AbstractStepScope stepScope)
        {
            AbstractPhaseScope phaseScope = stepScope.GetPhaseScope();
            API.Score.Score score = stepScope.GetScore();
            SolverScope solverScope = phaseScope.GetSolverScope();
            bool bestScoreImproved = score.CompareTo(solverScope.GetBestScore()) > 0;
            stepScope.SetBestScoreImproved(bestScoreImproved);
            if (bestScoreImproved)
            {
                phaseScope.SetBestSolutionStepIndex(stepScope.GetStepIndex());
                ISolution newBestSolution = stepScope.CreateOrGetClonedSolution();
                UpdateBestSolutionAndFire(solverScope, score, newBestSolution);
            }
            else if (AssertBestScoreIsUnmodified)
            {
                solverScope.AssertScoreFromScratch(solverScope.GetBestSolution());
            }
        }

        private void UpdateBestSolutionAndFire(SolverScope solverScope, API.Score.Score bestScore, ISolution bestSolution)
        {
            UpdateBestSolutionWithoutFiring(solverScope, bestScore, bestSolution);
            solverEventSupport.FireBestSolutionChanged(solverScope, solverScope.GetBestSolution());
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            InnerScoreDirector scoreDirector = solverScope.ScoreDirector;
            var score = scoreDirector.CalculateScore();
            solverScope.SetBestScore(score);
            solverScope.SetBestSolutionTimeMillis(DateTime.UtcNow.Ticks);
            // The original bestSolution might be the final bestSolution and should have an accurate Score
            solverScope.GetSolutionDescriptor().SetScore(solverScope.GetBestSolution(), score);
            if (score.IsSolutionInitialized())
            {
                solverScope.SetStartingInitializedScore(score);
            }
            else
            {
                solverScope.SetStartingInitializedScore(null);
            }
            if (AssertInitialScoreFromScratch)
            {
                scoreDirector.AssertWorkingScoreFromScratch(score, "Initial score calculated");
            }
            if (AssertShadowVariablesAreNotStale)
            {
                scoreDirector.AssertShadowVariablesAreNotStale(score, "Initial score calculated");
            }
        }

        public override void SolvingError(SolverScope solverScope, Exception exception) { }

        public void UpdateBestSolutionAndFireIfInitialized(SolverScope solverScope)
        {
            UpdateBestSolutionWithoutFiring(solverScope);
            if (solverScope.IsBestSolutionInitialized())
            {
                solverEventSupport.FireBestSolutionChanged(solverScope, solverScope.GetBestSolution());
            }
        }

        public void UpdateBestSolutionAndFire(SolverScope solverScope)
        {
            UpdateBestSolutionWithoutFiring(solverScope);
            solverEventSupport.FireBestSolutionChanged(solverScope, solverScope.GetBestSolution());
        }

        public void ProcessWorkingSolutionDuringConstructionHeuristicsStep(AbstractStepScope stepScope)
        {
            AbstractPhaseScope phaseScope = stepScope.GetPhaseScope();
            SolverScope solverScope = phaseScope.GetSolverScope();
            stepScope.SetBestScoreImproved(true);
            phaseScope.SetBestSolutionStepIndex(stepScope.GetStepIndex());
            ISolution newBestSolution = stepScope.GetWorkingSolution();
            // Construction heuristics don't fire intermediate best solution changed events.
            // But the best solution and score are updated, so that unimproved* terminations work correctly.
            UpdateBestSolutionWithoutFiring(solverScope, stepScope.GetScore(), newBestSolution);
        }

        private void UpdateBestSolutionWithoutFiring(SolverScope solverScope)
        {
            ISolution newBestSolution = solverScope.ScoreDirector.CloneWorkingSolution();
            API.Score.Score newBestScore = solverScope.GetSolutionDescriptor().GetScore(newBestSolution);
            UpdateBestSolutionWithoutFiring(solverScope, newBestScore, newBestSolution);
        }

        private void UpdateBestSolutionWithoutFiring(SolverScope solverScope, API.Score.Score bestScore, ISolution bestSolution)
        {
            if (bestScore.IsSolutionInitialized())
            {
                if (!solverScope.IsBestSolutionInitialized())
                {
                    solverScope.SetStartingInitializedScore(bestScore);
                }
            }
            solverScope.SetBestSolution(bestSolution);
            solverScope.SetBestScore(bestScore);
            solverScope.SetBestSolutionTimeMillis(DateTime.UtcNow.Ticks);
        }
    }
}
