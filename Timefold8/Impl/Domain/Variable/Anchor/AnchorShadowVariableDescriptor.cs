using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;
using TimefoldSharp.Core.Impl.Domain.Variable.Listener;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Anchor
{
    public class AnchorShadowVariableDescriptor : ShadowVariableDescriptor
    {

        protected VariableDescriptor sourceVariableDescriptor;

        public AnchorShadowVariableDescriptor(EntityDescriptor entityDescriptor, MemberAccessor variableMemberAccessor) : base(entityDescriptor, variableMemberAccessor)
        {
        }

        public override IEnumerable<VariableListenerWithSources> BuildVariableListeners(SupplyManager supplyManager)
        {
            SingletonInverseVariableSupply inverseVariableSupply = supplyManager.Demand(new SingletonInverseVariableDemand(sourceVariableDescriptor)) as SingletonInverseVariableSupply;
            return new VariableListenerWithSources(new AnchorVariableListener(this, sourceVariableDescriptor, inverseVariableSupply), sourceVariableDescriptor).ToCollection();
        }

        public override Demand GetProvidedDemand<S>()
        {
            return new AnchorVariableDemand(sourceVariableDescriptor);
        }

        public override List<VariableDescriptor> GetSourceVariableDescriptorList()
        {
            return new List<VariableDescriptor> { sourceVariableDescriptor };
        }
       
        public override void LinkVariableDescriptors(DescriptorPolicy descriptorPolicy)
        {
            LinkShadowSources(descriptorPolicy);
        }

        private void LinkShadowSources(DescriptorPolicy descriptorPolicy)
        {
            AnchorShadowVariableAttribute shadowVariableAnnotation = variableMemberAccessor.GetAnnotation<AnchorShadowVariableAttribute>(typeof(AnchorShadowVariableAttribute));
            string sourceVariableName = shadowVariableAnnotation.SourceVariableName;
            sourceVariableDescriptor = EntityDescriptor.GetVariableDescriptor(sourceVariableName);
            if (sourceVariableDescriptor == null)
            {
                throw new Exception("The entityClass (" + EntityDescriptor.EntityClass
                        + ") has an attribute" + typeof(AnchorShadowVariableAttribute).Name
                        + " annotated property (" + variableMemberAccessor.GetName()
                        + ") with sourceVariableName (" + sourceVariableName
                        + ") which is not a valid planning variable on entityClass ("
                        + EntityDescriptor.EntityClass + ").\n");
            }
            if (!(sourceVariableDescriptor is GenuineVariableDescriptor) || !((GenuineVariableDescriptor)sourceVariableDescriptor).IsChained())
            {
                throw new Exception("The entityClass (" + EntityDescriptor.EntityClass
                        + ") has an attribute" + typeof(AnchorShadowVariableAttribute).Name
                        + " annotated property (" + variableMemberAccessor.GetName()
                        + ") with sourceVariableName (" + sourceVariableName
                        + ") which is not chained.");
            }
            sourceVariableDescriptor.RegisterSinkVariableDescriptor(this);
        }

        public override void ProcessAnnotations(DescriptorPolicy descriptorPolicy)
        {
        }
    }
}
