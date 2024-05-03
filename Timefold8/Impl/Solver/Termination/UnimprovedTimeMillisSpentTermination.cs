using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class UnimprovedTimeMillisSpentTermination : AbstractTermination
    {

        private readonly double unimprovedTimeMillisSpentLimit;

        private readonly DateTime clock;

        public UnimprovedTimeMillisSpentTermination(double unimprovedTimeMillisSpentLimit)
                : this(unimprovedTimeMillisSpentLimit, DateTime.UtcNow)
        {
        }

        protected UnimprovedTimeMillisSpentTermination(double unimprovedTimeMillisSpentLimit, DateTime clock)
        {
            this.unimprovedTimeMillisSpentLimit = unimprovedTimeMillisSpentLimit;
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
