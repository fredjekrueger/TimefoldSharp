using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public class OrCompositeTermination : AbstractCompositeTermination
    {
        public OrCompositeTermination(List<Termination> terminationList)
            : base(terminationList)
        {
        }

        public override double CalculatePhaseTimeGradient(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override double CalculateSolverTimeGradient(SolverScope solverScope)
        {
            double timeGradient = 0.0;
            foreach (var termination in terminationList)
            {
                double nextTimeGradient = termination.CalculateSolverTimeGradient(solverScope);
                if (nextTimeGradient >= 0.0)
                {
                    timeGradient = Math.Max(timeGradient, nextTimeGradient);
                }
            }
            return timeGradient;
        }

        public override bool IsPhaseTerminated(AbstractPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override bool IsSolverTerminated(SolverScope solverScope)
        {
            foreach (var termination in terminationList)
            {
                if (termination.IsSolverTerminated(solverScope))
                {
                    return true;
                }
            }
            return false;
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
