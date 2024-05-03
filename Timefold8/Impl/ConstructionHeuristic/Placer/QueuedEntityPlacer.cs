using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public class QueuedEntityPlacer : AbstractEntityPlacer, EntityPlacer
    {

        protected readonly EntitySelector entitySelector;
        protected readonly List<MoveSelector> moveSelectorList;

        public QueuedEntityPlacer(EntitySelector entitySelector, List<MoveSelector> moveSelectorList)
        {
            this.entitySelector = entitySelector;
            this.moveSelectorList = moveSelectorList;
            phaseLifecycleSupport.AddEventListener(entitySelector);
            foreach (var moveSelector in moveSelectorList)
            {
                phaseLifecycleSupport.AddEventListener(moveSelector);
            }
        }

        public override IEnumerator<Placement> GetEnumerator()
        {
            return new QueuedEntityPlacingIterator(entitySelector.GetEnumerator(), moveSelectorList);
        }


        protected class QueuedEntityPlacingIterator : UpcomingSelectionIterator<Placement>
        {
            private readonly IEnumerator<object> entityIterator;
            private IEnumerator<MoveSelector> moveSelectorIterator;
            List<MoveSelector> moveSelectorList;

            public QueuedEntityPlacingIterator(IEnumerator<object> entityIterator, List<MoveSelector> moveSelectorList)
            {
                this.entityIterator = entityIterator;
                this.moveSelectorIterator = Enumerable.Empty<MoveSelector>().GetEnumerator();
                this.moveSelectorList = moveSelectorList;
            }

            protected override Placement CreateUpcomingSelection()
            {
                IEnumerator<Move> moveIterator = null;
                // Skip empty placements to avoid no-operation steps
                while (moveIterator == null || !moveIterator.MoveNext())
                {
                    // If a moveSelector's iterator is empty, it might not be empty the next time
                    // (because the entity changes)
                    while (!moveSelectorIterator.MoveNext())
                    {
                        if (!entityIterator.MoveNext())
                        {
                            return NoUpcomingSelection();
                        }
                        var dummy = entityIterator.Current;
                        moveSelectorIterator = moveSelectorList.GetEnumerator();
                    }
                    MoveSelector moveSelector = moveSelectorIterator.Current;
                    moveIterator = moveSelector.GetEnumerator();
                }
                return new Placement(moveIterator);
            }
        }
    }
}
