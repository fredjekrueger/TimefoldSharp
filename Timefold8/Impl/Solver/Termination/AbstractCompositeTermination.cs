using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Solver.Termination
{
    public abstract class AbstractCompositeTermination : AbstractTermination
    {

        protected readonly List<Termination> terminationList;

        protected AbstractCompositeTermination(List<Termination> terminationList)
        {
            this.terminationList = terminationList;
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            foreach (var termination in terminationList)
            {
                termination.SolvingEnded(solverScope);
            }
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            foreach (var termination in terminationList)
            {
                termination.SolvingStarted(solverScope);
            }
        }

        public override void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            foreach (var termination in terminationList)
            {
                termination.PhaseStarted(phaseScope);
            }
        }

        public override void StepStarted(AbstractStepScope stepScope)
        {
            foreach (var termination in terminationList)
            {
                termination.StepStarted(stepScope);
            }
        }

        public override void StepEnded(AbstractStepScope stepScope)
        {
            foreach (var termination in terminationList)
            {
                termination.StepEnded(stepScope);
            }
        }

        public override void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            foreach (var termination in terminationList)
            {
                termination.PhaseEnded(phaseScope);
            }
        }
    }
}
