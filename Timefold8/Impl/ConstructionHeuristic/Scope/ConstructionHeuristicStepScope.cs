using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Phase.Scope;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Scope
{
    public class ConstructionHeuristicStepScope : AbstractStepScope
    {

        private readonly ConstructionHeuristicPhaseScope phaseScope;
        private Move step = null;
        private long? selectedMoveCount = null;

        public ConstructionHeuristicStepScope(ConstructionHeuristicPhaseScope phaseScope)
            : this(phaseScope, phaseScope.GetNextStepIndex())
        {

        }

        public void SetSelectedMoveCount(long? selectedMoveCount)
        {
            this.selectedMoveCount = selectedMoveCount;
        }

        public override AbstractPhaseScope GetPhaseScope()
        {
            return phaseScope;
        }

        public ConstructionHeuristicStepScope(ConstructionHeuristicPhaseScope phaseScope, int stepIndex)
            : base(stepIndex)
        {
            this.phaseScope = phaseScope;
        }

        public void SetStep(Move step)
        {
            this.step = step;
        }

        public long? GetSelectedMoveCount()
        {
            return selectedMoveCount;
        }

        public Move GetStep()
        {
            return step;
        }
    }
}
