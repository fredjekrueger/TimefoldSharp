namespace TimefoldSharp.Core.API.Score.Stream.Bi
{
    public interface BiJoiner<A, B>
    {
        BiJoiner<A, B> And(BiJoiner<A, B> otherJoiner);
    }
}
