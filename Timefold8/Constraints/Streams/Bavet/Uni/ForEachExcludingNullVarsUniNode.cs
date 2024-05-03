using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public sealed class ForEachExcludingNullVarsUniNode<A> : AbstractForEachUniNode<A>
    {

        private readonly Func<A, bool> filter;

        public ForEachExcludingNullVarsUniNode(Type forEachClass, Func<A, bool> filter, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize)
                : base(forEachClass, nextNodesTupleLifecycle, outputStoreSize)
        {
            this.filter = filter;
        }

        public override void Insert(object a)
        {
            if (!filter.Invoke((A)a))
            { // Skip inserting the tuple as it does not pass the filter.
                return;
            }
            base.Insert(a);
        }

        public override void Retract(object a)
        {
            if (!filter.Invoke((A)a))
            { // The tuple was never inserted because it did not pass the filter.
                return;
            }
            base.Retract(a);
        }

        public override void Update(object a)
        {
            UniTuple<A> tuple = null;

            if (!tupleMap.TryGetValue(a, out tuple))
            { // The tuple was never inserted because it did not pass the filter.
                Insert(a);
            }
            else if (filter.Invoke((A)a))
            {
                InnerUpdate(a, tuple);
            }
            else
            {
                base.Retract(a); // Call super.retract() to avoid testing the filter again.
            }
        }
    }
}