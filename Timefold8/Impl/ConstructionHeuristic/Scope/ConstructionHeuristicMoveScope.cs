using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Phase.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope
{
    public class ConstructionHeuristicMoveScope : AbstractMoveScope
    {
        private readonly ConstructionHeuristicStepScope stepScope;

        public ConstructionHeuristicMoveScope(ConstructionHeuristicStepScope stepScope, int moveIndex, Move move)
            : base(moveIndex, move)
        {
            this.stepScope = stepScope;
        }

        public override AbstractStepScope GetStepScope()
        {
            return stepScope;
        }
    }
}
