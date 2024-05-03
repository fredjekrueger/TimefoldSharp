using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu.Size;
using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu
{
    public abstract class AbstractTabuAcceptor : AbstractAcceptor
    {
        protected TabuSizeStrategy tabuSizeStrategy = null;
        protected TabuSizeStrategy fadingTabuSizeStrategy = null;
        protected readonly string logIndentation;
        protected bool assertTabuHashCodeCorrectness = false;

        public AbstractTabuAcceptor(string logIndentation)
        {
            this.logIndentation = logIndentation;
        }

        public void SetTabuSizeStrategy(TabuSizeStrategy tabuSizeStrategy)
        {
            this.tabuSizeStrategy = tabuSizeStrategy;
        }

        public void SetFadingTabuSizeStrategy(TabuSizeStrategy fadingTabuSizeStrategy)
        {
            this.fadingTabuSizeStrategy = fadingTabuSizeStrategy;
        }

        public void SetAssertTabuHashCodeCorrectness(bool assertTabuHashCodeCorrectness)
        {
            this.assertTabuHashCodeCorrectness = assertTabuHashCodeCorrectness;
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
