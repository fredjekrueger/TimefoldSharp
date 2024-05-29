using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move
{
    public sealed class DoableMoveSelectionFilter : SelectionFilter<Heurisitic.Move.Move>
    {

        public DoableMoveSelectionFilter()
        {
            Accept = AcceptInt;
        }

        public static readonly SelectionFilter<Core.Impl.Heurisitic.Move.Move> INSTANCE = new DoableMoveSelectionFilter();

        public override Func<ScoreDirector, Heurisitic.Move.Move, bool> Accept { get; set; }

        public bool AcceptInt(ScoreDirector scoreDirector, Core.Impl.Heurisitic.Move.Move move)
        {
            return move.IsMoveDoable(scoreDirector);
        }
    }
}
