using TimefoldSharp.Core.Impl.Domain.Common.Accessor;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Descriptor
{
    public sealed class DummyMemberAccessor : MemberAccessor
    {

        public static readonly MemberAccessor INSTANCE = new DummyMemberAccessor();

        public object ExecuteGetter(object bean)
        {
            return null;
        }

        public void ExecuteSetter(object bean, object value)
        {

        }

        public T GetAnnotation<T>(Type annotationClass) where T : class
        {
            return null;
        }

        public Type GetClass()
        {
            return null;
        }

        public Type GetDeclaringClass()
        {
            return null;
        }

        public Type GetGenericType()
        {
            return null;
        }

        public Func<Fact_, Result_> GetGetterFunction<Fact_, Result_>()
        {
            return null;
        }

        public string GetName()
        {
            return null;
        }
    }
}
