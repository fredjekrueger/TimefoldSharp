namespace TimefoldSharp.Core.Impl.Util
{
    public sealed class PairImpl<A, B> : Pair<A, B>
    {

        private readonly A key;
        private readonly B value;

        public PairImpl(A key, B value)
        {
            this.key = key;
            this.value = value;
        }

        public A GetKey()
        {
            return key;
        }

        public B GetValue()
        {
            return value;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o == null)
                return false;
            PairImpl<A, B> that = (PairImpl<A, B>)o;
            return key.Equals(that.key) && value.Equals(that.value);
        }

        public override int GetHashCode()
        {
            int result = key.GetHashCode();
            result = 31 * result + value.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "(" + key + ", " + value + ")";
        }
    }
}
