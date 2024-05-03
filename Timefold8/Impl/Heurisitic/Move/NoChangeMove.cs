using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Move
{
    public class NoChangeMove : AbstractMove
    {
        public override bool IsMoveDoable(ScoreDirector scoreDirector)
        {
            throw new NotImplementedException();
        }

        protected override AbstractMove CreateUndoMove(ScoreDirector scoreDirector)
        {
            throw new NotImplementedException();
        }

        protected override void DoMoveOnGenuineVariables(ScoreDirector scoreDirector)
        {
            throw new NotImplementedException();
        }
    }
}
