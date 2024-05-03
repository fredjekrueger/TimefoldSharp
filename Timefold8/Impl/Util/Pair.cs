namespace TimefoldSharp.Core.Impl.Util
{
    public interface Pair<A, B>
    {
        A GetKey();

        B GetValue();
    }

    public static class PairHelper<A, B>
    {
        public static Pair<A, B> Of(A key, B value)
        {
            return new PairImpl<A, B>(key, value);
        }
    }
}
