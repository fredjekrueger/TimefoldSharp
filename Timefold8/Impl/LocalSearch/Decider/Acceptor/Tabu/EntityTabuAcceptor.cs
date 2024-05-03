using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Decider.Acceptor.Tabu
{
    public class EntityTabuAcceptor : AbstractTabuAcceptor
    {

        public EntityTabuAcceptor(string logIndentation)
                : base(logIndentation)
        {
        }

        public override void SolvingError(SolverScope solverScope, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
