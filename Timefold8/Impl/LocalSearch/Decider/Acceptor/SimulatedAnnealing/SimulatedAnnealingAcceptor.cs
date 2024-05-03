using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.SimulatedAnnealing
{
    public class SimulatedAnnealingAcceptor : AbstractAcceptor
    {

        protected API.Score.Score startingTemperature;

        public void SetStartingTemperature(API.Score.Score startingTemperature)
        {
            this.startingTemperature = startingTemperature;
        }

        public override bool IsAccepted(LocalSearchMoveScope moveScope)
        {
            throw new NotImplementedException();
        }

        public override void PhaseEnded(LocalSearchPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override void PhaseStarted(LocalSearchPhaseScope phaseScope)
        {
            throw new NotImplementedException();
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            throw new NotImplementedException();
        }

        public override void StepEnded(LocalSearchStepScope stepScope)
        {
            throw new NotImplementedException();
        }

        public override void StepStarted(LocalSearchStepScope stepScope)
        {
            throw new NotImplementedException();
        }
    }
}
