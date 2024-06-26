using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Listener;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;

namespace TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation
{
    public class InverseRelationShadowVariableDescriptor : ShadowVariableDescriptor
    {
        protected bool chained;
        protected bool singleton;
        protected VariableDescriptor sourceVariableDescriptor;

        public InverseRelationShadowVariableDescriptor(EntityDescriptor entityDescriptor, MemberAccessor variableMemberAccessor) : base(entityDescriptor, variableMemberAccessor)
        {

        }

        public override void ProcessAnnotations(DescriptorPolicy descriptorPolicy)
        {
        }

        public override IEnumerable<VariableListenerWithSources> BuildVariableListeners(SupplyManager supplyManager)
        {
            throw new NotImplementedException();
        }

        public override Demand GetProvidedDemand<S>()
        {
            throw new NotImplementedException();
        }

        public override List<VariableDescriptor> GetSourceVariableDescriptorList()
        {
            throw new NotImplementedException();
        }

        public override bool IsGenuineAndUninitialized(object entity)
        {
            throw new NotImplementedException();
        }

        public override void LinkVariableDescriptors(DescriptorPolicy descriptorPolicy)
        {
            LinkShadowSources(descriptorPolicy);
        }

        private void LinkShadowSources(DescriptorPolicy descriptorPolicy)
        {
            InverseRelationShadowVariableAttribute shadowVariableAnnotation = variableMemberAccessor.GetAnnotation<InverseRelationShadowVariableAttribute>(typeof(InverseRelationShadowVariableAttribute));
            var variablePropertyType = GetVariablePropertyType();
            Type sourceClass;
            if (typeof(List<>).IsAssignableFrom(variablePropertyType))
            {
                Type genericType = variableMemberAccessor.GetGenericType();
                sourceClass = ConfigUtils.ExtractCollectionGenericTypeParameterLeniently("entityClass", EntityDescriptor.EntityClass, variablePropertyType, genericType, typeof(InverseRelationShadowVariableAttribute), variableMemberAccessor.GetName());
                if (sourceClass == null)
                    sourceClass = typeof(object);
                singleton = false;
            }
            else
            {
                sourceClass = variablePropertyType;
                singleton = true;
            }
            EntityDescriptor sourceEntityDescriptor = EntityDescriptor.GetSolutionDescriptor().FindEntityDescriptor(sourceClass);
            if (sourceEntityDescriptor == null)
            {
                throw new Exception("The entityClass (" + EntityDescriptor.EntityClass
                        + ") has an attribute" + typeof(InverseRelationShadowVariableAttribute).Name
                        + " annotated property (" + variableMemberAccessor.GetName()
                        + ") with a sourceClass (" + sourceClass
                        + ") which is not a valid planning entity."
                        + "\nMaybe check the annotations of the class (" + sourceClass + ")."
                        + "\nMaybe add the class (" + sourceClass
                        + ") among planning entities in the solver configuration.");
            }
            string sourceVariableName = shadowVariableAnnotation.SourceVariableName;

            sourceVariableDescriptor = sourceEntityDescriptor.GetVariableDescriptor(sourceVariableName);
            if (sourceVariableDescriptor == null)
            {
                throw new Exception("The entityClass (" + EntityDescriptor.EntityClass
                        + ") has an attribute" + typeof(InverseRelationShadowVariableAttribute).Name
                        + " annotated property (" + variableMemberAccessor.GetName()
                        + ") with sourceVariableName (" + sourceVariableName
                        + ") which is not a valid planning variable on entityClass ("
                        + sourceEntityDescriptor.EntityClass + ").\n");
            }
            chained = (sourceVariableDescriptor is GenuineVariableDescriptor) && ((GenuineVariableDescriptor)sourceVariableDescriptor).IsChained();
            bool list = (sourceVariableDescriptor is GenuineVariableDescriptor) && ((GenuineVariableDescriptor)sourceVariableDescriptor).IsListVariable();
            if (singleton)
            {
                if (!chained && !list)
                {
                    throw new Exception("The entityClass (" + EntityDescriptor.EntityClass
                            + ") has an atribute" + typeof(InverseRelationShadowVariableAttribute).Name
                            + " annotated property (" + variableMemberAccessor.GetName()
                            + ") which does not return a collection"
                            + " with sourceVariableName (" + sourceVariableName
                            + ") which is neither a list variable attribute" + typeof(PlanningListVariableAttribute).Name
                            + " nor a chained variable attribute" + typeof(PlanningVariableAttribute).Name
                            + "(graphType=" + PlanningVariableGraphType.CHAINED + ")."
                            + " Only list and chained variables support a singleton inverse.");
                }
            }
            else
            {
                if (chained || list)
                {
                    throw new Exception("The entityClass (" + EntityDescriptor.EntityClass
                            + ") has an attribute" + typeof(InverseRelationShadowVariableAttribute).Name
                + " annotated property (" + variableMemberAccessor.GetName()
                + ") which returns a collection"
                + " (" + variablePropertyType
                + ") with sourceVariableName (" + sourceVariableName
                + ") which is a" + (chained
                        ? " chained variable attribute" + typeof(PlanningVariableAttribute).Name
                                + "(graphType=" + PlanningVariableGraphType.CHAINED
                                + "). A chained variable supports only a singleton inverse."
                                : " list variable attribute" + typeof(PlanningListVariableAttribute).Name
                                        + ". A list variable supports only a singleton inverse."));
                }
            }
            sourceVariableDescriptor.RegisterSinkVariableDescriptor(this);
        }
    }
}
