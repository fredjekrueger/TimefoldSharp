namespace TimefoldSharp.Core.API.Score.Stream.Bi
{
    public interface BiJoiner<A, B, Property_>
    {
        BiJoiner<A, B, Property_> And(BiJoiner<A, B, Property_> otherJoiner);
    }
}
