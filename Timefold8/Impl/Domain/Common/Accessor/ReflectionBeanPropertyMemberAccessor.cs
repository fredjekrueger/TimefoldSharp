using System.Reflection;

namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor
{
    public sealed class ReflectionBeanPropertyMemberAccessor : AbstractMemberAccessor
    {
        public ReflectionBeanPropertyMemberAccessor(MethodInfo getterMethod, bool getterOnly)
        {
            throw new NotImplementedException();
        }

        private readonly Type propertyType;
        private readonly string propertyName;
        private readonly MethodInfo getterMethod;

        public override string GetName()
        {
            return propertyName;
        }

        public override Type GetGenericType()
        {
            return getterMethod.ReturnType;
        }


        public override object ExecuteGetter(object o)
        {
            throw new NotImplementedException();
        }

        public override T GetAnnotation<T>(Type annotationClass)
        {
            throw new NotImplementedException();
        }

        public override Type GetClass()
        {
            throw new NotImplementedException();
        }

        public override Type GetDeclaringClass()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteSetter(object bean, object value)
        {
            throw new NotImplementedException();
        }
    }
}
