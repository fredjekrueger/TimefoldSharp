using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Mimic;
using TimefoldSharp.Core.Impl.Solver;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value
{
    public class ValueSelectorFactory : AbstractSelectorFactory<ValueSelectorConfig>
    {
        public ValueSelectorFactory(ValueSelectorConfig valueSelectorConfig)
            : base(valueSelectorConfig)
        {
        }

        public static ValueSelectorFactory Create(ValueSelectorConfig valueSelectorConfig)
        {
            return new ValueSelectorFactory(valueSelectorConfig);
        }

        public ValueSelector BuildValueSelector(HeuristicConfigPolicy configPolicy,
           EntityDescriptor entityDescriptor, SelectionCacheType minimumCacheType,
           SelectionOrder inheritedSelectionOrder)
        {
            return BuildValueSelector(configPolicy, entityDescriptor, minimumCacheType, inheritedSelectionOrder,
                    configPolicy.IsReinitializeVariableFilterEnabled(), ListValueFilteringType.NONE);
        }

        public ValueSelector BuildValueSelector(HeuristicConfigPolicy configPolicy,
           EntityDescriptor entityDescriptor, SelectionCacheType minimumCacheType,
           SelectionOrder? inheritedSelectionOrder, bool applyReinitializeVariableFiltering,
           ListValueFilteringType listValueFilteringType)
        {
            GenuineVariableDescriptor variableDescriptor = DeduceGenuineVariableDescriptor(
                    DowncastEntityDescriptor(configPolicy, entityDescriptor), config.GetVariableName());
            if (config.GetMimicSelectorRef() != null)
            {
                ValueSelector vSelector = BuildMimicReplaying(configPolicy);
                vSelector = ApplyReinitializeVariableFiltering(applyReinitializeVariableFiltering, variableDescriptor, vSelector);
                vSelector = ApplyDowncasting(vSelector);
                return vSelector;
            }
            SelectionCacheType resolvedCacheType = SelectionCacheTypeHelper.Resolve(config.GetCacheType(), minimumCacheType);
            SelectionOrder? resolvedSelectionOrder = SelectionOrderHelper.Resolve(config.GetSelectionOrder(), inheritedSelectionOrder);

            if (config.GetNearbySelectionConfig() != null)
            {
                config.GetNearbySelectionConfig().ValidateNearby(resolvedCacheType, resolvedSelectionOrder);
            }

            // baseValueSelector and lower should be SelectionOrder.ORIGINAL if they are going to get cached completely
            ValueSelector valueSelector =
                    BuildBaseValueSelector(variableDescriptor, SelectionCacheTypeHelper.Max(minimumCacheType, resolvedCacheType),
                            DetermineBaseRandomSelection(variableDescriptor, resolvedCacheType, resolvedSelectionOrder));

            if (config.GetNearbySelectionConfig() != null)
            {
                // TODO Static filtering (such as movableEntitySelectionFilter) should affect nearbySelection too
                valueSelector = ApplyNearbySelection(configPolicy, entityDescriptor, minimumCacheType,
                        resolvedSelectionOrder, valueSelector);
            }
            ClassInstanceCache instanceCache = configPolicy.BuilderInfo.GetClassInstanceCache();
            valueSelector = ApplyFiltering(valueSelector, instanceCache);
            valueSelector = ApplyInitializedChainedValueFilter(configPolicy, variableDescriptor, valueSelector);
            valueSelector = ApplySorting(resolvedCacheType, resolvedSelectionOrder, valueSelector, instanceCache);
            valueSelector = ApplyProbability(resolvedCacheType, resolvedSelectionOrder, valueSelector, instanceCache);
            valueSelector = ApplyShuffling(resolvedCacheType, resolvedSelectionOrder, valueSelector);
            valueSelector = ApplyCaching(resolvedCacheType, resolvedSelectionOrder, valueSelector);
            valueSelector = ApplySelectedLimit(valueSelector);
            valueSelector = ApplyListValueFiltering(configPolicy, listValueFilteringType, variableDescriptor, valueSelector);
            valueSelector = ApplyMimicRecording(configPolicy, valueSelector);
            valueSelector = ApplyReinitializeVariableFiltering(applyReinitializeVariableFiltering, variableDescriptor, valueSelector);
            valueSelector = ApplyDowncasting(valueSelector);
            return valueSelector;
        }

        ValueSelector ApplyListValueFiltering(HeuristicConfigPolicy configPolicy, ListValueFilteringType listValueFilteringType,
           GenuineVariableDescriptor variableDescriptor, ValueSelector valueSelector)
        {
            if (variableDescriptor.IsListVariable() && configPolicy.BuilderInfo.UnassignedValuesAllowed
                    && listValueFilteringType != ListValueFilteringType.NONE)
            {
                if (!(valueSelector is EntityIndependentValueSelector))
                {
                    throw new Exception("The valueSelectorConfig ( annotations.");
                }
                if (listValueFilteringType == ListValueFilteringType.ACCEPT_ASSIGNED)
                {
                    valueSelector = new AssignedValueSelector(((EntityIndependentValueSelector)valueSelector));
                }
                else
                    valueSelector = new UnassignedValueSelector(((EntityIndependentValueSelector)valueSelector));
            }
            return valueSelector;
        }

        private ValueSelector ApplyMimicRecording(HeuristicConfigPolicy configPolicy,
           ValueSelector valueSelector)
        {
            if (config.GetId() != null)
            {
                if (string.IsNullOrWhiteSpace(config.GetId()))
                {
                    throw new Exception("The valueSelectorConfig (" + config + ") has an empty id (" + config.GetId() + ").");
                }
                if (!(valueSelector is EntityIndependentValueSelector))
                {
                    throw new Exception("The valueSelectorConfig ( annotations.");
                }
                MimicRecordingValueSelector mimicRecordingValueSelector = new MimicRecordingValueSelector(
                        (EntityIndependentValueSelector)valueSelector);
                configPolicy.AddValueMimicRecorder(config.GetId(), mimicRecordingValueSelector);
                valueSelector = mimicRecordingValueSelector;
            }
            return valueSelector;
        }


        private ValueSelector ApplySelectedLimit(ValueSelector valueSelector)
        {
            if (config.GetSelectedCountLimit() != null)
            {
                valueSelector = new SelectedCountLimitValueSelector(valueSelector, config.GetSelectedCountLimit());
            }
            return valueSelector;
        }

        private ValueSelector ApplyShuffling(SelectionCacheType resolvedCacheType,
            SelectionOrder? resolvedSelectionOrder, ValueSelector valueSelector)
        {
            if (resolvedSelectionOrder == SelectionOrder.SHUFFLED)
            {
                if (!(valueSelector is EntityIndependentValueSelector))
                {
                    throw new Exception("The valueSelectorConfig ( annotations.");
                }
                valueSelector = new ShufflingValueSelector((EntityIndependentValueSelector)valueSelector, resolvedCacheType);
            }
            return valueSelector;
        }

        private ValueSelector ApplyCaching(SelectionCacheType resolvedCacheType,
            SelectionOrder? resolvedSelectionOrder, ValueSelector valueSelector)
        {
            if (SelectionCacheTypeHelper.IsCached(resolvedCacheType) && resolvedCacheType.CompareTo(valueSelector.GetCacheType()) > 0)
            {
                if (!(valueSelector is EntityIndependentValueSelector))
                {
                    throw new Exception("The valueSelectorConfig annotations.");
                }
                valueSelector = new CachingValueSelector((EntityIndependentValueSelector)valueSelector,
                        resolvedCacheType, SelectionOrderHelper.ToRandomSelectionBoolean(resolvedSelectionOrder));
            }
            return valueSelector;
        }

        protected ValueSelector ApplyProbability(SelectionCacheType resolvedCacheType,
           SelectionOrder? resolvedSelectionOrder, ValueSelector valueSelector, ClassInstanceCache instanceCache)
        {
            if (resolvedSelectionOrder == SelectionOrder.PROBABILISTIC)
            {
                if (config.GetProbabilityWeightFactoryClass() == null)
                {
                    throw new Exception("The valueSelectorConfig ).");
                }
                SelectionProbabilityWeightFactory<object> probabilityWeightFactory =
                    instanceCache.NewInstance<SelectionProbabilityWeightFactory<object>>(config, "probabilityWeightFactoryClass", config.GetProbabilityWeightFactoryClass());
                if (!(valueSelector is EntityIndependentValueSelector))
                {
                    throw new Exception("The valueSelectorConfig  annotations.");
                }
                valueSelector = new ProbabilityValueSelector((EntityIndependentValueSelector)valueSelector, resolvedCacheType, probabilityWeightFactory);
            }
            return valueSelector;
        }

        protected ValueSelector ApplySorting(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder,
            ValueSelector valueSelector, ClassInstanceCache instanceCache)
        {
            if (resolvedSelectionOrder == SelectionOrder.SORTED)
            {
                SelectionSorter<Object> sorter;
                if (config.GetSorterManner() != null)
                {
                    GenuineVariableDescriptor variableDescriptor = valueSelector.GetVariableDescriptor();
                    if (!ValueSelectorConfig.HasSorter(config.GetSorterManner(), variableDescriptor))
                    {
                        return valueSelector;
                    }
                    sorter = ValueSelectorConfig.DetermineSorter(config.GetSorterManner(), variableDescriptor);
                }
                else if (config.GetSorterComparatorClass() != null)
                {
                    Comparer<object> sorterComparator =
                            instanceCache.NewInstance<Comparer<object>>(config, "sorterComparatorClass", config.GetSorterComparatorClass());
                    sorter = new ComparatorSelectionSorter<object>(sorterComparator, SelectionSorterOrderHelper.Resolve(config.GetSorterOrder()));
                }
                else if (config.GetSorterWeightFactoryClass() != null)
                {
                    SelectionSorterWeightFactory<object> sorterWeightFactory =
                            instanceCache.NewInstance<SelectionSorterWeightFactory<object>>(config, "sorterWeightFactoryClass", config.GetSorterWeightFactoryClass());
                    sorter = new WeightFactorySelectionSorter<object>(sorterWeightFactory, SelectionSorterOrderHelper.Resolve(config.GetSorterOrder()));
                }
                else if (config.GetSorterClass() != null)
                {
                    sorter = instanceCache.NewInstance<SelectionSorter<object>>(config, "sorterClass", config.GetSorterClass());
                }
                else
                {
                    throw new Exception("The valueSelectorConfig).");
                }
                if (!valueSelector.GetVariableDescriptor().IsValueRangeEntityIndependent()
                        && resolvedCacheType == SelectionCacheType.STEP)
                {
                    valueSelector = new EntityDependentSortingValueSelector(valueSelector, resolvedCacheType, sorter);
                }
                else
                {
                    if (!(valueSelector is EntityIndependentValueSelector))
                    {
                        throw new Exception("The valueSelectorConfig  annotations.");
                    }
                    valueSelector = new SortingValueSelector((EntityIndependentValueSelector)valueSelector,
                            resolvedCacheType, sorter);
                }
            }
            return valueSelector;
        }

        protected ValueSelector ApplyInitializedChainedValueFilter(HeuristicConfigPolicy configPolicy,
           GenuineVariableDescriptor variableDescriptor, ValueSelector valueSelector)
        {
            if (configPolicy.BuilderInfo.InitializedChainedValueFilterEnabled && variableDescriptor.IsChained())
            {
                valueSelector = InitializedValueSelector.Create(valueSelector);
            }
            return valueSelector;
        }

        protected ValueSelector ApplyFiltering(ValueSelector valueSelector, ClassInstanceCache instanceCache)
        {
            GenuineVariableDescriptor variableDescriptor = valueSelector.GetVariableDescriptor();
            if (HasFiltering(variableDescriptor))
            {
                List<SelectionFilter<object>> filterList = new List<SelectionFilter<object>>(config.GetFilterClass() == null ? 1 : 2);
                if (config.GetFilterClass() != null)
                {
                    filterList.Add(instanceCache.NewInstance<SelectionFilter<object>>(config, "filterClass", config.GetFilterClass()));
                }
                // Filter out pinned entities
                if (variableDescriptor.HasMovableChainedTrailingValueFilter())
                {
                    filterList.Add(variableDescriptor.GetMovableChainedTrailingValueFilter());
                }
                valueSelector = FilteringValueSelector.Create(valueSelector, filterList);
            }
            return valueSelector;
        }

        private bool HasFiltering(GenuineVariableDescriptor variableDescriptor)
        {
            return config.GetFilterClass() != null || variableDescriptor.HasMovableChainedTrailingValueFilter();
        }

        private ValueSelector ApplyNearbySelection(HeuristicConfigPolicy configPolicy,
            EntityDescriptor entityDescriptor, SelectionCacheType minimumCacheType,
            SelectionOrder? resolvedSelectionOrder, ValueSelector valueSelector)
        {
            throw new NotImplementedException();
            /*return NearbySelectionEnterpriseService.Load()
                    .applyNearbySelection(config, configPolicy, entityDescriptor, minimumCacheType, resolvedSelectionOrder,
                            valueSelector);*/
        }

        protected bool DetermineBaseRandomSelection(GenuineVariableDescriptor variableDescriptor,
            SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder)
        {
            switch (resolvedSelectionOrder)
            {
                case SelectionOrder.ORIGINAL:
                    return false;
                case SelectionOrder.SORTED:
                case SelectionOrder.SHUFFLED:
                case SelectionOrder.PROBABILISTIC:
                    // baseValueSelector and lower should be ORIGINAL if they are going to get cached completely
                    return false;
                case SelectionOrder.RANDOM:
                    // Predict if caching will occur
                    return !SelectionCacheTypeHelper.IsCached(resolvedCacheType)
                            || (IsBaseInherentlyCached(variableDescriptor) && !HasFiltering(variableDescriptor));
                default:
                    throw new Exception("The selectionOrder (" + resolvedSelectionOrder
                            + ") is not implemented.");
            }
        }

        protected bool IsBaseInherentlyCached(GenuineVariableDescriptor variableDescriptor)
        {
            return variableDescriptor.IsValueRangeEntityIndependent();
        }

        private ValueSelector BuildBaseValueSelector(GenuineVariableDescriptor variableDescriptor,
            SelectionCacheType minimumCacheType, bool randomSelection)
        {
            ValueRangeDescriptor valueRangeDescriptor = variableDescriptor.ValueRangeDescriptor;
            // TODO minimumCacheType SOLVER is only a problem if the valueRange includes entities or custom weird cloning
            if (minimumCacheType == SelectionCacheType.SOLVER)
            {
                // TODO Solver cached entities are not compatible with ConstraintStreams and IncrementalScoreDirector
                // because between phases the entities get cloned
                throw new Exception("The minimumCacheType (" + minimumCacheType
                        + ") is not yet supported. Please use " + SelectionCacheType.PHASE + " instead.");
            }
            if (valueRangeDescriptor.IsEntityIndependent())
            {
                return new FromSolutionPropertyValueSelector(
                        (EntityIndependentValueRangeDescriptor)valueRangeDescriptor, minimumCacheType,
                        randomSelection);
            }
            else
            {
                // TODO Do not allow PHASE cache on FromEntityPropertyValueSelector, except if the moveSelector is PHASE cached too.
                return new FromEntityPropertyValueSelector(valueRangeDescriptor, randomSelection);
            }
        }

        private ValueSelector ApplyReinitializeVariableFiltering(bool applyReinitializeVariableFiltering,
           GenuineVariableDescriptor variableDescriptor, ValueSelector valueSelector)
        {
            if (applyReinitializeVariableFiltering && !variableDescriptor.IsListVariable())
            {
                valueSelector = new ReinitializeVariableValueSelector(valueSelector);
            }
            return valueSelector;
        }

        private ValueSelector ApplyDowncasting(ValueSelector valueSelector)
        {
            if (config.GetDowncastEntityClass() != null)
            {
                valueSelector = new DowncastingValueSelector(valueSelector, config.GetDowncastEntityClass());
            }
            return valueSelector;
        }

        private ValueSelector applyReinitializeVariableFiltering(bool applyReinitializeVariableFiltering,
            GenuineVariableDescriptor variableDescriptor, ValueSelector valueSelector)
        {
            if (applyReinitializeVariableFiltering && !variableDescriptor.IsListVariable())
            {
                valueSelector = new ReinitializeVariableValueSelector(valueSelector);
            }
            return valueSelector;
        }

        protected ValueSelector BuildMimicReplaying(HeuristicConfigPolicy configPolicy)
        {
            if (config.GetId() != null
                    || config.GetVariableName() != null
                    || config.GetCacheType() != null
                    || config.GetSelectionOrder() != null
                    || config.GetNearbySelectionConfig() != null
                    || config.GetFilterClass() != null
                    || config.GetSorterManner() != null
                    || config.GetSorterComparatorClass() != null
                    || config.GetSorterWeightFactoryClass() != null
                    || config.GetSorterOrder() != null
                    || config.GetSorterClass() != null
                    || config.GetProbabilityWeightFactoryClass() != null
                    || config.GetSelectedCountLimit() != null)
            {
                throw new Exception("The valueSelectorConfig (" + config
                        + ") with mimicSelectorRef (" + config.GetMimicSelectorRef()
                        + ") has another property that is not null.");
            }
            ValueMimicRecorder valueMimicRecorder = configPolicy.GetValueMimicRecorder(config.GetMimicSelectorRef());
            if (valueMimicRecorder == null)
            {
                throw new Exception("The valueSelectorConfig (" + config
                        + ") has a mimicSelectorRef (" + config.GetMimicSelectorRef()
                        + ") for which no valueSelector with that id exists (in its solver phase).");
            }
            return new MimicReplayingValueSelector(valueMimicRecorder);
        }

        protected EntityDescriptor DowncastEntityDescriptor(HeuristicConfigPolicy configPolicy, EntityDescriptor entityDescriptor)
        {
            if (config.GetDowncastEntityClass() != null)
            {
                Type parentEntityClass = entityDescriptor.EntityClass;
                if (!parentEntityClass.IsAssignableFrom(config.GetDowncastEntityClass()))
                {
                    throw new Exception("The downcastEntityClass (" + config.GetDowncastEntityClass()
                            + ") is not a subclass of the parentEntityClass (" + parentEntityClass
                            + ") configured by the .");
                }
                SolutionDescriptor solutionDescriptor = configPolicy.BuilderInfo.SolutionDescriptor;
                entityDescriptor = solutionDescriptor.GetEntityDescriptorStrict(config.GetDowncastEntityClass());
                if (entityDescriptor == null)
                {
                    throw new Exception("The selectorConfig (" + config
                            + ") has an downcastEntityClass (" + config.GetDowncastEntityClass()
                            + ") that is not a known planning entity.\n"
                            + "Check your solver configuration. If that class (" + config.GetDowncastEntityClass().Name
                            + ") is not in the entityClassSet ), check your @implementation's annotated methods too.");
                }
            }
            return entityDescriptor;
        }

        public GenuineVariableDescriptor ExtractVariableDescriptor(HeuristicConfigPolicy configPolicy, EntityDescriptor entityDescriptor)
        {
            String variableName = config.GetVariableName();
            if (variableName != null)
            {
                return GetVariableDescriptorForName(DowncastEntityDescriptor(configPolicy, entityDescriptor), variableName);
            }
            else if (config.GetMimicSelectorRef() != null)
            {
                return configPolicy.GetValueMimicRecorder(config.GetMimicSelectorRef()).GetVariableDescriptor();
            }
            else
            {
                return null;
            }
        }
    }

    public enum ListValueFilteringType
    {
        NONE,
        ACCEPT_ASSIGNED,
        ACCEPT_UNASSIGNED
    }
}