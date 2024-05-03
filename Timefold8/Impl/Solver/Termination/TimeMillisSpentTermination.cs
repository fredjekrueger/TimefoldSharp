using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class TimeMillisSpentTermination : AbstractTermination
    {

        private readonly double timeMillisSpentLimit;

        public TimeMillisSpentTermination(double timeMillisSpentLimit)
        {
            this.timeMillisSpentLimit = timeMillisSpentLimit;
            if (timeMillisSpentLimit < 0L)
            {
                throw new Exception("The timeMillisSpentLimit (" + timeMillisSpentLimit
                        + ") cannot be negative.");
            }
        }

        public override bool IsSolverTerminated(SolverScope solverScope)
        {
            long solverTimeMillisSpent = solverScope.CalculateTimeMillisSpentUpToNow();
            return IsTerminated(solverTimeMillisSpent);
        }

        protected bool IsTerminated(long timeMillisSpent)
        {
            return timeMillisSpent >= timeMillisSpentLimit;
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool IsPhaseTerminated(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override double CalculateSolverTimeGradient(SolverScope solverScope)
        {
            long solverTimeMillisSpent = solverScope.CalculateTimeMillisSpentUpToNow();
            return CalculateTimeGradient(solverTimeMillisSpent);
        }

        protected double CalculateTimeGradient(long timeMillisSpent)
        {
            double timeGradient = timeMillisSpent / ((double)timeMillisSpentLimit);
            return Math.Min(timeGradient, 1.0);
        }
    }
}
