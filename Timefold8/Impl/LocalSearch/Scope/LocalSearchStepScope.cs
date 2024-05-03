using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Phase.Scope;

namespace TimefoldSharp.Core.Impl.LocalSearch.Scope
{
    public class LocalSearchStepScope : AbstractStepScope
    {
        private readonly LocalSearchPhaseScope phaseScope;
        private Move step = null;
        private Move undoStep = null;
        private double timeGradient = double.NaN;
        private long? selectedMoveCount = null;
        private long? acceptedMoveCount = null;

        public LocalSearchStepScope(LocalSearchPhaseScope phaseScope)
            : this(phaseScope, phaseScope.GetNextStepIndex())
        {
        }

        public void SetStep(Move step)
        {
            this.step = step;
        }

        public void SetUndoStep(Move undoStep)
        {
            this.undoStep = undoStep;
        }

        public long? GetSelectedMoveCount()
        {
            return selectedMoveCount;
        }

        public void SetSelectedMoveCount(long? selectedMoveCount)
        {
            this.selectedMoveCount = selectedMoveCount;
        }

        public void SetAcceptedMoveCount(long? acceptedMoveCount)
        {
            this.acceptedMoveCount = acceptedMoveCount;
        }

        public Move GetStep()
        {
            return step;
        }

        public void SetTimeGradient(double timeGradient)
        {
            this.timeGradient = timeGradient;
        }

        public LocalSearchStepScope(LocalSearchPhaseScope phaseScope, int stepIndex)
            : base(stepIndex)
        {
            this.phaseScope = phaseScope;
        }

        public override AbstractPhaseScope GetPhaseScope()
        {
            return phaseScope;
        }
    }
}
