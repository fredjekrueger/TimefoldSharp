namespace TimefoldSharp.Core.Config.Heuristics.Selector.Common
{
    public enum SelectionOrder
    {
        INHERIT,
        /**
         * Select the elements in original order.
         */
        ORIGINAL,
        /**
         * Select in sorted order by sorting the elements.
         * Each element will be selected exactly once (if all elements end up being selected).
         * Requires {@link SelectionCacheType#STEP} or higher.
         */
        SORTED,
        /**
         * Select in random order, without shuffling the elements.
         * Each element might be selected multiple times.
         * Scales well because it does not require caching.
         */
        RANDOM,
        /**
         * Select in random order by shuffling the elements when a selection iterator is created.
         * Each element will be selected exactly once (if all elements end up being selected).
         * Requires {@link SelectionCacheType#STEP} or higher.
         */
        SHUFFLED,
        /**
         * Select in random order, based on the selection probability of each element.
         * Elements with a higher probability have a higher chance to be selected than elements with a lower probability.
         * Each element might be selected multiple times.
         * Requires {@link SelectionCacheType#STEP} or higher.
         */
        PROBABILISTIC
    }

    public static class SelectionOrderHelper
    {
        public static SelectionOrder? Resolve(SelectionOrder? selectionOrder, SelectionOrder? inheritedSelectionOrder)
        {
            if (selectionOrder == null || selectionOrder == SelectionOrder.INHERIT)
            {
                if (inheritedSelectionOrder == null)
                {
                    throw new Exception("The inheritedSelectionOrder (" + inheritedSelectionOrder
                            + ") cannot be null.");
                }
                return inheritedSelectionOrder;
            }
            return selectionOrder;
        }



        public static SelectionOrder FromRandomSelectionBoolean(bool randomSelection)
        {
            return randomSelection ? SelectionOrder.RANDOM : SelectionOrder.ORIGINAL; ;
        }

        internal static bool ToRandomSelectionBoolean(SelectionOrder? resolvedSelectionOrder)
        {
            if (resolvedSelectionOrder == null)
                throw new Exception("blah");
            if (resolvedSelectionOrder.Value == SelectionOrder.RANDOM)
            {
                return true;
            }
            else if (resolvedSelectionOrder == SelectionOrder.ORIGINAL)
            {
                return false;
            }
            else
            {
                throw new Exception("The selectionOrder (" + resolvedSelectionOrder
                        + ") cannot be casted to a randomSelectionBoolean.");
            }
        }
    }
}

