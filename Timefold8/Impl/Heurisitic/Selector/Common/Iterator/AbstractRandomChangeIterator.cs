using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    internal class AbstractRandomChangeIterator<Move> : UpcomingSelectionIterator<Move>
    {

        private EntitySelector entitySelector;
        private ValueSelector valueSelector;

        private IEnumerator<Object> entityIterator;


        public AbstractRandomChangeIterator(EntitySelector entitySelector, ValueSelector valueSelector)
        {
            this.entitySelector = entitySelector;
            this.valueSelector = valueSelector;
            entityIterator = entitySelector.GetEnumerator();
            // Don't do hasNext() in constructor (to avoid upcoming selections breaking mimic recording)
        }

        protected override Move CreateUpcomingSelection()
        {
            if (!entityIterator.MoveNext())
            {
                entityIterator = entitySelector.GetEnumerator();
                if (!entityIterator.MoveNext())
                {
                    return NoUpcomingSelection();
                }
            }
            object entity = entityIterator.Current;

            var valueIterator = valueSelector.Iterator(entity);
            int entityIteratorCreationCount = 0;
            // This loop is mostly only relevant when the entityIterator or valueIterator is non-random or shuffled
            while (!valueIterator.MoveNext())
            {
                // Try the next entity
                if (!entityIterator.MoveNext())
                {
                    entityIterator = entitySelector.GetEnumerator();
                    entityIteratorCreationCount++;
                    if (entityIteratorCreationCount >= 2)
                    {
                        // All entity-value combinations have been tried (some even more than once)
                        return NoUpcomingSelection();
                    }
                }
                entity = entityIterator.Current;
                valueIterator = valueSelector.Iterator(entity);
            }
            Object toValue = valueIterator.Current;
            return NewChangeSelection(entity, toValue);
        }

        public Func<object, object, Move> NewChangeSelection;
    }
}
