using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Nearby;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Entity
{
    public class EntitySelectorConfig : SelectorConfig<EntitySelectorConfig>
    {
        protected Type probabilityWeightFactoryClass = null;
        protected String id = null;
        protected Type entityClass = null;
        protected String mimicSelectorRef = null;
        protected SelectionCacheType? cacheType = null;
        protected SelectionOrder? selectionOrder = null;
        protected NearbySelectionConfig nearbySelectionConfig = null;
        protected Type filterClass;
        protected EntitySorterManner? sorterManner = null;
        protected Type sorterComparatorClass = null;
        protected Type sorterClass = null;
        protected Type sorterWeightFactoryClass = null;
        protected SelectionSorterOrder? sorterOrder = null;


        public EntitySelectorConfig()
        {
        }

        public EntitySelectorConfig(Type entityClass)
        {
            this.entityClass = entityClass;
        }

        public EntitySelectorConfig(EntitySelectorConfig inheritedConfig)
        {
            if (inheritedConfig != null)
            {
                Inherit(inheritedConfig);
            }
        }

        public EntitySelectorConfig CopyConfig()
        {
            return new EntitySelectorConfig().Inherit(this);
        }

        public EntitySelectorConfig WithMimicSelectorRef(string mimicSelectorRef)
        {
            this.SetMimicSelectorRef(mimicSelectorRef);
            return this;
        }

        public void SetMimicSelectorRef(String mimicSelectorRef)
        {
            this.mimicSelectorRef = mimicSelectorRef;
        }

        public static EntitySelectorConfig NewMimicSelectorConfig(string mimicSelectorRef)
        {
            return new EntitySelectorConfig().WithMimicSelectorRef(mimicSelectorRef);
        }

        public String GetMimicSelectorRef()
        {
            return mimicSelectorRef;
        }

        public EntitySelectorConfig Inherit(EntitySelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public EntitySelectorConfig WithId(String id)
        {
            this.SetId(id);
            return this;
        }

        public EntitySelectorConfig WithEntityClass(Type entityClass)
        {
            this.SetEntityClass(entityClass);
            return this;
        }

        public void SetEntityClass(Type entityClass)
        {
            this.entityClass = entityClass;
        }

        public void SetId(String id)
        {
            this.id = id;
        }

        public string GetId()
        {
            return id;
        }

        public static bool HasSorter(EntitySorterManner? entitySorterManner, EntityDescriptor entityDescriptor)
        {
            switch (entitySorterManner)
            {
                case EntitySorterManner.NONE:
                    return false;
                case EntitySorterManner.DECREASING_DIFFICULTY:
                    return true;
                case EntitySorterManner.DECREASING_DIFFICULTY_IF_AVAILABLE:
                    return entityDescriptor.GetDecreasingDifficultySorter() != null;
                default:
                    throw new Exception("The sorterManner (" + entitySorterManner + ") is not implemented.");
            }
        }

        public Type GetEntityClass()
        {
            return entityClass;
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

        public Type GetFilterClass()
        {
            return filterClass;
        }

        public Type GetProbabilityWeightFactoryClass()
        {
            return probabilityWeightFactoryClass;
        }

        protected long? selectedCountLimit = null;
        public long? GetSelectedCountLimit()
        {
            return selectedCountLimit;
        }

        public EntitySorterManner? GetSorterManner()
        {
            return sorterManner;
        }

        public static SelectionSorter<T> DetermineSorter<T>(EntitySorterManner? entitySorterManner, EntityDescriptor entityDescriptor)
        {
            SelectionSorter<T> sorter;
            switch (entitySorterManner)
            {
                case EntitySorterManner.NONE:
                    throw new Exception("Impossible state: hasSorter() should have returned null.");
                case EntitySorterManner.DECREASING_DIFFICULTY:
                case EntitySorterManner.DECREASING_DIFFICULTY_IF_AVAILABLE:
                    sorter = (SelectionSorter<T>)entityDescriptor.GetDecreasingDifficultySorter();
                    if (sorter == null)
                    {
                        throw new Exception("The sorterMann annotation does not declare any difficulty comparison.");
                    }
                    return sorter;
                default:
                    throw new Exception("The sorterManner (" + entitySorterManner + ") is not implemented.");
            }
        }

        public Type GetSorterComparatorClass()
        {
            return sorterComparatorClass;
        }

        public Type GetSorterClass()
        {
            return sorterClass;
        }

        public Type GetSorterWeightFactoryClass()
        {
            return sorterWeightFactoryClass;
        }

        public SelectionSorterOrder? GetSorterOrder()
        {
            return sorterOrder;
        }
    }
}
