namespace TimefoldSharp.Core.API.Function
{
    public abstract class TriConsumer<A, B, C>
    {
        public abstract void Accept(A a, B b, C c);

        public TriConsumer<A, B, C> AndThen(TriConsumer<A, B, C> after)
        {
            throw new NotImplementedException();
        }
    }
}
