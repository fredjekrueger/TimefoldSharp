using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    internal class AbstractOriginalChangeIterator<Move> : UpcomingSelectionIterator<Move>
    {

        private ValueSelector valueSelector;

        private IEnumerator<Object> entityIterator;
        private IEnumerator<Object> valueIterator;

        private Object upcomingEntity;


        public AbstractOriginalChangeIterator(EntitySelector entitySelector, ValueSelector valueSelector)
        {
            this.valueSelector = valueSelector;
            entityIterator = entitySelector.GetEnumerator();
            // Don't do hasNext() in constructor (to avoid upcoming selections breaking mimic recording)
            valueIterator = Enumerable.Empty<ValueType>().GetEnumerator();
        }

        protected override Move CreateUpcomingSelection()
        {
            while (!valueIterator.MoveNext())
            {
                if (!entityIterator.MoveNext())
                {
                    return NoUpcomingSelection();
                }
                upcomingEntity = entityIterator.Current;
                valueIterator = valueSelector.Iterator(upcomingEntity);
            }
            Object toValue = valueIterator.Current;
            return NewChangeSelection(upcomingEntity, toValue);
        }

        public Func<object, object, Move> NewChangeSelection;
    }
}
