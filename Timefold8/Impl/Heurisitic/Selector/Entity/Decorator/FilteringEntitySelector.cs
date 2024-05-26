using Serilog.Core;
using System.Collections;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public sealed class FilteringEntitySelector : AbstractDemandEnabledSelector, EntitySelector
    {

        private readonly EntitySelector childEntitySelector;
        private readonly SelectionFilter<Object> selectionFilter;
        private readonly bool bailOutEnabled;
        private ScoreDirector scoreDirector = null;

        public FilteringEntitySelector(EntitySelector childEntitySelector, List<SelectionFilter<object>> filterList)
        {
            this.childEntitySelector = childEntitySelector;
            if (filterList == null || filterList.Count == 0)
                throw new Exception("must have at least one filter, but got (" + filterList + ").");

            this.selectionFilter = SelectionFilter<object>.Compose(filterList);
            bailOutEnabled = childEntitySelector.IsNeverEnding();
            phaseLifecycleSupport.AddEventListener(childEntitySelector);
        }

        public IEnumerator<object> EndingIterator()
        {
            throw new NotImplementedException();
        }

        public EntityDescriptor GetEntityDescriptor()
        {
            return childEntitySelector.GetEntityDescriptor();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return new JustInTimeFilteringEntityIterator(childEntitySelector.GetEnumerator(), DetermineBailOutSize(), this);
        }

        private long DetermineBailOutSize()
        {
            if (!bailOutEnabled)
            {
                return -1L;
            }
            return childEntitySelector.GetSize() * 10L;
        }

        public long GetSize()
        {
            return childEntitySelector.GetSize();
        }

        public override bool IsCountable()
        {
            return childEntitySelector.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return childEntitySelector.IsNeverEnding();
        }

        public IEnumerator<object> ListIterator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
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


        public class JustInTimeFilteringEntityIterator : UpcomingSelectionIterator<object>
        {

            private IEnumerator<object> childEntityIterator;
            private long bailOutSize;
            FilteringEntitySelector parent;

            public JustInTimeFilteringEntityIterator(IEnumerator<object> childEntityIterator, long bailOutSize, FilteringEntitySelector parent)
            {
                this.childEntityIterator = childEntityIterator;
                this.bailOutSize = bailOutSize;
                this.parent = parent;
            }

            protected override object CreateUpcomingSelection()
            {
                object next;
                long attemptsBeforeBailOut = bailOutSize;
                do
                {
                    if (!childEntityIterator.MoveNext())
                    {
                        return NoUpcomingSelection();
                    }
                    if (parent.bailOutEnabled)
                    {
                        // if childEntityIterator is neverEnding and nothing is accepted, bail out of the infinite loop
                        if (attemptsBeforeBailOut <= 0L)
                        {
                            return NoUpcomingSelection();
                        }
                        attemptsBeforeBailOut--;
                    }
                    next = childEntityIterator.Current;
                }
                while (!parent.selectionFilter.Accept(parent.scoreDirector, next));
                return next;
            }
        }
    }
}
