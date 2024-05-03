namespace TimefoldSharp.Core.Impl.Util
{
    public sealed class MutablePairImpl<A, B> : MutablePair<A, B>
    {
        private A key;
        private B value;

        public MutablePairImpl(A key, B value)
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

        public MutablePair<A, B> SetKey(A key)
        {
            this.key = key;
            return this;
        }

        public MutablePair<A, B> SetValue(B value)
        {
            this.value = value;
            return this;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}