using TimefoldSharp.Core.Impl.LocalSearch.Scope;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor
{
    public class GreatDelugeAcceptor : AbstractAcceptor
    {

        private double? waterLevelIncrementRatio;
        private API.Score.Score waterLevelIncrementScore;
        private API.Score.Score initialWaterLevel;

        public void SetWaterLevelIncrementScore(API.Score.Score waterLevelIncrementScore)
        {
            this.waterLevelIncrementScore = waterLevelIncrementScore;
        }

        public API.Score.Score getInitialWaterLevel()
        {
            return this.initialWaterLevel;
        }

        public void SetInitialWaterLevel(API.Score.Score initialLevel)
        {
            this.initialWaterLevel = initialLevel;
        }

        public double? GetWaterLevelIncrementRatio()
        {
            return this.waterLevelIncrementRatio;
        }

        public void SetWaterLevelIncrementRatio(double? waterLevelIncrementRatio)
        {
            this.waterLevelIncrementRatio = waterLevelIncrementRatio;
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

        public override void StepStarted(LocalSearchStepScope stepScope)
        {
            throw new NotImplementedException();
        }

        public override void StepEnded(LocalSearchStepScope stepScope)
        {
            throw new NotImplementedException();
        }
    }
}
