using System.Reflection;
using System.Runtime.CompilerServices;

namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor
{
    public class ReflectionFieldMemberAccessor : AbstractMemberAccessor
    {
        private readonly PropertyInfo property;

        public override string GetName()
        {
            return property.Name;
        }

        public override Type GetGenericType()
        {
            return property.PropertyType;
        }

        public ReflectionFieldMemberAccessor(PropertyInfo property)
        {
            this.property = property;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override object ExecuteGetter(object bean)
        {
            return property.GetValue(bean);
        }

        public override T GetAnnotation<T>(Type annotationClass)
        {
            var att = Attribute.GetCustomAttribute(property, annotationClass);
            if (att != null)
                return att as T;
            return null;
        }

        public override Type GetClass()
        {
            return property.PropertyType;
        }

        public override Type GetDeclaringClass()
        {
            return property.ReflectedType;
        }

        public override void ExecuteSetter(object bean, object value)
        {
            property.SetValue(bean, value, null);
        }
    }
}
