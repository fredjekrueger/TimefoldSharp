using TimefoldSharp.Core.Impl.LocalSearch.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Forager.Finalist
{
    public sealed class StrategicOscillationByLevelFinalistPodium : AbstractFinalistPodium
    {

        readonly bool referenceBestScoreInsteadOfLastStepScore;

        public StrategicOscillationByLevelFinalistPodium(bool referenceBestScoreInsteadOfLastStepScore)
        {
            this.referenceBestScoreInsteadOfLastStepScore = referenceBestScoreInsteadOfLastStepScore;
        }


        public override void AddMove(LocalSearchMoveScope moveScope)
        {
            throw new NotImplementedException();
        }
    }
}
