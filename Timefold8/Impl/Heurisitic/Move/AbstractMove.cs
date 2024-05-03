using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Move
{
    public abstract class AbstractMove : Move
    {
        public void DoMoveOnly(ScoreDirector scoreDirector)
        {
            DoMoveOnGenuineVariables(scoreDirector);
            scoreDirector.TriggerVariableListeners();
        }

        public Move DoMove(ScoreDirector scoreDirector)
        {
            AbstractMove undoMove = CreateUndoMove(scoreDirector);
            DoMoveOnly(scoreDirector);
            return undoMove;
        }

        protected abstract void DoMoveOnGenuineVariables(ScoreDirector scoreDirector);

        protected abstract AbstractMove CreateUndoMove(ScoreDirector scoreDirector);

        public abstract bool IsMoveDoable(ScoreDirector scoreDirector);
    }
}
