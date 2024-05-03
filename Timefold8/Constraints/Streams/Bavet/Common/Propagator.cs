namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public interface Propagator
    {
        void PropagateRetracts();

        void PropagateUpdates();

        void PropagateInserts();

        void PropagateEverything();
    }
}
