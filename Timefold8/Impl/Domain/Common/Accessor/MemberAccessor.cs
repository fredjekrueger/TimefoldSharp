namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor
{
    public interface MemberAccessor
    {
        Func<Fact_, object> GetGetterFunction<Fact_>();

        object ExecuteGetter(Object bean);

        Type GetGenericType();
        String GetName();
        T GetAnnotation<T>(Type annotationClass) where T : class;
        Type GetClass();
        Type GetDeclaringClass();
        void ExecuteSetter(object bean, object value);
    }
}
