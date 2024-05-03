using TimefoldSharp.Core.Impl.Heurisitic.Selector.AbstractSelectors;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator
{
    public sealed class FilteringMoveSelector : AbstractMoveSelector
    {

        private readonly MoveSelector childMoveSelector;
        private readonly SelectionFilter<Heurisitic.Move.Move> filter;
        private readonly bool bailOutEnabled;

        private ScoreDirector scoreDirector = null;

        public FilteringMoveSelector(MoveSelector childMoveSelector,
                SelectionFilter<Heurisitic.Move.Move> filter)
        {
            this.childMoveSelector = childMoveSelector;
            this.filter = filter;
            bailOutEnabled = childMoveSelector.IsNeverEnding();
            phaseLifecycleSupport.AddEventListener(childMoveSelector);
        }

        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
        {
            return new JustInTimeFilteringMoveIterator(childMoveSelector.GetEnumerator(), DetermineBailOutSize(), this);
        }

        public override void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            scoreDirector = phaseScope.GetScoreDirector();
        }

        public override void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            scoreDirector = null;
        }

        private long DetermineBailOutSize()
        {
            if (!bailOutEnabled)
            {
                return -1L;
            }
            try
            {
                return childMoveSelector.GetSize() * 10L;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return "Filtering(" + childMoveSelector + ")";
        }

        public override bool IsCountable()
        {
            return childMoveSelector.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return childMoveSelector.IsNeverEnding();
        }

        public override long GetSize()
        {
            throw new NotImplementedException();
        }


        private class JustInTimeFilteringMoveIterator : UpcomingSelectionIterator<Heurisitic.Move.Move>
        {
            private IEnumerator<Heurisitic.Move.Move> childMoveIterator;
            private long bailOutSize;
            FilteringMoveSelector parent;

            public JustInTimeFilteringMoveIterator(IEnumerator<Heurisitic.Move.Move> childMoveIterator, long bailOutSize, FilteringMoveSelector parent)
            {
                this.childMoveIterator = childMoveIterator;
                this.bailOutSize = bailOutSize;
                this.parent = parent;
            }

            private bool Accept(ScoreDirector scoreDirector, Heurisitic.Move.Move move)
            {
                if (parent.filter != null)
                {
                    if (!parent.filter.Accept(scoreDirector, move))
                    {
                        return false;
                    }
                }
                return true;
            }

            protected override Heurisitic.Move.Move CreateUpcomingSelection()
            {
                Heurisitic.Move.Move next;
                long attemptsBeforeBailOut = bailOutSize;
                do
                {
                    if (!childMoveIterator.MoveNext())
                    {
                        return NoUpcomingSelection();
                    }
                    if (parent.bailOutEnabled)
                    {
                        // if childMoveIterator is neverEnding and nothing is accepted, bail out of the infinite loop
                        if (attemptsBeforeBailOut <= 0L)
                        {
                            return NoUpcomingSelection();
                        }
                        attemptsBeforeBailOut--;
                    }
                    next = childMoveIterator.Current;
                } while (!Accept(parent.scoreDirector, next));
                return next;
            }
        }
    }
}
