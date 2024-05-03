using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move
{
    public sealed class DoableMoveSelectionFilter : SelectionFilter<Core.Impl.Heurisitic.Move.Move>
    {

        public static readonly SelectionFilter<Core.Impl.Heurisitic.Move.Move> INSTANCE = new DoableMoveSelectionFilter();

        public override bool Accept(ScoreDirector scoreDirector, Core.Impl.Heurisitic.Move.Move move)
        {
            return move.IsMoveDoable(scoreDirector);
        }
    }
}
