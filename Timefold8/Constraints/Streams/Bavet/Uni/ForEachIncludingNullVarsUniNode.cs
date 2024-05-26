using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public sealed class ForEachIncludingNullVarsUniNode<A> : AbstractForEachUniNode<A>
    {

        public ForEachIncludingNullVarsUniNode(Type forEachClass, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize)
                : base(forEachClass, nextNodesTupleLifecycle, outputStoreSize)
        {
        }

        public override void Update(object a)
        {
            UniTuple<A> tuple = tupleMap[a];
            if (tuple == null)
            {
                throw new Exception("The fact (" + a + ") was never inserted, so it cannot update.");
            }
            InnerUpdate(a, tuple);
        }

    }
}
