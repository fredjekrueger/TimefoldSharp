namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public enum TupleState
    {
        CREATING,
        UPDATING,
        OK,
        DYING,
        DEAD,
        ABORTING
    }

    public static class TupleStateHelper
    {
        public static bool IsDirty(TupleState state)
        {
            return state == TupleState.CREATING || state == TupleState.UPDATING || state == TupleState.DYING || state == TupleState.ABORTING;
        }

        public static bool IsActive(TupleState state)
        {
            return state == TupleState.CREATING || state == TupleState.UPDATING || state == TupleState.OK;
        }
    }
}
