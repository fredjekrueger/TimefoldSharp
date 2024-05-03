namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor
{
    public interface MemberAccessor
    {
        Func<Fact_, Result_> GetGetterFunction<Fact_, Result_>();

        object ExecuteGetter(Object bean);

        Type GetGenericType();
        String GetName();
        T GetAnnotation<T>(Type annotationClass) where T : class;
        Type GetClass();
        Type GetDeclaringClass();
        void ExecuteSetter(object bean, object value);
    }
}
