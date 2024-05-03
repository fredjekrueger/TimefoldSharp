using System.Reflection;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Cloner
{
    public sealed class FieldCloningUtils
    {
        public static void CopyField<T>(PropertyInfo field, object original, object clone)
        {
            T originalValue = GetFieldValue<T>(original, field);
            SetFieldValue(clone, field, originalValue);
        }

        private static void SetFieldValue<T>(object bean, PropertyInfo field, T value)
        {
            field.SetValue(bean, value);
        }

        public static T GetFieldValue<T>(object bean, PropertyInfo field)
        {
            return (T)field.GetValue(bean);
        }

        public static void SetObjectFieldValue(object bean, PropertyInfo field, object value)
        {
            field.SetValue(bean, value);
        }
    }
}
