using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Decorator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move
{
    public abstract class AbstractMoveSelectorFactory<MoveSelectorConfig_> : AbstractSelectorFactory<MoveSelectorConfig_>,
        MoveSelectorFactory
            where MoveSelectorConfig_ : MoveSelectorConfig<MoveSelectorConfig_>
    {

        public AbstractMoveSelectorFactory(MoveSelectorConfig_ moveSelectorConfig)
            : base(moveSelectorConfig)
        {

        }

        public static AbstractMoveSelectorFactory<AbstractMoveSelectorConfig> Create(AbstractMoveSelectorConfig moveSelectorConfig)
        {
            if (typeof(ChangeMoveSelectorConfig).IsAssignableFrom(moveSelectorConfig.GetType()))
            {
                return new ChangeMoveSelectorFactory((ChangeMoveSelectorConfig)moveSelectorConfig);
            }

            else if (typeof(SwapMoveSelectorConfig).IsAssignableFrom(moveSelectorConfig.GetType()))
            {
                return new SwapMoveSelectorFactory((SwapMoveSelectorConfig)moveSelectorConfig);
            }
            /*} 
        else if (ListChangeMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new ListChangeMoveSelectorFactory<>((ListChangeMoveSelectorConfig)moveSelectorConfig);
    } 
    else if (ListSwapMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new ListSwapMoveSelectorFactory<>((ListSwapMoveSelectorConfig)moveSelectorConfig);
    }
    else if (PillarChangeMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new PillarChangeMoveSelectorFactory<>((PillarChangeMoveSelectorConfig)moveSelectorConfig);
    }
    else if (PillarSwapMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new PillarSwapMoveSelectorFactory<>((PillarSwapMoveSelectorConfig)moveSelectorConfig);
    }*/
            else if (typeof(UnionMoveSelectorConfig).IsAssignableFrom(moveSelectorConfig.GetType()))
            {
                return new UnionMoveSelectorFactory((UnionMoveSelectorConfig)moveSelectorConfig);
            }
            else if (typeof(CartesianProductMoveSelectorConfig).IsAssignableFrom(moveSelectorConfig.GetType()))
            {
                return new CartesianProductMoveSelectorFactory((CartesianProductMoveSelectorConfig)moveSelectorConfig);
            }/*
    else if (SubListChangeMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new SubListChangeMoveSelectorFactory<>((SubListChangeMoveSelectorConfig)moveSelectorConfig);
    }
    else if (SubListSwapMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new SubListSwapMoveSelectorFactory<>((SubListSwapMoveSelectorConfig)moveSelectorConfig);
    }
    else if (SubChainChangeMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new SubChainChangeMoveSelectorFactory<>((SubChainChangeMoveSelectorConfig)moveSelectorConfig);
    }
    else if (SubChainSwapMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new SubChainSwapMoveSelectorFactory<>((SubChainSwapMoveSelectorConfig)moveSelectorConfig);
    }
    else if (TailChainSwapMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new TailChainSwapMoveSelectorFactory<>((TailChainSwapMoveSelectorConfig)moveSelectorConfig);
    }
    else if (MoveIteratorFactoryConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new MoveIteratorFactoryFactory<>((MoveIteratorFactoryConfig)moveSelectorConfig);
    }
    else if (MoveListFactoryConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new MoveListFactoryFactory<>((MoveListFactoryConfig)moveSelectorConfig);
    }
    else if (KOptMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new KOptMoveSelectorFactory<>((KOptMoveSelectorConfig)moveSelectorConfig);
    }
    else if (KOptListMoveSelectorConfig.class.isAssignableFrom(moveSelectorConfig.getClass())) {
        return new KOptListMoveSelectorFactory<>((KOptListMoveSelectorConfig)moveSelectorConfig);
    }*/
            else
            {
                throw new Exception("Unknown");
            }
        }

        public virtual MoveSelector BuildMoveSelector(HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, SelectionOrder inheritedSelectionOrder, bool skipNonDoableMoves)
        {
            AbstractMoveSelectorConfig unfoldedMoveSelectorConfig = BuildUnfoldedMoveSelectorConfig(configPolicy);
            if (unfoldedMoveSelectorConfig != null)
            {
                return AbstractMoveSelectorFactory<AbstractMoveSelectorConfig>.Create(unfoldedMoveSelectorConfig)
                        .BuildMoveSelector(configPolicy, minimumCacheType, inheritedSelectionOrder, skipNonDoableMoves);
            }

            SelectionCacheType resolvedCacheType = SelectionCacheTypeHelper.Resolve(config.MoveSelectorConfigImpl.CacheType, minimumCacheType);
            SelectionOrder? resolvedSelectionOrder = SelectionOrderHelper.Resolve(config.MoveSelectorConfigImpl.SelectionOrder, inheritedSelectionOrder);

            bool randomMoveSelection = DetermineBaseRandomSelection(resolvedCacheType, resolvedSelectionOrder);
            SelectionCacheType selectionCacheType = SelectionCacheTypeHelper.Max(minimumCacheType, resolvedCacheType);
            MoveSelector moveSelector = BuildBaseMoveSelector(configPolicy, selectionCacheType, randomMoveSelection);

            moveSelector = ApplyFiltering(moveSelector, skipNonDoableMoves);
            moveSelector = ApplySorting(resolvedCacheType, resolvedSelectionOrder, moveSelector);
            moveSelector = ApplyProbability(resolvedCacheType, resolvedSelectionOrder, moveSelector);
            moveSelector = ApplyShuffling(resolvedCacheType, resolvedSelectionOrder, moveSelector);
            moveSelector = ApplyCaching(resolvedCacheType, resolvedSelectionOrder, moveSelector);
            moveSelector = ApplySelectedLimit(moveSelector);
            return moveSelector;
        }

        private MoveSelector ApplySelectedLimit(MoveSelector moveSelector)
        {
            if (config.MoveSelectorConfigImpl.SelectedCountLimit != null)
            {
                moveSelector = new SelectedCountLimitMoveSelector(moveSelector, config.MoveSelectorConfigImpl.SelectedCountLimit.Value);
            }
            return moveSelector;
        }

        private MoveSelector ApplyProbability(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, MoveSelector moveSelector)
        {
            if (resolvedSelectionOrder == SelectionOrder.PROBABILISTIC)
            {
                if (config.MoveSelectorConfigImpl.ProbabilityWeightFactoryClass == null)
                {
                    throw new Exception("The moveSelectorConfig (" + config
                            + ") with resolvedSelectionOrder (" + resolvedSelectionOrder
                            + ") needs a probabilityWeightFactoryClass ("
                            + config.MoveSelectorConfigImpl.ProbabilityWeightFactoryClass + ").");
                }
                SelectionProbabilityWeightFactory<Heurisitic.Move.Move> probabilityWeightFactory =
                        ConfigUtils.NewInstance<SelectionProbabilityWeightFactory<Heurisitic.Move.Move>>(config.MoveSelectorConfigImpl.ProbabilityWeightFactoryClass);
                moveSelector = new ProbabilityMoveSelector(moveSelector, resolvedCacheType, probabilityWeightFactory);
            }
            return moveSelector;
        }

        private MoveSelector ApplyCaching(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, MoveSelector moveSelector)
        {
            if (SelectionCacheTypeHelper.IsCached(resolvedCacheType) && resolvedCacheType.CompareTo(moveSelector.GetCacheType()) > 0)
            {
                moveSelector =
                        new CachingMoveSelector(moveSelector, resolvedCacheType, SelectionOrderHelper.ToRandomSelectionBoolean(resolvedSelectionOrder));
            }
            return moveSelector;
        }

        private MoveSelector ApplyShuffling(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, MoveSelector moveSelector)
        {
            if (resolvedSelectionOrder == SelectionOrder.SHUFFLED)
            {
                moveSelector = new ShufflingMoveSelector(moveSelector, resolvedCacheType);
            }
            return moveSelector;
        }

        private MoveSelector ApplyFiltering(MoveSelector moveSelector, bool skipNonDoableMoves)
        {
            /*
             * Do not filter out pointless moves in Construction Heuristics,
             * because the original value of the entity is irrelevant.
             * If the original value is null and the variable is nullable,
             * the change move to null must be done too.
             */
            SelectionFilter<Heurisitic.Move.Move> baseFilter = skipNonDoableMoves ? DoableMoveSelectionFilter.INSTANCE : null;
            if (HasFiltering())
            {
                SelectionFilter<Heurisitic.Move.Move> selectionFilter =
                        ConfigUtils.NewInstance<SelectionFilter<Heurisitic.Move.Move>>(config.MoveSelectorConfigImpl.FilterClass);
                SelectionFilter<Heurisitic.Move.Move> finalFilter =
                        baseFilter == null ? selectionFilter : SelectionFilter<Heurisitic.Move.Move>.Compose(new List<SelectionFilter<Heurisitic.Move.Move>>() { baseFilter, selectionFilter });
                return new FilteringMoveSelector(moveSelector, finalFilter);
            }
            else if (baseFilter != null)
            {
                return new FilteringMoveSelector(moveSelector, baseFilter);
            }
            else
            {
                return moveSelector;
            }
        }

        protected MoveSelector ApplySorting(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder, MoveSelector moveSelector)
        {
            if (resolvedSelectionOrder == SelectionOrder.SORTED)
            {
                SelectionSorter<Heurisitic.Move.Move> sorter;
                if (config.MoveSelectorConfigImpl.SorterComparatorClass != null)
                {
                    Comparer<Heurisitic.Move.Move> sorterComparator = ConfigUtils.NewInstance<Comparer<Heurisitic.Move.Move>>(config.MoveSelectorConfigImpl.SorterComparatorClass);
                    sorter = new ComparatorSelectionSorter<Heurisitic.Move.Move>(sorterComparator, SelectionSorterOrderHelper.Resolve(config.MoveSelectorConfigImpl.SorterOrder));
                }
                else if (config.MoveSelectorConfigImpl.SorterWeightFactoryClass != null)
                {
                    SelectionSorterWeightFactory<Heurisitic.Move.Move> sorterWeightFactory =
                            ConfigUtils.NewInstance<SelectionSorterWeightFactory<Heurisitic.Move.Move>>(config.MoveSelectorConfigImpl.SorterWeightFactoryClass);
                    sorter = new WeightFactorySelectionSorter<Heurisitic.Move.Move>(sorterWeightFactory, SelectionSorterOrderHelper.Resolve(config.MoveSelectorConfigImpl.SorterOrder));
                }
                else if (config.MoveSelectorConfigImpl.SorterClass != null)
                {
                    sorter = ConfigUtils.NewInstance<SelectionSorter<Heurisitic.Move.Move>>(config.MoveSelectorConfigImpl.SorterClass);
                }
                else
                {
                    throw new Exception("The moveSelectorConfig (" + config
                            + ") with resolvedSelectionOrder (" + resolvedSelectionOrder
                            + ") needs a sorterComparatorClass (" + config.MoveSelectorConfigImpl.SorterComparatorClass
                            + ") or a sorterWeightFactoryClass (" + config.MoveSelectorConfigImpl.SorterWeightFactoryClass
                            + ") or a sorterClass (" + config.MoveSelectorConfigImpl.SorterClass + ").");
                }
                moveSelector = new SortingMoveSelector(moveSelector, resolvedCacheType, sorter);
            }
            return moveSelector;
        }

        protected abstract MoveSelector BuildBaseMoveSelector(HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, bool randomSelection);

        protected bool DetermineBaseRandomSelection(SelectionCacheType resolvedCacheType, SelectionOrder? resolvedSelectionOrder)
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
                    return !SelectionCacheTypeHelper.IsCached(resolvedCacheType) || (IsBaseInherentlyCached() && !HasFiltering());
                default:
                    throw new Exception("The selectionOrder (" + resolvedSelectionOrder
                            + ") is not implemented.");
            }
        }

        protected bool IsBaseInherentlyCached()
        {
            return false;
        }

        private bool HasFiltering()
        {
            return config.MoveSelectorConfigImpl.FilterClass != null;
        }

        protected virtual AbstractMoveSelectorConfig BuildUnfoldedMoveSelectorConfig(HeuristicConfigPolicy configPolicy)
        {
            return null;
        }
    }
}