using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Nearby;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Value
{
    public class ValueSelectorConfig : SelectorConfig<ValueSelectorConfig>
    {
        protected string variableName = null;
        protected SelectionCacheType? cacheType = null;
        protected SelectionOrder? selectionOrder = null;
        protected ValueSorterManner? sorterManner = null;
        protected string mimicSelectorRef = null;
        protected NearbySelectionConfig nearbySelectionConfig = null;
        protected long? selectedCountLimit = null;
        protected Type probabilityWeightFactoryClass = null;
        protected Type sorterComparatorClass = null;
        protected SelectionSorterOrder? sorterOrder = null;
        protected Type sorterWeightFactoryClass = null;
        protected Type sorterClass = null;
        protected Type filterClass = null;
        protected Type downcastEntityClass = null;

        protected string id = null;


        public ValueSelectorConfig()
        {
        }

        public ValueSelectorConfig(String variableName)
        {
            this.variableName = variableName;
        }

        public ValueSelectorConfig(ValueSelectorConfig inheritedConfig)
        {
            if (inheritedConfig != null)
            {
                Inherit(inheritedConfig);
            }
        }

        public void SetVariableName(String variableName)
        {
            this.variableName = variableName;
        }

        public long? GetSelectedCountLimit()
        {
            return selectedCountLimit;
        }

        public string GetId()
        {
            return id;
        }

        public static bool HasSorter(ValueSorterManner? valueSorterManner, GenuineVariableDescriptor variableDescriptor)
        {
            switch (valueSorterManner)
            {
                case ValueSorterManner.NONE:
                    return false;
                case ValueSorterManner.INCREASING_STRENGTH:
                case ValueSorterManner.DECREASING_STRENGTH:
                    return true;
                case ValueSorterManner.INCREASING_STRENGTH_IF_AVAILABLE:
                    return variableDescriptor.GetIncreasingStrengthSorter() != null;
                case ValueSorterManner.DECREASING_STRENGTH_IF_AVAILABLE:
                    return variableDescriptor.GetDecreasingStrengthSorter() != null;
                default:
                    throw new Exception("The sorterManner (" + valueSorterManner + ") is not implemented.");
            }
        }

        public ValueSelectorConfig WithCacheType(SelectionCacheType cacheType)
        {
            this.SetCacheType(cacheType);
            return this;
        }

        public void SetSelectionOrder(SelectionOrder selectionOrder)
        {
            this.selectionOrder = selectionOrder;
        }

        public ValueSelectorConfig WithSelectionOrder(SelectionOrder selectionOrder)
        {
            this.SetSelectionOrder(selectionOrder);
            return this;
        }

        public void SetSorterManner(ValueSorterManner sorterManner)
        {
            this.sorterManner = sorterManner;
        }

        public ValueSelectorConfig WithSorterManner(ValueSorterManner sorterManner)
        {
            this.SetSorterManner(sorterManner);
            return this;
        }

        public void SetCacheType(SelectionCacheType cacheType)
        {
            this.cacheType = cacheType;
        }

        public ValueSelectorConfig WithVariableName(String variableName)
        {
            this.SetVariableName(variableName);
            return this;
        }

        public ValueSelectorConfig Inherit(ValueSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public ValueSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public string GetVariableName()
        {
            return variableName;
        }

        public string GetMimicSelectorRef()
        {
            return mimicSelectorRef;
        }

        public SelectionCacheType? GetCacheType()
        {
            return cacheType;
        }

        public SelectionOrder? GetSelectionOrder()
        {
            return selectionOrder;
        }

        public NearbySelectionConfig GetNearbySelectionConfig()
        {
            return nearbySelectionConfig;
        }

        public Type GetProbabilityWeightFactoryClass()
        {
            return probabilityWeightFactoryClass;
        }

        public ValueSorterManner? GetSorterManner()
        {
            return sorterManner;
        }

        public static SelectionSorter<object> DetermineSorter(ValueSorterManner? valueSorterManner, GenuineVariableDescriptor variableDescriptor)
        {
            SelectionSorter<object> sorter;
            switch (valueSorterManner)
            {
                case ValueSorterManner.NONE:
                    throw new Exception("Impossible state: hasSorter() should have returned null.");
                case ValueSorterManner.INCREASING_STRENGTH:
                case ValueSorterManner.INCREASING_STRENGTH_IF_AVAILABLE:
                    sorter = variableDescriptor.GetIncreasingStrengthSorter();
                    break;
                case ValueSorterManner.DECREASING_STRENGTH:
                case ValueSorterManner.DECREASING_STRENGTH_IF_AVAILABLE:
                    sorter = variableDescriptor.GetDecreasingStrengthSorter();
                    break;
                default:
                    throw new Exception("The sorterManner ("
                            + valueSorterManner + ") is not implemented.");
            }
            if (sorter == null)
            {
                throw new Exception("The sorterMann annotation does not declare any strength comparison.");
            }
            return sorter;
        }

        public Type GetSorterComparatorClass()
        {
            return sorterComparatorClass;
        }

        public SelectionSorterOrder? GetSorterOrder()
        {
            return sorterOrder;
        }

        public Type GetSorterWeightFactoryClass()
        {
            return sorterWeightFactoryClass;
        }

        public Type GetSorterClass()
        {
            return sorterClass;
        }

        public Type GetFilterClass()
        {
            return filterClass;
        }

        public Type GetDowncastEntityClass()
        {
            return downcastEntityClass;
        }

        public ValueSelectorConfig WithId(string id)
        {
            this.id = id;
            return this;
        }

        public ValueSelectorConfig WithMimicSelectorRef(string mimicSelectorRef)
        {
            this.mimicSelectorRef = mimicSelectorRef;
            return this;
        }
    }
}
