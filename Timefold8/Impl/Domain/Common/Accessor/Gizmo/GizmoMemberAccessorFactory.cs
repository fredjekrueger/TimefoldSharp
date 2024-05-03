using System.Reflection;

namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor.Gizmo
{
    public class GizmoMemberAccessorFactory
    {
        public static string GetGeneratedClassName(MemberInfo member, Type type)
        {
            string memberName = /*ReflectionHelper.GetGetterPropertyName(member) ??*/ member.Name;
            string memberType = (member is PropertyInfo) ? "Property" : "Method";

            return type.Name + "$Timefold$MemberAccessor$" + memberType + "$" + memberName;
        }

        internal static MemberAccessor BuildGizmoMemberAccessor(MemberInfo member, Type annotationClass, GizmoClassLoader classLoader)
        {
            throw new NotImplementedException();
        }
    }
}
