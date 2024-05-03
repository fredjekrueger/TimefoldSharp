using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Policy;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Descriptor
{
    public class ListVariableDescriptor : GenuineVariableDescriptor
    {
        public ListVariableDescriptor(EntityDescriptor entityDescriptor, MemberAccessor variableMemberAccessor) : base(entityDescriptor, variableMemberAccessor)
        {

        }

        public override bool AcceptsValueType(Type valueType)
        {
            throw new NotImplementedException();
        }

        public override bool IsChained()
        {
            return false;
        }

        public override bool IsInitialized(object entity)
        {
            return true;
        }

        public override bool IsListVariable()
        {
            return true;
        }

        public override bool IsNullable()
        {
            throw new NotImplementedException();
        }

        protected override void ProcessPropertyAnnotations(DescriptorPolicy descriptorPolicy)
        {
            throw new NotImplementedException();
        }

        public int GetListSize(object entity)
        {
            return GetListVariable(entity).Count();
        }

        public List<object> GetListVariable(object entity)
        {
            object value = GetValue(entity);
            if (value == null)
            {
                throw new Exception("The planning list variable (" + this + ") of entity (" + entity + ") is null.");
            }
            return (List<Object>)value;
        }

        public override bool IsGenuineAndUninitialized(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
