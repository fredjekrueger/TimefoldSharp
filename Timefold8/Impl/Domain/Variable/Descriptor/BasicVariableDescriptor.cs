using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Policy;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Descriptor
{
    public class BasicVariableDescriptor : GenuineVariableDescriptor
    {

        public BasicVariableDescriptor(EntityDescriptor entityDescriptor, MemberAccessor variableMemberAccessor) : base(entityDescriptor, variableMemberAccessor)
        {
        }

        protected override void ProcessPropertyAnnotations(DescriptorPolicy descriptorPolicy)
        {
            PlanningVariableAttribute planningVariableAnnotation = variableMemberAccessor.GetAnnotation<PlanningVariableAttribute>(typeof(PlanningVariableAttribute));
            ProcessNullable(planningVariableAnnotation);
            ProcessChained(planningVariableAnnotation);
            ProcessValueRangeRefs(descriptorPolicy, planningVariableAnnotation.ValueRangeProviderRefs);
            ProcessStrength(planningVariableAnnotation.StrengthComparatorClass,
                    planningVariableAnnotation.StrengthWeightFactoryClass);
        }

        private void ProcessNullable(PlanningVariableAttribute planningVariableAnnotation)
        {
            nullable = planningVariableAnnotation.Nullable;
            if (nullable && variableMemberAccessor.GetClass().IsPrimitive)
            {
                throw new Exception("The entityCl).");
            }
        }

        private void ProcessChained(PlanningVariableAttribute planningVariableAnnotation)
        {
            chained = planningVariableAnnotation.GraphType == PlanningVariableGraphType.CHAINED;
            if (!chained)
            {
                return;
            }
            if (!AcceptsValueType(EntityDescriptor.EntityClass))
            {
                throw new Exception("The entityClass If an entity's chained planning variable cannot point to another entity of the same class,"
                        + " then it is impossible to make a chain longer than 1 entity and therefore chaining is useless.");
            }
            if (nullable)
            {
                throw new Exception("The entityClas");
            }
        }

        public override bool AcceptsValueType(Type valueType)
        {
            return GetVariablePropertyType().IsAssignableFrom(valueType);
        }

        private bool chained;
        private bool nullable;

        public override bool IsChained()
        {
            return chained;
        }

        public override bool IsInitialized(object entity)
        {
            if (IsNullable())
            {
                return true;
            }
            object variable = GetValue(entity);
            return variable != null;
        }

        public override bool IsListVariable()
        {
            return false;
        }

        public override bool IsNullable()
        {
            return nullable;
        }
    }
}
