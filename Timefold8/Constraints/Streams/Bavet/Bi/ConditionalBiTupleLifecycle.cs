using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Bi
{
    public class ConditionalBiTupleLifecycle<A, B> : AbstractConditionalTupleLifecycle
    {
        private Func<A, B, bool> predicate;

        public ConditionalBiTupleLifecycle(Func<A, B, bool> predicate, TupleLifecycle tupleLifecycle)
            : base(tupleLifecycle)
        {
            this.predicate = predicate;
        }

        protected override bool Test(ITuple tuple)
        {
            var bi = (BiTuple<A, B>)tuple;
            return predicate.Invoke(bi.factA, bi.factB);
        }
    }
}
