using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Phase.Scope
{
    public abstract class AbstractMoveScope
    {
        protected readonly int moveIndex;
        protected readonly Move move;

        protected API.Score.Score score = null;

        public AbstractMoveScope(int moveIndex, Move move)
        {
            this.moveIndex = moveIndex;
            this.move = move;
        }

        public void SetScore(API.Score.Score score)
        {
            this.score = score;
        }

        public InnerScoreDirector GetScoreDirector()
        {
            return GetStepScope().GetScoreDirector();
        }

        public Move GetMove()
        {
            return move;
        }

        public API.Score.Score GetScore()
        {
            return score;
        }

        public abstract AbstractStepScope GetStepScope();
    }
}
