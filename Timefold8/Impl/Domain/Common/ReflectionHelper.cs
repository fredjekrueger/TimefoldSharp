using System.Reflection;

namespace TimefoldSharp.Core.Impl.Domain.Common
{
    public sealed class ReflectionHelper
    {

        private static string PROPERTY_ACCESSOR_PREFIX_GET = "get";
        private static string PROPERTY_ACCESSOR_PREFIX_IS = "is";

        private static String[] PROPERTY_ACCESSOR_PREFIXES = {
            PROPERTY_ACCESSOR_PREFIX_GET,
            PROPERTY_ACCESSOR_PREFIX_IS
    };


        internal static bool IsGetterMethod(MethodInfo method)
        {
            if (method.GetParameters().Length != 0)
            {
                return false;
            }
            string methodName = method.Name;
            if (methodName.StartsWith(PROPERTY_ACCESSOR_PREFIX_GET) && method.ReturnType != null)
            {
                return true;
            }
            else if (methodName.StartsWith(PROPERTY_ACCESSOR_PREFIX_IS) && method.ReturnType == typeof(bool))
            {
                return true;
            }
            return false;

        }

        public static bool IsMethodOverwritten(MethodInfo parentMethod, Type childClass)
        {
            //lichtjes aangepast
            return childClass.GetMethods().Any(m => m.Name == parentMethod.Name);
        }

        public static List<Object> TransformArrayToList(Object arrayObject)
        {
            throw new NotImplementedException();
        }


        public static MethodInfo GetGetterMethod(Type containingClass, String propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
