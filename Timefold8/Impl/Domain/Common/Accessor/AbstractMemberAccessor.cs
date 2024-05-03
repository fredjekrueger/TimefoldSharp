namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor
{
    public abstract class AbstractMemberAccessor : MemberAccessor
    {
        public AbstractMemberAccessor()
        {
            getterFunction = (f) => ExecuteGetter(f);
        }

        private readonly Func<object, object> getterFunction;

        public abstract object ExecuteGetter(object o);

        public Func<Fact_, Result_> GetGetterFunction<Fact_, Result_>()
        {
            return (f) => (Result_)getterFunction(f);
        }

        public abstract Type GetGenericType();

        public abstract string GetName();

        public abstract T GetAnnotation<T>(Type annotationClass) where T : class;

        public abstract Type GetClass();

        public abstract Type GetDeclaringClass();

        public abstract void ExecuteSetter(object bean, object value);
    }
}
