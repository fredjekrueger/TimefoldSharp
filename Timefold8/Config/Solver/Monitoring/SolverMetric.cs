namespace TimefoldSharp.Core.Config.Solver.Monitoring
{
    public class SolverMetric
    {
        public enum SolverMetricEnum
        {
            SOLVE_DURATION,
            ERROR_COUNT,
            BEST_SCORE,
            STEP_SCORE,
            SCORE_CALCULATION_COUNT,
            BEST_SOLUTION_MUTATION,
            MOVE_COUNT_PER_STEP,
            MEMORY_USE,
            CONSTRAINT_MATCH_TOTAL_BEST_SCORE,
            CONSTRAINT_MATCH_TOTAL_STEP_SCORE,
            PICKED_MOVE_TYPE_BEST_SCORE_DIFF,
            PICKED_MOVE_TYPE_STEP_SCORE_DIFF
        }

        public string MeterId { get; set; }
        public bool IsBestSolutionBased { get; set; }
        public Func<object, Task> RegisterFunction { get; set; }
        SolverMetricEnum metric;

        public SolverMetric(string meterId, Func<object, Task> registerFunction, bool isBestSolutionBased, SolverMetricEnum metric)
        {
            MeterId = meterId;
            RegisterFunction = registerFunction;
            IsBestSolutionBased = isBestSolutionBased;
            this.metric = metric;
        }

        public static SolverMetric GetInfo(SolverMetricEnum metric)
        {
            switch (metric)
            {
                case SolverMetricEnum.SOLVE_DURATION:
                    return new SolverMetric("timefold.solver.solve.duration", null, false, metric);

                case SolverMetricEnum.ERROR_COUNT:
                    return new SolverMetric("timefold.solver.errors", null, false, metric);

                case SolverMetricEnum.BEST_SCORE:
                    return new SolverMetric("timefold.solver.best.score", /*new BestScoreStatistic<object>()*/ null, true, metric);

                case SolverMetricEnum.STEP_SCORE:
                    return new SolverMetric("timefold.solver.step.score", null, false, metric);

                case SolverMetricEnum.SCORE_CALCULATION_COUNT:
                    return new SolverMetric("timefold.solver.score.calculation.count", null, false, metric);

                case SolverMetricEnum.BEST_SOLUTION_MUTATION:
                    return new SolverMetric("timefold.solver.best.solution.mutation", /*new BestSolutionMutationCountStatistic<object>()*/ null, true, metric);

                case SolverMetricEnum.MOVE_COUNT_PER_STEP:
                    return new SolverMetric("timefold.solver.step.move.count", null, false, metric);

                case SolverMetricEnum.MEMORY_USE:
                    return new SolverMetric("jvm.memory.used", /*new MemoryUseStatistic<object>()*/ null, false, metric);

                case SolverMetricEnum.CONSTRAINT_MATCH_TOTAL_BEST_SCORE:
                    return new SolverMetric("timefold.solver.constraint.match.best.score", null, true, metric);

                case SolverMetricEnum.CONSTRAINT_MATCH_TOTAL_STEP_SCORE:
                    return new SolverMetric("timefold.solver.constraint.match.step.score", null, false, metric);

                case SolverMetricEnum.PICKED_MOVE_TYPE_BEST_SCORE_DIFF:
                    return new SolverMetric("timefold.solver.move.type.best.score.diff", /*new PickedMoveBestScoreDiffStatistic<object>()*/ null, true, metric);

                case SolverMetricEnum.PICKED_MOVE_TYPE_STEP_SCORE_DIFF:
                    return new SolverMetric("timefold.solver.move.type.step.score.diff", /*new PickedMoveStepScoreDiffStatistic<object>()*/ null, false, metric);

                default:
                    throw new ArgumentException("Invalid SolverMetric value.");
            }
        }

    }
}
