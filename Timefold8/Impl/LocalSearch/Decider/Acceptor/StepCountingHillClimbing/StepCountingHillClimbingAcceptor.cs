using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.StepCountingHillClimbing
{
    public class StepCountingHillClimbingAcceptor : AbstractAcceptor
    {

        protected int stepCountingHillClimbingSize = -1;
        protected StepCountingHillClimbingType? stepCountingHillClimbingType;

        protected int count = -1;

        public StepCountingHillClimbingAcceptor(int stepCountingHillClimbingSize, StepCountingHillClimbingType? stepCountingHillClimbingType)
        {
            this.stepCountingHillClimbingSize = stepCountingHillClimbingSize;
            this.stepCountingHillClimbingType = stepCountingHillClimbingType;
            if (stepCountingHillClimbingSize <= 0)
            {
                throw new Exception("The stepCountingHillClimbingSize (" + stepCountingHillClimbingSize
                        + ") cannot be negative or zero.");
            }
            if (stepCountingHillClimbingType == null)
            {
                throw new Exception("The stepCountingHillClimbingType (" + stepCountingHillClimbingType
                        + ") cannot be null.");
            }
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
