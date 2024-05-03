namespace TimefoldSharp.Core.Impl.Util
{
    public interface MutablePair<A, B> : Pair<A, B>
    {
        MutablePair<A, B> SetKey(A key);

        MutablePair<A, B> SetValue(B value);
    }

    public static class MutablePairHelper
    {
        public static MutablePair<A, B> Of<A, B>(A key, B value)
        {
            return new MutablePairImpl<A, B>(key, value);
        }
    }
}
