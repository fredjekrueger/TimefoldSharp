using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public interface PropagationQueue<T> : Propagator
    {
        void Insert(T item);
        void Update(T item);
        void Retract(T item, TupleState state);
    }
}