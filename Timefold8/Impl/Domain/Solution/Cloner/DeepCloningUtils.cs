using System.Reflection;
using TimefoldSharp.Core.API.Domain.Solution.Cloner;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Cloner
{
    public sealed class DeepCloningUtils
    {

        private static readonly HashSet<Type> IMMUTABLE_CLASSES = new HashSet<Type>() {
            // Numbers
          typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double),
            // Optional
            // Date and time
            typeof(DateTime), typeof(TimeSpan), typeof(DateTimeOffset),
            // Others
            typeof(bool), typeof(char), typeof(string), typeof(Guid)};


        public static bool IsClassDeepCloned(SolutionDescriptor solutionDescriptor, Type type)
        {
            if (IsImmutable(type))
            {
                return false;
            }
            return solutionDescriptor.HasEntityDescriptor(type)
                    || solutionDescriptor.SolutionClass.IsAssignableFrom(type)
                    || Attribute.IsDefined(type, typeof(DeepPlanningCloneAttribute));
        }

        public static bool IsImmutable(Type clz)
        {
            if (clz.IsPrimitive || clz.IsEnum || typeof(API.Score.Score).IsAssignableFrom(clz))
            {
                return true;
            }
            return IMMUTABLE_CLASSES.Contains(clz);
        }

        public static bool IsFieldDeepCloned(SolutionDescriptor solutionDescriptor, PropertyInfo field, Type owningClass)
        {
            var fieldType = field.GetType();
            if (IsImmutable(fieldType))
            {
                return false;
            }
            return IsFieldAnEntityPropertyOnSolution(solutionDescriptor, field, owningClass)
                    || IsFieldAnEntityOrSolution(solutionDescriptor, field)
                    || IsFieldAPlanningListVariable(field, owningClass)
                    || IsFieldADeepCloneProperty(field, owningClass);
        }

        private static bool IsFieldADeepCloneProperty(PropertyInfo field, Type owningClass)
        {
            return Attribute.IsDefined(field, typeof(DeepPlanningCloneAttribute));

            //MethodInfo getterMethod = ReflectionHelper.GetGetterMethod(owningClass, field.Name);
            //return getterMethod != null && getterMethod.GetCustomAttribute<DeepPlanningCloneAttribute>() != null;
        }

        static bool IsFieldAnEntityPropertyOnSolution(SolutionDescriptor solutionDescriptor, PropertyInfo field, Type owningClass)
        {
            if (!solutionDescriptor.SolutionClass.IsAssignableFrom(owningClass))
            {
                return false;
            }

            // field.getDeclaringClass() is a superclass of or equal to the owningClass
            String fieldName = field.Name;
            // This assumes we're dealing with a simple getter/setter.
            // If that assumption is false, validateCloneSolution(...) fails-fast.
            if (solutionDescriptor.GetEntityMemberAccessorMap().ContainsKey(fieldName))
            {
                return true;
            }
            // This assumes we're dealing with a simple getter/setter.
            // If that assumption is false, validateCloneSolution(...) fails-fast.
            return solutionDescriptor.GetEntityCollectionMemberAccessorMap().ContainsKey(fieldName);
        }

        private static bool IsFieldAnEntityOrSolution(SolutionDescriptor solutionDescriptor, PropertyInfo field)
        {
            Type type = field.GetType();
            if (IsClassDeepCloned(solutionDescriptor, type))
            {
                return true;
            }
            if (typeof(List<>).IsAssignableFrom(type) || typeof(Dictionary<,>).IsAssignableFrom(type))
            {
                throw new NotImplementedException();
                //return IsTypeArgumentDeepCloned(solutionDescriptor, field.GetGenericType());
            }
            else if (type.IsArray)
            {
                throw new NotImplementedException();
                //return IsClassDeepCloned(solutionDescriptor, type.GetComponentType());
            }
            return false;
        }

        private static bool IsTypeArgumentDeepCloned(SolutionDescriptor solutionDescriptor, Type type)
        {
            // Check the generic type arguments of the field.
            // It is possible for fields and methods, but not instances.
            if (!type.IsGenericType)
            {
                return false;
            }
            Type[] genericArguments = type.GetGenericArguments();

            foreach (var actualTypeArgument in genericArguments)
            {
                if (actualTypeArgument is Type class1 && IsClassDeepCloned(solutionDescriptor, class1))
                {
                    return true;
                }
                if (IsTypeArgumentDeepCloned(solutionDescriptor, actualTypeArgument))
                {
                    return true;
                }

            }
            return false;
        }

        private static bool IsFieldAPlanningListVariable(PropertyInfo field, Type owningClass)
        {
            /*if (!Attribute.IsDefined(field, typeof(PlanningListVariableAttribute)))
            {
                MethodInfo getterMethod = ReflectionHelper.GetGetterMethod(owningClass, field.Name);
                return getterMethod != null && getterMethod.GetCustomAttribute<PlanningListVariableAttribute>() != null;
            }
            else
            {
                return true;
            }*/
            return Attribute.IsDefined(field, typeof(PlanningListVariableAttribute));
        }
    }
}
