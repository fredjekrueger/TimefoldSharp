using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Policy;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Descriptor
{
    public abstract class VariableDescriptor
    {
        protected readonly string variableName;

        public VariableDescriptor(EntityDescriptor entityDescriptor, MemberAccessor variableMemberAccessor)
        {
            this.EntityDescriptor = entityDescriptor;
            this.variableMemberAccessor = variableMemberAccessor;
            variableName = variableMemberAccessor.GetName();
            if (variableMemberAccessor.GetClass().IsPrimitive)
            {
                throw new Exception("The entityClass (its primitive wrapper type instead.");
            }
        }

        public abstract bool IsGenuineAndUninitialized(object entity);

        public void SetValue(object entity, object value)
        {
            variableMemberAccessor.ExecuteSetter(entity, value);
        }

        public String GetVariableName()
        {
            return variableName;
        }

        public Type GetVariablePropertyType()
        {
            return variableMemberAccessor.GetClass();
        }

        public bool IsGenuineListVariable()
        {
            return false;
        }

        public List<ShadowVariableDescriptor> SinkVariableDescriptorList = new List<ShadowVariableDescriptor>(4);

        public EntityDescriptor EntityDescriptor { get; set; }

        protected readonly MemberAccessor variableMemberAccessor;

        public object GetValue(object entity)
        {
            return variableMemberAccessor.ExecuteGetter(entity);
        }

        public abstract void LinkVariableDescriptors(DescriptorPolicy descriptorPolicy);
    }
}
