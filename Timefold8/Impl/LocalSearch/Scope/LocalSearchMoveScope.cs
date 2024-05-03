using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Phase.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Scope
{
    public class LocalSearchMoveScope : AbstractMoveScope
    {
        private readonly LocalSearchStepScope stepScope;
        bool? accepted = null;

        public LocalSearchMoveScope(LocalSearchStepScope stepScope, int moveIndex, Move move)
            : base(moveIndex, move)
        {
            this.stepScope = stepScope;
        }

        public void SetAccepted(bool? accepted)
        {
            this.accepted = accepted;
        }

        public override AbstractStepScope GetStepScope()
        {
            return stepScope;
        }

        public bool? GetAccepted()
        {
            return accepted;
        }
    }
}
