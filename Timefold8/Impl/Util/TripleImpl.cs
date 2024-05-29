namespace TimefoldSharp.Core.Impl.Util
{
    public class TripleImpl<A, B, C> : Triple<A, B, C>
    {

        private A a;
        private B b;
        private C c;

        public TripleImpl(A a, B b, C c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public A GetA()
        {
            return a;
        }

        public B GetB()
        {
            return b;
        }

        public C GetC()
        {
            return c;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o == null || GetType() != o.GetType())
                return false;
            TripleImpl<A, B, C> that = (TripleImpl<A, B, C>)o;
            return a.Equals(that.a) && b.Equals(that.b) && c.Equals(that.c);
        }

        public override int GetHashCode()
        {
            // Not using Objects.hash(Object...) as that would create an array on the hot path.
            int result = a.GetHashCode();
            result = 31 * result + b.GetHashCode();
            result = 31 * result + c.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "(" + a + ", " + b + ", " + c + ")";
        }
    }
}
