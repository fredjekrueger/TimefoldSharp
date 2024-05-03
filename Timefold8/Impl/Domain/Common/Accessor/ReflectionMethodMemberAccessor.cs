using System.Reflection;

namespace TimefoldSharp.Core.Impl.Domain.Common.Accessor
{
    public sealed class ReflectionMethodMemberAccessor : AbstractMemberAccessor
    {

        private readonly string methodName;
        private readonly MethodInfo readMethod;
        private readonly Type returnType;

        public ReflectionMethodMemberAccessor(MethodInfo readMethod)
        {
            this.readMethod = readMethod;
            this.returnType = readMethod.ReturnType;
            this.methodName = readMethod.Name;
            try
            {
                // this.methodHandle = MethodHandles.lookup()
                //.unreflect(readMethod)
                //.asFixedArity();
            }
            catch (Exception e)
            {
                throw new Exception("Impossible state: Method (%s) not accessible.", e);
            }
            if (readMethod.GetParameters().Length != 0)
            {
                throw new Exception("The readMethod (" + readMethod + ") must not have any parameters ().");
            }
            if (readMethod.ReturnType == null)
            {
                throw new Exception("The readMethod (" + readMethod + ") must have a return type ");
            }
        }

        public override string GetName()
        {
            return methodName;
        }

        public override Type GetGenericType()
        {
            return returnType;
        }

        public override object ExecuteGetter(object o)
        {
            throw new NotImplementedException();
        }

        public override T GetAnnotation<T>(Type annotationClass)
        {
            var att = Attribute.GetCustomAttribute(readMethod, annotationClass);
            if (att != null)
                return att as T;
            return null;
        }

        public override Type GetClass()
        {
            return returnType;
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
