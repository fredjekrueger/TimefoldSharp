using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class UnimprovedTimeMillisSpentScoreDifferenceThresholdTermination : AbstractTermination
    {

        private readonly double unimprovedTimeMillisSpentLimit;
        private readonly API.Score.Score unimprovedScoreDifferenceThreshold;
        private readonly DateTime clock;

        private Queue<Pair<long?, API.Score.Score>> bestScoreImprovementHistoryQueue;
        // safeTimeMillis is until when we're safe from termination
        private long solverSafeTimeMillis = -1L;
        private long phaseSafeTimeMillis = -1L;

        public UnimprovedTimeMillisSpentScoreDifferenceThresholdTermination(double unimprovedTimeMillisSpentLimit, API.Score.Score unimprovedScoreDifferenceThreshold)
                : this(unimprovedTimeMillisSpentLimit, unimprovedScoreDifferenceThreshold, DateTime.UtcNow)
        {
        }

        protected UnimprovedTimeMillisSpentScoreDifferenceThresholdTermination(double unimprovedTimeMillisSpentLimit, API.Score.Score unimprovedScoreDifferenceThreshold, DateTime clock)
        {
            this.unimprovedTimeMillisSpentLimit = unimprovedTimeMillisSpentLimit;
            this.unimprovedScoreDifferenceThreshold = unimprovedScoreDifferenceThreshold;
            if (unimprovedTimeMillisSpentLimit < 0L)
            {
                throw new Exception("The unimprovedTimeMillisSpentLimit (" + unimprovedTimeMillisSpentLimit
                        + ") cannot be negative.");
            }
            this.clock = clock;
        }

        public override double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override double CalculateSolverTimeGradient(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public override bool IsPhaseTerminated(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override bool IsSolverTerminated(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
