using System.Collections.Concurrent;
using System.Reflection;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor.Gizmo;

namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor
{
    public sealed class MemberAccessorFactory
    {

        public MemberAccessorFactory(Dictionary<string, MemberAccessor> memberAccessorMap)
        {
            // The MemberAccessorFactory may be accessed, and this cache both read and updated, by multiple threads.
            this.memberAccessorCache =
                    memberAccessorMap == null ? new ConcurrentDictionary<string, MemberAccessor>() : new ConcurrentDictionary<string, MemberAccessor>(memberAccessorMap);
        }

        private readonly ConcurrentDictionary<string, MemberAccessor> memberAccessorCache = new ConcurrentDictionary<string, MemberAccessor>();

        private readonly GizmoClassLoader gizmoClassLoader = new GizmoClassLoader();

        public GizmoClassLoader GetGizmoClassLoader()
        {
            return gizmoClassLoader;
        }

        public MemberAccessor BuildAndCacheMemberAccessor(Type type, MemberInfo member, MemberAccessorType memberAccessorType, Type annotationClass)
        {
            string generatedClassName = GizmoMemberAccessorFactory.GetGeneratedClassName(member, type);
            return memberAccessorCache.GetOrAdd(generatedClassName,
                    k => MemberAccessorFactory.BuildMemberAccessor(member, memberAccessorType, annotationClass, gizmoClassLoader));
        }

        public static MemberAccessor BuildMemberAccessor(MemberInfo member, MemberAccessorType memberAccessorType,  Type annotationClass, ClassLoaderJDEF classLoader)
        {
            return BuildReflectiveMemberAccessor(member, memberAccessorType, annotationClass);
        }

        private static MemberAccessor BuildReflectiveMemberAccessor(MemberInfo member, MemberAccessorType memberAccessorType, Type annotationClass)
        {
            if (member is PropertyInfo property)
            {
                return new ReflectionFieldMemberAccessor(property);
            }
            else if (member is MethodInfo method)
            {
                MemberAccessor memberAccessor;

                if (memberAccessorType == MemberAccessorType.PROPERTY_OR_READ_METHOD)
                {
                    if (!ReflectionHelper.IsGetterMethod(method))
                    {
                        memberAccessor = new ReflectionMethodMemberAccessor(method);
                        return memberAccessor;
                    }
                }
                if (memberAccessorType == MemberAccessorType.PROPERTY_OR_GETTER_METHOD || memberAccessorType == MemberAccessorType.PROPERTY_OR_GETTER_METHOD_WITH_SETTER)
                {
                    bool getterOnly = memberAccessorType != MemberAccessorType.PROPERTY_OR_GETTER_METHOD_WITH_SETTER;
                    return memberAccessor = new ReflectionBeanPropertyMemberAccessor(method, getterOnly);
                }
                else
                {
                    throw new Exception("The memberAccessorType (" + memberAccessorType
                               + ") is not implemented.");
                }
            }
            else
            {
                throw new Exception("Impossible state: .");
            }
        }

        public enum MemberAccessorType
        {
            PROPERTY_OR_READ_METHOD,
            PROPERTY_OR_GETTER_METHOD,
            PROPERTY_OR_GETTER_METHOD_WITH_SETTER
        }
    }
}
