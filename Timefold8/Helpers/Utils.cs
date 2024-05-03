namespace TimefoldSharp.Core.Helpers
{
    public static class Utils
    {
        public static int CombineHashCodes(params object[] objects)
        {
            unchecked
            {
                int hash = 17;
                foreach (var obj in objects)
                {
                    hash = hash * 23 + (obj != null ? obj.GetHashCode() : 0);
                }
                return hash;
            }
        }

        public static bool IsGenericTypeOfType(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
