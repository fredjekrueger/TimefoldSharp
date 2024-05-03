namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public class BiTuple<A, B> : AbstractTuple
    {
        public A factA;
        public B factB;

        public BiTuple(A factA, B factB, int storeSize)
            : base(storeSize)
        {

            this.factA = factA;
            this.factB = factB;
        }

        public override string ToString()
        {
            return "{" + factA + ", " + factB + "}";
        }
    }
}
