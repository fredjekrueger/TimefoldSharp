using TimefoldSharp.Core.Impl.Heurisitic.Selector.AbstractSelectors;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator
{
    public class SelectedCountLimitMoveSelector : AbstractMoveSelector
    {

        protected readonly MoveSelector childMoveSelector;
        protected readonly long selectedCountLimit;

        public SelectedCountLimitMoveSelector(MoveSelector childMoveSelector, long selectedCountLimit)
        {
            this.childMoveSelector = childMoveSelector;
            this.selectedCountLimit = selectedCountLimit;
            if (selectedCountLimit < 0L)
            {
                throw new Exception("The selector (" + this
                        + ") has a negative selectedCountLimit (" + selectedCountLimit + ").");
            }
            phaseLifecycleSupport.AddEventListener(childMoveSelector);
        }

        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override long GetSize()
        {
            throw new NotImplementedException();
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            throw new NotImplementedException();
        }
    }
}
