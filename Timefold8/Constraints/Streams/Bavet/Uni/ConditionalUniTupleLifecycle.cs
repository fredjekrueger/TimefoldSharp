using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class ConditionalUniTupleLifecycle<A> : AbstractConditionalTupleLifecycle
    {
        private Func<A, bool> predicate;

        public ConditionalUniTupleLifecycle(Func<A, bool> predicate, TupleLifecycle tupleLifecycle)
            : base(tupleLifecycle)
        {
            this.predicate = predicate;
        }

        protected override bool Test(ITuple tuple)
        {
            var uni = (UniTuple<A>)tuple;
            return predicate(uni.factA);
        }
    }
}
