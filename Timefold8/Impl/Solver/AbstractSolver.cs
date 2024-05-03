using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Solver;
using TimefoldSharp.Core.API.Solver.Change;
using TimefoldSharp.Core.API.Solver.Event;
using TimefoldSharp.Core.Impl.Phase;
using TimefoldSharp.Core.Impl.Phase.Event;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Event;
using TimefoldSharp.Core.Impl.Solver.Recaller;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver
{
    public abstract class AbstractSolver : API.Solver.Solver
    {
        protected BestSolutionRecaller bestSolutionRecaller;
        protected Solver.Termination.Termination solverTermination;
        protected List<Phase.Phase> phaseList;

        protected readonly SolverEventSupport solverEventSupport;
        protected readonly PhaseLifecycleSupport phaseLifecycleSupport = new PhaseLifecycleSupport();

        protected AbstractSolver(BestSolutionRecaller bestSolutionRecaller, Solver.Termination.Termination solverTermination, List<Phase.Phase> phaseList)
        {
            solverEventSupport = new SolverEventSupport(this);
            this.bestSolutionRecaller = bestSolutionRecaller;
            this.solverTermination = solverTermination;
            bestSolutionRecaller.SetSolverEventSupport(solverEventSupport);
            this.phaseList = phaseList;
            phaseList.ForEach(phase => ((AbstractPhase)phase).SetSolver(this));
        }

        public BestSolutionRecaller GetBestSolutionRecaller()
        {
            return bestSolutionRecaller;
        }

        public void AddEventListener(SolverEventListener eventListener)
        {
            throw new NotImplementedException();
        }

        public virtual void SolvingEnded(SolverScope solverScope)
        {
            foreach (var phase in phaseList)
            {
                phase.SolvingEnded(solverScope);
            }
            bestSolutionRecaller.SolvingEnded(solverScope);
            solverTermination.SolvingEnded(solverScope);
            phaseLifecycleSupport.FireSolvingEnded(solverScope);
        }

        protected void RunPhases(SolverScope solverScope)
        {
            if (!solverScope.GetSolutionDescriptor().HasMovableEntities(solverScope.ScoreDirector))
            {
                return;
            }
            var it = phaseList.GetEnumerator();
            bool hasNext = it.MoveNext();
            var current = it.Current;
            while (!solverTermination.IsSolverTerminated(solverScope) && hasNext)
            {
                Phase.Phase phase = current;
                phase.Solve(solverScope);
                // If there is a next phase, it starts from the best solution, which might differ from the working solution.
                // If there isn't, no need to planning clone the best solution to the working solution.
                hasNext = it.MoveNext();
                current = it.Current;
                if (hasNext)
                {
                    solverScope.SetWorkingSolutionFromBestSolution();
                }
            }
        }


        public void StepEnded(AbstractStepScope stepScope)
        {
            bestSolutionRecaller.StepEnded(stepScope);
            phaseLifecycleSupport.FireStepEnded(stepScope);
            solverTermination.StepEnded(stepScope);
            // Do not propagate to phases; the active phase does that for itself and they should not propagate further.
        }

        public void StepStarted(AbstractStepScope stepScope)
        {
            bestSolutionRecaller.StepStarted(stepScope);
            phaseLifecycleSupport.FireStepStarted(stepScope);
            solverTermination.StepStarted(stepScope);
            // Do not propagate to phases; the active phase does that for itself and they should not propagate further.
        }

        public void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            bestSolutionRecaller.PhaseStarted(phaseScope);
            phaseLifecycleSupport.FirePhaseStarted(phaseScope);
            solverTermination.PhaseStarted(phaseScope);
            // Do not propagate to phases; the active phase does that for itself and they should not propagate further.
        }

        public void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            bestSolutionRecaller.PhaseEnded(phaseScope);
            phaseLifecycleSupport.FirePhaseEnded(phaseScope);
            solverTermination.PhaseEnded(phaseScope);
            // Do not propagate to phases; the active phase does that for itself and they should not propagate further.
        }

        public void SolvingError(SolverScope solverScope, Exception exception)
        {
            phaseLifecycleSupport.FireSolvingError(solverScope, exception);
            foreach (var phase in phaseList)
            {
                phase.SolvingError(solverScope, exception);
            }
        }

        public virtual void SolvingStarted(SolverScope solverScope)
        {
            solverScope.SetWorkingSolutionFromBestSolution();
            bestSolutionRecaller.SolvingStarted(solverScope);
            solverTermination.SolvingStarted(solverScope);
            phaseLifecycleSupport.FireSolvingStarted(solverScope);
            foreach (var phase in phaseList)
            {
                phase.SolvingStarted(solverScope);
            }
        }

        public abstract void AddProblemChange(ProblemChange problemChange);

        public abstract void AddProblemChanges(List<ProblemChange> problemChangeList);

        public abstract bool AddProblemFactChange(ProblemFactChange problemFactChange);

        public abstract bool AddProblemFactChanges(List<ProblemFactChange> problemFactChangeList);

        public abstract bool IsEveryProblemChangeProcessed();

        public abstract bool IsEveryProblemFactChangeProcessed();

        public abstract bool IsSolving();

        public abstract bool IsTerminateEarly();

        public void RemoveEventListener(SolverEventListener eventListener)
        {
            throw new NotImplementedException();
        }

        public abstract ISolution Solve(ISolution problem);

        public abstract bool TerminateEarly();

        virtual public API.Score.Score GetBestScore()
        {
            throw new NotImplementedException();
        }
    }
}
