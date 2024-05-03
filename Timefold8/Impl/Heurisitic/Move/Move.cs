using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Move
{
    public interface Move
    {
        void DoMoveOnly(ScoreDirector scoreDirector);
        Move DoMove(ScoreDirector scoreDirector);
        bool IsMoveDoable(ScoreDirector scoreDirector);
    }
}
