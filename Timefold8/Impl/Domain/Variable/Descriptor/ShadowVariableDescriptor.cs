using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Domain.Variable.Listener;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Descriptor
{
    public abstract class ShadowVariableDescriptor : VariableDescriptor
    {
        public int GlobalShadowOrder = int.MaxValue;

        public abstract Demand GetProvidedDemand<S>() where S : Supply.Supply;

        public bool HasVariableListener()
        {
            return true;
        }

        public override bool IsGenuineAndUninitialized(object entity)
        {
            return false;
        }

        public ShadowVariableDescriptor(EntityDescriptor entityDescriptor, MemberAccessor variableMemberAccessor) : base(entityDescriptor, variableMemberAccessor)
        {
        }

        public abstract void ProcessAnnotations(DescriptorPolicy descriptorPolicy);
        public abstract IEnumerable<VariableListenerWithSources> BuildVariableListeners(SupplyManager supplyManager);
        public abstract List<VariableDescriptor> GetSourceVariableDescriptorList();
    }
}
