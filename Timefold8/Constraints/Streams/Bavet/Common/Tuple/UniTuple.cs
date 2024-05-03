namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public class UniTuple<A> : AbstractTuple
    {
        public A factA;

        public UniTuple(A factA, int storeSize)
            : base(storeSize)
        {
            this.factA = factA;
        }

        public override string ToString()
        {
            return "{" + factA + "}";
        }
    }
}
