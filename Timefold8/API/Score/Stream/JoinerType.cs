namespace TimefoldSharp.Core.API.Score.Stream
{
    public enum JoinerType
    {
        EQUAL, LESS_THAN, LESS_THAN_OR_EQUAL, GREATER_THAN, GREATER_THAN_OR_EQUAL, CONTAINING, INTERSECTING, DISJOINT
    }

    public static class JoinerTypeEnumHelper
    {
        public static JoinerType Flip(JoinerType type)
        {
            switch (type)
            {
                case JoinerType.LESS_THAN:
                    return JoinerType.GREATER_THAN;
                case JoinerType.LESS_THAN_OR_EQUAL:
                    return JoinerType.GREATER_THAN_OR_EQUAL;
                case JoinerType.GREATER_THAN:
                    return JoinerType.LESS_THAN;
                case JoinerType.GREATER_THAN_OR_EQUAL:
                    return JoinerType.LESS_THAN_OR_EQUAL;
                default:
                    throw new Exception("The joinerType (" + type + ") cannot be flipped.");
            }
        }
    }
}
