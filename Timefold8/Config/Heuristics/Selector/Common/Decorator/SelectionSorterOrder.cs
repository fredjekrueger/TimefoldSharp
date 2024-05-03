namespace TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator
{
    public enum SelectionSorterOrder
    {
        /**
         * For example: 0, 1, 2, 3.
         */
        ASCENDING,
        /**
         * For example: 3, 2, 1, 0.
         */
        DESCENDING
    }

    public static class SelectionSorterOrderHelper
    {
        public static SelectionSorterOrder Resolve(SelectionSorterOrder? sorterOrder)
        {
            if (sorterOrder == null)
            {
                return SelectionSorterOrder.ASCENDING;
            }
            return sorterOrder.Value;
        }
    }

}
