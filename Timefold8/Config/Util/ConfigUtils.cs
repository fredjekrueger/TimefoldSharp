using System.Reflection;
using TimefoldSharp.Core.API.Domain.Lookup;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;

namespace TimefoldSharp.Core.Config.Util
{
    public class ConfigUtils
    {
        public static T NewInstance<T>(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public static T InheritOverwritableProperty<T>(T original, T inherited)
        {
            if (original != null)
            {
                // Original overwrites inherited
                return original;
            }
            else
            {
                return inherited;
            }
        }

        public static Config_ InheritConfig<Config_>(Config_ original, Config_ inherited) where Config_ : AbstractConfig<Config_>
        {
            if (inherited != null)
            {
                if (original == null)
                {
                    original = inherited.CopyConfig();
                }
                else
                {
                    original.Inherit(inherited);
                }
            }
            return original;
        }

        public static Dictionary<K, T> InheritMergeableMapProperty<K, T>(Dictionary<K, T> originalMap, Dictionary<K, T> inheritedMap)
        {
            if (inheritedMap == null)
            {
                return originalMap;
            }
            else if (originalMap == null)
            {
                return inheritedMap;
            }
            else
            {
                // The inheritedMap should be before the originalMap
                Dictionary<K, T> mergedMap = new Dictionary<K, T>(inheritedMap);
                foreach (var item in originalMap)
                {
                    mergedMap[item.Key] = item.Value;
                }
                return mergedMap;
            }
        }

        public static List<T> InheritMergeableListProperty<T>(List<T> originalList, List<T> inheritedList)
        {
            if (inheritedList == null)
            {
                return originalList;
            }
            else if (originalList == null)
            {
                // Shallow clone due to modifications after calling inherit
                return new List<T>(inheritedList);
            }
            else
            {
                // The inheritedList should be before the originalList
                List<T> mergedList = new List<T>(inheritedList);
                mergedList.AddRange(originalList);
                return mergedList;
            }
        }

        public static List<Config> InheritMergeableListConfig<Config>(List<Config> originalList, List<Config> inheritedList) where Config : AbstractConfig<Config>
        {
            if (inheritedList != null)
            {
                List<Config> mergedList = new List<Config>(inheritedList.Count + (originalList == null ? 0 : originalList.Count));
                // The inheritedList should be before the originalList
                foreach (Config inherited in inheritedList)
                {
                    Config copy = inherited.CopyConfig();
                    mergedList.Add(copy);
                }
                if (originalList != null)
                {
                    mergedList.AddRange(originalList);
                }
                originalList = mergedList;
            }
            return originalList;
        }

        internal static int ResolvePoolSize(string propertyName, string value, params string[] magicValues)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception ex)
            {
                throw new Exception("The " + propertyName + " (" + value + ") resolved to neither of ("
                        + ") nor a number.");
            }
        }

        internal static void ApplyCustomProperties(object bean, string beanClassPropertyName, Dictionary<string, string> constraintProviderCustomProperties, string customPropertiesPropertyName)
        {

        }

        public static List<MemberInfo> GetAllMembers(Type baseClass, Type annotationClass)
        {
            var membersWithAttribute = baseClass.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(member => member.IsDefined(annotationClass, false));
            return membersWithAttribute.ToList();
        }

        private static MemberInfo GetSingleMember(Type clazz, Type annotationClass)
        {
            List<MemberInfo> memberList = GetAllMembers(clazz, annotationClass);
            if (memberList.Count == 0)
            {
                return null;
            }
            if (memberList.Count > 1)
            {
                throw new Exception("The class (" + clazz
                        + ") has " + memberList.Count + " members (" + memberList + ") with a "
                        + annotationClass.Name + " annotation.");
            }
            return memberList.First();
        }

        public static MemberAccessor FindPlanningIdMemberAccessor(Type clazz, MemberAccessorFactory memberAccessorFactory)
        {
            MemberInfo member = GetSingleMember(clazz, typeof(PlanningIdAttribute));
            if (member == null)
            {
                return null;
            }
            MemberAccessor memberAccessor =
                    memberAccessorFactory.BuildAndCacheMemberAccessor(clazz, member, MemberAccessorFactory.MemberAccessorType.PROPERTY_OR_READ_METHOD, typeof(PlanningIdAttribute));
            return memberAccessor;
        }

        public static Type ExtractCollectionGenericTypeParameterLeniently(string parentClassConcept, Type parentClass, Type type, Type genericType, Type annotationClass, string memberName)
        {
            return ExtractCollectionGenericTypeParameter(parentClassConcept, parentClass, type, genericType, annotationClass, memberName);
        }

        private static Type ExtractCollectionGenericTypeParameter(string parentClassConcept, Type parentClass, Type type, Type genericType, Type annotationClass, string memberName)
        {
            if (!genericType.IsGenericType)
            {
                return null;
            }
            Type[] typeArguments = genericType.GenericTypeArguments;
            if (typeArguments.Count() != 1)
            {
                throw new Exception("The ");
            }
            Type typeArgument = typeArguments[0];

            /*if (typeArgument is WildcardType wildcardType)
            {
                Type[] upperBounds = wildcardType.getUpperBounds();
                if (upperBounds.Count() > 1)
                {
                    // Multiple upper bounds is impossible in traditional Java
                    // Other JVM languages or future java versions might enabling triggering this
                    throw new Exception("The ).");
                }
                if (upperBounds.Count() == 0)
                {
                    typeArgument = typeof(object);
                }
                else
                {
                    typeArgument = upperBounds[0];
                }
            }*/
            if (typeArgument is Type class1)
            {
                return class1;
            }
            else if (typeArgument.IsGenericType)
            {
                // Turns SomeGenericType<T> into SomeGenericType.
                return typeArgument;
            }
            else
            {
                throw new Exception("The ");
            }
        }

        public static List<MemberInfo> GetDeclaredMembers(Type baseClass)
        {
            List<PropertyInfo> propertyStream = baseClass.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(c => c.Name).ToList();
            List<MemberInfo> returnList = new List<MemberInfo>();
            returnList.AddRange(propertyStream);
            return returnList;
        }

        public static List<Type> GetAllAnnotatedLineageClasses(Type bottomClass, Type annotation)
        {
            if (!IsAnnotationPresent(bottomClass, annotation))
            {
                return new List<Type>();
            }
            List<Type> lineageClassList = new List<Type>();
            lineageClassList.Add(bottomClass);
            Type superclass = bottomClass.BaseType;
            lineageClassList.AddRange(GetAllAnnotatedLineageClasses(superclass, annotation));
            foreach (var superInterface in bottomClass.GetInterfaces())
            {
                lineageClassList.AddRange(GetAllAnnotatedLineageClasses(superInterface, annotation));
            }
            return lineageClassList;
        }

        static bool IsAnnotationPresent(Type classtype, Type annotationType)
        {
            return Attribute.GetCustomAttribute(classtype, annotationType) != null;
        }

        public static Type ExtractAnnotationClass(MemberInfo member, params Type[] annotationClasses)
        {
            Type annotationClass = null;
            foreach (var detectedAnnotationClass in annotationClasses)
            {
                if (Attribute.IsDefined(member, detectedAnnotationClass, false))
                {
                    if (annotationClass != null)
                    {
                        throw new Exception("The clastation.");
                    }
                    annotationClass = detectedAnnotationClass;
                    // Do not break early: check other annotationClasses too
                }
            }
            return annotationClass;
        }

        public static Type ExtractCollectionGenericTypeParameterStrictly(string parentClassConcept, Type parentClass, Type type, Type genericType, Type annotationClass, string memberName)
        {
            return ExtractCollectionGenericTypeParameter(parentClassConcept, parentClass, type, genericType, annotationClass, memberName);
        }

        internal static bool IsEmptyCollection(List<AbstractMoveSelectorConfig> list)
        {
            return list == null || list.Count == 0;
        }
    }
}
