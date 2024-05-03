using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.StepCountingHillClimbing;

namespace TimefoldSharp.Core.Config.LocalSearch.Decider.Acceptor
{
    public class LocalSearchAcceptorConfig : AbstractConfig<LocalSearchAcceptorConfig>
    {

        private List<AcceptorType> acceptorTypeList = null;
        protected int? stepCountingHillClimbingSize = null;
        protected StepCountingHillClimbingType? stepCountingHillClimbingType = null;
        protected int? entityTabuSize = null;
        protected double? entityTabuRatio = null;
        protected int? fadingEntityTabuSize = null;
        protected double? fadingEntityTabuRatio = null;
        protected int? valueTabuSize = null;
        protected double? valueTabuRatio = null;
        protected int? fadingValueTabuSize = null;
        protected double? fadingValueTabuRatio = null;
        protected int? moveTabuSize = null;
        protected int? fadingMoveTabuSize = null;
        protected int? undoMoveTabuSize = null;
        protected int? fadingUndoMoveTabuSize = null;

        protected string simulatedAnnealingStartingTemperature = null;

        protected int? lateAcceptanceSize = null;

        protected string greatDelugeWaterLevelIncrementScore = null;
        protected double? greatDelugeWaterLevelIncrementRatio = null;


        public LocalSearchAcceptorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public int? GetLateAcceptanceSize()
        {
            return lateAcceptanceSize;
        }

        public string GetSimulatedAnnealingStartingTemperature()
        {
            return simulatedAnnealingStartingTemperature;
        }

        public string GetGreatDelugeWaterLevelIncrementScore()
        {
            return greatDelugeWaterLevelIncrementScore;
        }

        public double? GetGreatDelugeWaterLevelIncrementRatio()
        {
            return greatDelugeWaterLevelIncrementRatio;
        }

        public void SetGreatDelugeWaterLevelIncrementRatio(double? greatDelugeWaterLevelIncrementRatio)
        {
            this.greatDelugeWaterLevelIncrementRatio = greatDelugeWaterLevelIncrementRatio;
        }

        public void SetGreatDelugeWaterLevelIncrementScore(string greatDelugeWaterLevelIncrementScore)
        {
            this.greatDelugeWaterLevelIncrementScore = greatDelugeWaterLevelIncrementScore;
        }

        public LocalSearchAcceptorConfig Inherit(LocalSearchAcceptorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public void SetAcceptorTypeList(List<AcceptorType> acceptorTypeList)
        {
            this.acceptorTypeList = acceptorTypeList;
        }

        public List<AcceptorType> GetAcceptorTypeList()
        {
            return acceptorTypeList;
        }

        public int? GetStepCountingHillClimbingSize()
        {
            return stepCountingHillClimbingSize;
        }

        public int? GetUndoMoveTabuSize()
        {
            return undoMoveTabuSize;
        }

        public int? GetFadingUndoMoveTabuSize()
        {
            return fadingUndoMoveTabuSize;
        }

        public StepCountingHillClimbingType? GetStepCountingHillClimbingType()
        {
            return stepCountingHillClimbingType;
        }

        public int? GetEntityTabuSize()
        {
            return entityTabuSize;
        }

        public int? GetFadingEntityTabuSize()
        {
            return fadingEntityTabuSize;
        }

        public double? GetEntityTabuRatio()
        {
            return entityTabuRatio;
        }

        public double? GetFadingEntityTabuRatio()
        {
            return fadingEntityTabuRatio;
        }

        public int? GetValueTabuSize()
        {
            return valueTabuSize;
        }

        public int? GetFadingValueTabuSize()
        {
            return fadingValueTabuSize;
        }

        public double? GetFadingValueTabuRatio()
        {
            return fadingValueTabuRatio;
        }

        public double? GetValueTabuRatio()
        {
            return valueTabuRatio;
        }

        public int? GetMoveTabuSize()
        {
            return moveTabuSize;
        }

        public int? GetFadingMoveTabuSize()
        {
            return fadingMoveTabuSize;
        }
    }
}
