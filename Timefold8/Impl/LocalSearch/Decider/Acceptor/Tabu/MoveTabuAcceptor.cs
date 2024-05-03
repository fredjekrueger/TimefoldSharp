using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu
{
    public class MoveTabuAcceptor : AbstractTabuAcceptor
    {

        protected bool useUndoMoveAsTabuMove = true;

        public MoveTabuAcceptor(String logIndentation)
                : base(logIndentation)
        {
        }

        public void SetUseUndoMoveAsTabuMove(bool useUndoMoveAsTabuMove)
        {
            this.useUndoMoveAsTabuMove = useUndoMoveAsTabuMove;
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }


    }
}
