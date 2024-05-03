using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public interface PropagationQueue : Propagator
    {
        void Insert(ITuple item); // niet zeker of ITuple juist is hier
        void Update(ITuple item);// niet zeker of ITuple juist is hier
        void Retract(ITuple item, TupleState state);
    }
}
