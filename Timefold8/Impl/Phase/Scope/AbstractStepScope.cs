using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Phase.Scope
{
    public abstract class AbstractStepScope
    {

        protected readonly int stepIndex;
        protected API.Score.Score score = null;
        protected bool bestScoreImproved = false;
        protected ISolution clonedSolution = null;

        public AbstractStepScope(int stepIndex)
        {
            this.stepIndex = stepIndex;
        }
        public int GetStepIndex()
        {
            return stepIndex;
        }

        public Random GetWorkingRandom()
        {
            return GetPhaseScope().GetWorkingRandom();
        }

        public ISolution CreateOrGetClonedSolution()
        {
            if (clonedSolution == null)
            {
                clonedSolution = GetScoreDirector().CloneWorkingSolution();
            }
            return clonedSolution;
        }

        public API.Score.Score GetScore()
        {
            return score;
        }

        public void SetScore(API.Score.Score score)
        {
            this.score = score;
        }

        public void SetBestScoreImproved(bool bestScoreImproved)
        {
            this.bestScoreImproved = bestScoreImproved;
        }

        public abstract AbstractPhaseScope GetPhaseScope();

        public InnerScoreDirector GetScoreDirector()
        {
            return GetPhaseScope().GetScoreDirector();
        }

        public ISolution GetWorkingSolution()
        {
            return GetPhaseScope().GetWorkingSolution();
        }
    }
}
