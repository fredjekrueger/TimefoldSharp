using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Policy;
using TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Descriptor
{
    public abstract class GenuineVariableDescriptor : VariableDescriptor
    {
        public abstract bool IsInitialized(object entity);

        public abstract bool IsListVariable();

        public abstract bool AcceptsValueType(Type valueType);

        private SelectionSorter<Object> increasingStrengthSorter;
        private SelectionSorter<Object> decreasingStrengthSorter;

        public ValueRangeDescriptor ValueRangeDescriptor { get; set; }

        private SelectionFilter<object> movableChainedTrailingValueFilter;

        public GenuineVariableDescriptor(EntityDescriptor entityDescriptor, MemberAccessor variableMemberAccessor) : base(entityDescriptor, variableMemberAccessor)
        {

        }

        public override bool IsGenuineAndUninitialized(object entity)
        {
            return !IsInitialized(entity);
        }

        public long GetValueCount(ISolution solution, Object entity)
        {
            return ValueRangeDescriptor.ExtractValueRangeSize(solution, entity);
        }

        public bool IsValueRangeEntityIndependent()
        {
            return ValueRangeDescriptor.IsEntityIndependent();
        }

        public SelectionSorter<object> GetIncreasingStrengthSorter()
        {
            return increasingStrengthSorter;

        }
        public SelectionSorter<object> GetDecreasingStrengthSorter()
        {
            return decreasingStrengthSorter;
        }

        public override void LinkVariableDescriptors(DescriptorPolicy descriptorPolicy)
        {
            if (IsChained() && EntityDescriptor.HasEffectiveMovableEntitySelectionFilter())
            {
                movableChainedTrailingValueFilter = new MovableChainedTrailingValueFilter(this);
            }
            else
            {
                movableChainedTrailingValueFilter = null;
            }
        }

        public abstract bool IsChained();

        public abstract bool IsNullable();

        public void ProcessAnnotations(DescriptorPolicy descriptorPolicy)
        {
            ProcessPropertyAnnotations(descriptorPolicy);
        }

        protected abstract void ProcessPropertyAnnotations(DescriptorPolicy descriptorPolicy);

        protected void ProcessStrength(Type strengthComparatorClass, Type strengthWeightFactoryClass)
        {
            if (strengthComparatorClass == typeof(PlanningVariableAttribute.NullStrengthComparator))
            {
                strengthComparatorClass = null;
            }
            if (strengthWeightFactoryClass == typeof(PlanningVariableAttribute.NullStrengthWeightFactory))
            {
                strengthWeightFactoryClass = null;
            }
            if (strengthComparatorClass != null && strengthWeightFactoryClass != null)
            {
                throw new Exception("The entityime.");
            }
            if (strengthComparatorClass != null)
            {
                /*Comparator<object> strengthComparator = ConfigUtils.newInstance(this::toString,
                        "strengthComparatorClass", strengthComparatorClass);
                increasingStrengthSorter = new ComparatorSelectionSorter<>(strengthComparator,
                        SelectionSorterOrder.ASCENDING);
                decreasingStrengthSorter = new ComparatorSelectionSorter<>(strengthComparator,
                        SelectionSorterOrder.DESCENDING);*/
            }
            if (strengthWeightFactoryClass != null)
            {
                /* SelectionSorterWeightFactory<Solution_, Object> strengthWeightFactory = ConfigUtils.newInstance(this::toString,
                         "strengthWeightFactoryClass", strengthWeightFactoryClass);
                 increasingStrengthSorter = new WeightFactorySelectionSorter<>(strengthWeightFactory,
                         SelectionSorterOrder.ASCENDING);
                 decreasingStrengthSorter = new WeightFactorySelectionSorter<>(strengthWeightFactory,
                         SelectionSorterOrder.DESCENDING);*/
            }
        }


        protected void ProcessValueRangeRefs(DescriptorPolicy descriptorPolicy, string[] valueRangeProviderRefs)
        {
            MemberAccessor[] valueRangeProviderMemberAccessors;
            if (valueRangeProviderRefs == null || valueRangeProviderRefs.Length == 0)
            {
                valueRangeProviderMemberAccessors = FindAnonymousValueRangeMemberAccessors(descriptorPolicy);
                if (valueRangeProviderMemberAccessors.Length == 0)
                {
                    throw new Exception("The entityCla and no matching anonymous value range providers were found.");
                }
            }
            else
            {
                valueRangeProviderMemberAccessors = valueRangeProviderRefs.Select(refe => FindValueRangeMemberAccessor(descriptorPolicy, refe)).ToArray();
            }
            var valueRangeDescriptorList = new List<ValueRangeDescriptor>(valueRangeProviderMemberAccessors.Length);
            bool addNullInValueRange = IsNullable() && valueRangeProviderMemberAccessors.Length == 1;
            foreach (var valueRangeProviderMemberAccessor in valueRangeProviderMemberAccessors)
            {
                valueRangeDescriptorList.Add(BuildValueRangeDescriptor(descriptorPolicy, valueRangeProviderMemberAccessor, addNullInValueRange));
            }
            if (valueRangeDescriptorList.Count == 1)
            {
                ValueRangeDescriptor = valueRangeDescriptorList.ElementAt(0);
            }
            else
            {
                ValueRangeDescriptor = new CompositeValueRangeDescriptor(this, IsNullable(), valueRangeDescriptorList);
            }
        }

        private MemberAccessor FindValueRangeMemberAccessor(DescriptorPolicy descriptorPolicy, string valueRangeProviderRef)
        {
            if (descriptorPolicy.HasFromSolutionValueRangeProvider(valueRangeProviderRef))
            {
                return descriptorPolicy.GetFromSolutionValueRangeProvider(valueRangeProviderRef);
            }
            else if (descriptorPolicy.HasFromEntityValueRangeProvider(valueRangeProviderRef))
            {
                return descriptorPolicy.GetFromEntityValueRangeProvider(valueRangeProviderRef);
            }
            else
            {
                descriptorPolicy.GetValueRangeProviderIds();
                throw new Exception("The entit");
            }
        }

        private ValueRangeDescriptor BuildValueRangeDescriptor(DescriptorPolicy descriptorPolicy,
                    MemberAccessor valueRangeProviderMemberAccessor, bool addNullInValueRange)
        {
            if (descriptorPolicy.IsFromSolutionValueRangeProvider(valueRangeProviderMemberAccessor))
            {
                return new FromSolutionPropertyValueRangeDescriptor(this, addNullInValueRange, valueRangeProviderMemberAccessor);
            }
            else if (descriptorPolicy.IsFromEntityValueRangeProvider(valueRangeProviderMemberAccessor))
            {
                return new FromEntityPropertyValueRangeDescriptor(this, addNullInValueRange, valueRangeProviderMemberAccessor);
            }
            else
            {
                throw new Exception("Impossible state: member accessor (" + valueRangeProviderMemberAccessor
                        + ") is not a value range provider.");
            }
        }

        private MemberAccessor[] FindAnonymousValueRangeMemberAccessors(DescriptorPolicy descriptorPolicy)
        {
            bool supportsValueRangeProviderFromEntity = !this.IsListVariable();
            IEnumerable<MemberAccessor> applicableValueRangeProviderAccessors;

            if (supportsValueRangeProviderFromEntity)
            {
                applicableValueRangeProviderAccessors = descriptorPolicy.GetAnonymousFromEntityValueRangeProviderSet()
                    .Concat(descriptorPolicy.GetAnonymousFromSolutionValueRangeProviderSet());
            }
            else
            {
                applicableValueRangeProviderAccessors = descriptorPolicy.GetAnonymousFromSolutionValueRangeProviderSet();
            }

            return applicableValueRangeProviderAccessors
                    .Where(valueRangeProviderAccessor =>
                    {
                        /*
                         * For basic variable, the type is the type of the variable.
                         * For list variable, the type is List<X>, and we need to know X.
                         */
                        Type variableType =
                                IsListVariable() ? (variableMemberAccessor.GetGenericType())
                                        .GetGenericArguments()[0] : variableMemberAccessor.GetClass();
                        // We expect either ValueRange, Collection or an array.
                        Type valueRangeType = valueRangeProviderAccessor.GetGenericType();
                        if (valueRangeType.IsGenericType)
                        {
                            //if (!typeof(ValueRange<>).IsAssignableFrom(valueRangeType) && !typeof(Collection<>).IsAssignableFrom(valueRangeType))
                            if (!(valueRangeType.GetGenericTypeDefinition() == typeof(List<>)))
                            {
                                return false;
                            }
                            Type[] generics = valueRangeType.GenericTypeArguments;
                            if (generics.Length != 1)
                            {
                                return false;
                            }
                            var valueRangeGenericType = generics[0];
                            return variableType.IsAssignableFrom(valueRangeGenericType);
                        }
                        else
                        {
                            Type clz = valueRangeType;
                            if (clz.IsArray)
                            {
                                var componentType = clz.GetElementType();
                                return variableType.IsAssignableFrom(componentType);
                            }
                            return false;
                        }
                    })
                .ToArray();
        }

        public bool HasMovableChainedTrailingValueFilter()
        {
            return movableChainedTrailingValueFilter != null;
        }

        public SelectionFilter<object> GetMovableChainedTrailingValueFilter()
        {
            return movableChainedTrailingValueFilter;
        }
    }
}
