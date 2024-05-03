namespace TimefoldSharp.Core.Impl.Solver
{
    public sealed class ClassInstanceCache
    {
        public static ClassInstanceCache Create()
        {
            return new ClassInstanceCache();
        }

        internal T NewInstance<T>(object config, string propertyName, Type clazz)
        {
            throw new NotImplementedException();
        }
    }
}
