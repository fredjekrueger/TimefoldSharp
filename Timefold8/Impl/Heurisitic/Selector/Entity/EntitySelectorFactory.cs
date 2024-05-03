using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Nearby;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Mimic;
using TimefoldSharp.Core.Impl.Solver;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity
{
    public class EntitySelectorFactory : AbstractSelectorFactory<Config.Heuristics.Selector.Entity.EntitySelectorConfig>
    {

        Config.Heuristics.Selector.Entity.EntitySelectorConfig entityConfig => (Config.Heuristics.Selector.Entity.EntitySelectorConfig)config;

        public EntitySelectorFactory(Config.Heuristics.Selector.Entity.EntitySelectorConfig entitySelectorConfig)
            : base(entitySelectorConfig)
        {
        }

        public static EntitySelectorFactory Create(Config.Heuristics.Selector.Entity.EntitySelectorConfig entitySelectorConfig)
        {
            return new EntitySelectorFactory(entitySelectorConfig);
        }

        internal EntitySelector BuildEntitySelector(HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, SelectionOrder? inheritedSelectionOrder)
        {
            if (entityConfig.GetMimicSelectorRef() != null)
            {
                return BuildMimicReplaying(configPolicy);
            }
            EntityDescriptor entityDescriptor = DeduceEntityDescriptor(configPolicy, entityConfig.GetEntityClass());
            SelectionCacheType resolvedCacheType = SelectionCacheTypeHelper.Resolve(entityConfig.GetCacheType(), minimumCacheType);
            SelectionOrder? resolvedSelectionOrder = SelectionOrderHelper.Resolve(entityConfig.GetSelectionOrder(), inheritedSelectionOrder);

            if (entityConfig.GetNearbySelectionConfig() != null)
            {
                entityConfig.GetNearbySelectionConfig().ValidateNearby(resolvedCacheType, resolvedSelectionOrder);
            }

            // baseEntitySelector and lower should be SelectionOrder.ORIGINAL if they are going to get cached completely
            bool baseRandomSelection = DetermineBaseRandomSelection(entityDescriptor, resolvedCacheType, resolvedSelectionOrder);
            SelectionCacheType baseSelectionCacheType = SelectionCacheTypeHelper.Max(minimumCacheType, resolvedCacheType);
            EntitySelector entitySelector = BuildBaseEntitySelector(entityDescriptor, baseSelectionCacheType,
                    baseRandomSelection);
            if (config.GetNearbySelectionConfig() != null)
            {
                // TODO Static filtering (such as movableEntitySelectionFilter) should affect nearbySelection
                entitySelector = ApplyNearbySelection(configPolicy, config.GetNearbySelectionConfig(), minimumCacheType,
                        resolvedSelectionOrder, entitySelector);
            }
            ClassInstanceCache instanceCache = configPolicy.BuilderInfo.GetClassInstanceCache();
            entitySelector = ApplyFiltering(entitySelector, instanceCache);
            entitySelector = ApplySorting(resolvedCacheType, resolvedSelectionOrder, entitySelector, instanceCache);
            entitySelector = ApplyProbability(resolvedCacheType, resolvedSelectionOrder, entitySelector, instanceCache);
            entitySelector = ApplyShuffling(resolvedCacheType, resolvedSelectionOrder, entitySelector);
            entitySelector = ApplyCaching(resolvedCacheType, resolvedSelectionOrder, entitySelector);
            entitySelector = ApplySelectedLimit(resolvedSelectionOrder, entitySelector);
            entitySelector = ApplyMimicRecording(configPolicy, entitySelector);
            return entitySelector;
        }

        private EntitySelector ApplyShuffling(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, EntitySelector entitySelector)
        {
            if (resolvedSelectionOrder == SelectionOrder.SHUFFLED)
            {
                entitySelector = new ShufflingEntitySelector(entitySelector, resolvedCacheType);
            }
            return entitySelector;
        }

        private EntitySelector ApplyCaching(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, EntitySelector entitySelector)
        {
            if (!SelectionCacheTypeHelper.IsCached(resolvedCacheType) && resolvedCacheType > entitySelector.GetCacheType())
            {
                entitySelector = new CachingEntitySelector(entitySelector, resolvedCacheType, SelectionOrderHelper.ToRandomSelectionBoolean(resolvedSelectionOrder));
            }
            return entitySelector;
        }

        protected EntitySelector ApplyProbability(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, EntitySelector entitySelector, ClassInstanceCache instanceCache)
        {
            if (resolvedSelectionOrder == SelectionOrder.PROBABILISTIC)
            {
                if (entityConfig.GetProbabilityWeightFactoryClass() == null)
                {
                    throw new Exception("The entitySelectorConfig (" + config
                            + ") with resolvedSelectionOrder (" + resolvedSelectionOrder
                            + ") needs a probabilityWeightFactoryClass ("
                            + config.GetProbabilityWeightFactoryClass() + ").");
                }
                SelectionProbabilityWeightFactory<object> probabilityWeightFactory = instanceCache.NewInstance<SelectionProbabilityWeightFactory<object>>(config,
                        "probabilityWeightFactoryClass", config.GetProbabilityWeightFactoryClass());
                entitySelector = new ProbabilityEntitySelector(entitySelector, resolvedCacheType, probabilityWeightFactory);
            }
            return entitySelector;
        }

        private EntitySelector ApplySelectedLimit(SelectionOrder? resolvedSelectionOrder, EntitySelector entitySelector)
        {
            if (entityConfig.GetSelectedCountLimit() != null)
            {
                entitySelector = new SelectedCountLimitEntitySelector(entitySelector, SelectionOrderHelper.ToRandomSelectionBoolean(resolvedSelectionOrder), entityConfig.GetSelectedCountLimit());
            }
            return entitySelector;
        }

        private EntitySelector ApplyMimicRecording(HeuristicConfigPolicy configPolicy,
                EntitySelector entitySelector)
        {
            if (entityConfig.GetId() != null)
            {
                if (string.IsNullOrWhiteSpace(entityConfig.GetId()))
                {
                    throw new Exception("The entitySelectorConfig (" + config
                            + ") has an empty id (" + config.GetId() + ").");
                }
                MimicRecordingEntitySelector mimicRecordingEntitySelector = new MimicRecordingEntitySelector(entitySelector);
                configPolicy.AddEntityMimicRecorder(entityConfig.GetId(), mimicRecordingEntitySelector);
                entitySelector = mimicRecordingEntitySelector;
            }
            return entitySelector;
        }

        protected EntitySelector ApplySorting(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, EntitySelector entitySelector, ClassInstanceCache instanceCache)
        {
            if (resolvedSelectionOrder == SelectionOrder.SORTED)
            {
                SelectionSorter<object> sorter;
                if (entityConfig.GetSorterManner() != null)
                {
                    EntityDescriptor entityDescriptor = entitySelector.GetEntityDescriptor();
                    if (!EntitySelectorConfig.HasSorter(entityConfig.GetSorterManner(), entityDescriptor))
                    {
                        return entitySelector;
                    }
                    sorter = EntitySelectorConfig.DetermineSorter<Object>(entityConfig.GetSorterManner(), entityDescriptor);
                }
                else if (entityConfig.GetSorterComparatorClass() != null)
                {
                    Comparer<Object> sorterComparator = instanceCache.NewInstance<Comparer<Object>>(config, "sorterComparatorClass", config.GetSorterComparatorClass());
                    sorter = new ComparatorSelectionSorter<object>(sorterComparator, SelectionSorterOrderHelper.Resolve(entityConfig.GetSorterOrder()));
                }
                else if (config.GetSorterWeightFactoryClass() != null)
                {
                    SelectionSorterWeightFactory<object> sorterWeightFactory = instanceCache.NewInstance<SelectionSorterWeightFactory<Object>>(config, "sorterWeightFactoryClass", entityConfig.GetSorterWeightFactoryClass());
                    sorter = new WeightFactorySelectionSorter<object>(sorterWeightFactory, SelectionSorterOrderHelper.Resolve(config.GetSorterOrder()));
                }
                else if (entityConfig.GetSorterClass() != null)
                {
                    sorter = instanceCache.NewInstance<SelectionSorter<object>>(config, "sorterClass", config.GetSorterClass());
                }
                else
                {
                    throw new Exception("The entitySelectorConf).");
                }
                entitySelector = new SortingEntitySelector(entitySelector, resolvedCacheType, sorter);
            }
            return entitySelector;
        }

        private EntitySelector ApplyFiltering(EntitySelector entitySelector, ClassInstanceCache instanceCache)
        {
            EntityDescriptor entityDescriptor = entitySelector.GetEntityDescriptor();
            if (HasFiltering(entityDescriptor))
            {
                List<SelectionFilter<object>> filterList = new List<SelectionFilter<object>>(config.GetFilterClass() == null ? 1 : 2);
                if (config.GetFilterClass() != null)
                {
                    throw new NotImplementedException();
                    //SelectionFilter<Solution_, Object> selectionFilter = instanceCache.NewInstance(config, "filterClass", config.GetFilterClass());
                    //filterList.Add(selectionFilter);
                }
                // Filter out pinned entities
                if (entityDescriptor.HasEffectiveMovableEntitySelectionFilter())
                {
                    filterList.Add(entityDescriptor.GetEffectiveMovableEntitySelectionFilter());
                }
                // Do not filter out initialized entities here for CH and ES, because they can be partially initialized
                // Instead, ValueSelectorConfig.applyReinitializeVariableFiltering() does that.
                entitySelector = new FilteringEntitySelector(entitySelector, filterList);
            }
            return entitySelector;
        }

        private EntitySelector ApplyNearbySelection(HeuristicConfigPolicy configPolicy,
            NearbySelectionConfig nearbySelectionConfig, SelectionCacheType minimumCacheType,
            SelectionOrder? resolvedSelectionOrder, EntitySelector entitySelector)
        {
            throw new NotImplementedException();
        }

        private EntitySelector BuildBaseEntitySelector(EntityDescriptor entityDescriptor, SelectionCacheType minimumCacheType, bool randomSelection)
        {
            if (minimumCacheType == SelectionCacheType.SOLVER)
            {
                // TODO Solver cached entities are not compatible with ConstraintStreams and IncrementalScoreDirector
                // because between phases the entities get cloned
                throw new Exception("The minimumCacheType (" + minimumCacheType
                        + ") is not yet supported. Please use " + SelectionCacheType.PHASE + " instead.");
            }
            // FromSolutionEntitySelector has an intrinsicCacheType STEP
            return new FromSolutionEntitySelector(entityDescriptor, minimumCacheType, randomSelection);
        }

        protected bool DetermineBaseRandomSelection(EntityDescriptor entityDescriptor, SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder)
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
                            || (IsBaseInherentlyCached() && !HasFiltering(entityDescriptor));
                default:
                    throw new Exception("The selectionOrder (" + resolvedSelectionOrder + ") is not implemented.");
            }
        }

        protected bool IsBaseInherentlyCached()
        {
            return true;
        }

        private bool HasFiltering(EntityDescriptor entityDescriptor)
        {
            return entityConfig.GetFilterClass() != null || entityDescriptor.HasEffectiveMovableEntitySelectionFilter();
        }

        protected EntitySelector BuildMimicReplaying(HeuristicConfigPolicy configPolicy)
        {
            bool anyConfigurationParameterDefined = new List<object>
                {
                    config.GetId(),
                    config.GetEntityClass(),
                    config.GetCacheType(),
                    config.GetSelectionOrder(),
                    config.GetNearbySelectionConfig(),
                    config.GetFilterClass(),
                    config.GetSorterManner(),
                    config.GetSorterComparatorClass(),
                    config.GetSorterWeightFactoryClass(),
                    config.GetSorterOrder(),
                    config.GetSorterClass(),
                    config.GetProbabilityWeightFactoryClass(),
                    config.GetSelectedCountLimit()
                }.Any(x => x != null);
            if (anyConfigurationParameterDefined)
            {
                throw new Exception("The entitySelectorConfig (" + config
                        + ") with mimicSelectorRehas another property that is not null.");
            }
            EntityMimicRecorder entityMimicRecorder = configPolicy.GetEntityMimicRecorder(config.GetMimicSelectorRef());
            if (entityMimicRecorder == null)
            {
                throw new Exception("The entitySelectorConfig (" + config
                        + ") has a mimicSelectorRef ) for which no entitySelector with that id exists (in its solver phase).");
            }
            return new MimicReplayingEntitySelector(entityMimicRecorder);
        }

        public EntityDescriptor ExtractEntityDescriptor(HeuristicConfigPolicy configPolicy)
        {
            if (config.GetEntityClass() != null)
            {
                SolutionDescriptor solutionDescriptor = configPolicy.BuilderInfo.SolutionDescriptor;
                EntityDescriptor entityDescriptor =
                        solutionDescriptor.GetEntityDescriptorStrict(config.GetEntityClass());
                if (entityDescriptor == null)
                {
                    throw new Exception("The selectorConfigmplementation's annotated methods too.");
                }
                return entityDescriptor;
            }
            else if (config.GetMimicSelectorRef() != null)
            {
                return configPolicy.GetEntityMimicRecorder(config.GetMimicSelectorRef()).GetEntityDescriptor();
            }
            else
            {
                return null;
            }
        }
    }
}
