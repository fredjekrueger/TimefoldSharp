using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Solver;
using TimefoldSharp.Core.API.Solver.Change;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Solver.Change;
using TimefoldSharp.Core.Impl.Solver.Random;
using TimefoldSharp.Core.Impl.Solver.Recaller;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Solver.Termination;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Impl.Solver
{
    //OC
    public class DefaultSolver : AbstractSolver
    {
        object lockerSolving = new object();
        bool solving = false;

        protected EnvironmentMode environmentMode;
        protected RandomFactory randomFactory;
        protected int startingSolverCount;

        protected BasicPlumbingTermination basicPlumbingTermination;


        protected readonly SolverScope solverScope;
        private readonly string moveThreadCountDescription;

        public DefaultSolver(EnvironmentMode environmentMode, RandomFactory randomFactory,
            BestSolutionRecaller bestSolutionRecaller,
            BasicPlumbingTermination basicPlumbingTermination, Solver.Termination.Termination termination,
            List<Phase.Phase> phaseList, SolverScope solverScope, string moveThreadCountDescription)
             : base(bestSolutionRecaller, termination, phaseList)
        {

            this.environmentMode = environmentMode;
            this.randomFactory = randomFactory;
            this.basicPlumbingTermination = basicPlumbingTermination;
            this.solverScope = solverScope;
            this.moveThreadCountDescription = moveThreadCountDescription;
        }

        public override API.Score.Score GetBestScore()
        {
            return solverScope.GetBestScore();
        }

        public override bool AddProblemFactChange(ProblemFactChange problemFactChange)
        {
            throw new NotImplementedException();
        }

        public override bool AddProblemFactChanges(List<ProblemFactChange> problemFactChangeList)
        {
            throw new NotImplementedException();
        }

        public override bool TerminateEarly()
        {
            bool terminationEarlySuccessful = basicPlumbingTermination.TerminateEarly();
            return terminationEarlySuccessful;
        }

        public override bool IsSolving()
        {
            lock (lockerSolving)
                return solving;
        }

        public override bool IsEveryProblemFactChangeProcessed()
        {
            return basicPlumbingTermination.IsEveryProblemFactChangeProcessed();
        }

        public override bool IsEveryProblemChangeProcessed()
        {
            return basicPlumbingTermination.IsEveryProblemFactChangeProcessed();
        }

        public override bool IsTerminateEarly()
        {
            return basicPlumbingTermination.IsTerminateEarly();
        }

        public override void AddProblemChange(ProblemChange problemChange)
        {
            throw new NotImplementedException();
        }

        public override void AddProblemChanges(List<ProblemChange> problemChangeList)
        {
            throw new NotImplementedException();
        }

        public override ISolution Solve(ISolution problem)
        {
            if (problem == null)
            {
                throw new Exception("The problem (" + problem + ") must not be null.");
            }

            // No tags for these metrics; they are global
            var stopwatch = new Stopwatch();
            Counter errorCounter = new Counter();

            // Score Calculation Count is specific per solver
            //Metrics.gauge(SolverMetric.SCORE_CALCULATION_COUNT.getMeterId(), solverScope.getMonitoringTags(), solverScope, SolverScope::getScoreCalculationCount);
            //solverScope.getSolverMetricSet().forEach(solverMetric->solverMetric.register(this));

            solverScope.SetBestSolution(problem);
            OuterSolvingStarted(solverScope);
            bool restartSolver = true;
            while (restartSolver)
            {
                stopwatch.Start();
                try
                {
                    SolvingStarted(solverScope);
                    RunPhases(solverScope);
                    SolvingEnded(solverScope);
                }
                catch (Exception e)
                {
                    errorCounter.Increment();
                    SolvingError(solverScope, e);
                    throw e;
                }
                finally
                {
                    stopwatch.Stop();
                    //Metrics.globalRegistry.remove(new Meter.Id(SolverMetric.SCORE_CALCULATION_COUNT.getMeterId(),
                    //solverScope.getMonitoringTags(), null, null, Meter.Type.GAUGE));
                    //solverScope.GetSolverMetricSet().ForEach(solverMetric=>solverMetric.unregister(this));
                }
                restartSolver = CheckProblemFactChanges();
            }
            OuterSolvingEnded(solverScope);
            return solverScope.GetBestSolution();
        }

        public void OuterSolvingEnded(SolverScope solverScope)
        {
            // Must be kept open for doProblemFactChange
            solverScope.ScoreDirector.Dispose();
            Log.Debug($@"Solving ended: time spent ({solverScope.GetTimeMillisSpent()}), best score ({solverScope.GetBestScore()}), score calculation speed ({solverScope.GetScoreCalculationSpeed()}/sec), 
                phase total ({phaseList.Count}), environment mode ({environmentMode}), move thread count ({moveThreadCountDescription}).");

            lock (lockerSolving)
                solving = false;
        }

        private bool CheckProblemFactChanges()
        {
            bool restartSolver = basicPlumbingTermination.WaitForRestartSolverDecision();
            if (!restartSolver)
            {
                return false;
            }
            else
            {
                BlockingCollection<ProblemChangeAdapter> problemFactChangeQueue = basicPlumbingTermination.StartProblemFactChangesProcessing();
                solverScope.SetWorkingSolutionFromBestSolution();

                int stepIndex = 0;
                throw new NotImplementedException();
                //ProblemChangeAdapter problemChangeAdapter = problemFactChangeQueue.poll();
                /*while (problemChangeAdapter != null)
                {
                    problemChangeAdapter.DoProblemChange(solverScope);
                    stepIndex++;
                    problemChangeAdapter = problemFactChangeQueue.poll();
                }
                // All PFCs are processed, fail fast if any of the new facts have null planning IDs.
                InnerScoreDirector < Solution_, Score_> scoreDirector = solverScope.ScoreDirector;
                // Everything is fine, proceed.
                Score<Score_> score = scoreDirector.CalculateScore();
                basicPlumbingTermination.EndProblemFactChangesProcessing();
                bestSolutionRecaller.UpdateBestSolutionAndFireIfInitialized(solverScope);
                return true;*/
            }
        }

        public void OuterSolvingStarted(SolverScope solverScope)
        {
            lock (lockerSolving)
                solving = true;
            basicPlumbingTermination.ResetTerminateEarly();
            solverScope.SetStartingSolverCount(0);
            solverScope.SetWorkingRandom(randomFactory.CreateRandom());
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            solverScope.StartingNow();
            solverScope.ScoreDirector.ResetCalculationCount();
            base.SolvingStarted(solverScope);
            int startingSolverCount = solverScope.GetStartingSolverCount() + 1;
            solverScope.SetStartingSolverCount(startingSolverCount);
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            base.SolvingEnded(solverScope);
            solverScope.EndingNow();
        }

        public int GetStartingSolverCount()
        {
            return startingSolverCount;
        }
    }
}
