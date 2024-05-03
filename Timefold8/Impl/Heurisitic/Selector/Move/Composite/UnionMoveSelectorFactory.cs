using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public class UnionMoveSelectorFactory : AbstractCompositeMoveSelectorFactory<UnionMoveSelectorConfig>
    {

        UnionMoveSelectorConfig uMoveConfig => (UnionMoveSelectorConfig)config;

        public UnionMoveSelectorFactory(UnionMoveSelectorConfig moveSelectorConfig)
            : base(moveSelectorConfig)
        {
        }

        protected override MoveSelector BuildBaseMoveSelector(HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, bool randomSelection)
        {
            List<MoveSelector> moveSelectorList = BuildInnerMoveSelectors(uMoveConfig.GetMoveSelectorList(), configPolicy, minimumCacheType, randomSelection);

            SelectionProbabilityWeightFactory<MoveSelector> selectorProbabilityWeightFactory;
            if (uMoveConfig.GetSelectorProbabilityWeightFactoryClass() != null)
            {
                if (!randomSelection)
                {
                    throw new Exception("The moveSelectorConfig (" + config
                            + ") with selectorProbabilityWeightFactoryClass ("
                            + uMoveConfig.GetSelectorProbabilityWeightFactoryClass()
                            + ") has non-random randomSelection (" + randomSelection + ").");
                }
                selectorProbabilityWeightFactory = ConfigUtils.NewInstance<SelectionProbabilityWeightFactory<MoveSelector>>(config,
                        "selectorProbabilityWeightFactoryClass", uMoveConfig.GetSelectorProbabilityWeightFactoryClass());
            }
            else if (randomSelection)
            {
                Dictionary<MoveSelector, double?> fixedProbabilityWeightMap = new Dictionary<MoveSelector, double?>();
                for (int i = 0; i < uMoveConfig.GetMoveSelectorList().Count; i++)
                {
                    var innerMoveSelectorConfig = uMoveConfig.GetMoveSelectorList()[i];
                    MoveSelector moveSelector = moveSelectorList[i];
                    double? fixedProbabilityWeight = innerMoveSelectorConfig.GetFixedProbabilityWeight();
                    if (fixedProbabilityWeight != null)
                    {
                        fixedProbabilityWeightMap.Add(moveSelector, fixedProbabilityWeight);
                    }
                }
                if (fixedProbabilityWeightMap.Count == 0)
                { // Will end up using UniformRandomUnionMoveIterator.
                    selectorProbabilityWeightFactory = null;
                }
                else
                { // Will end up using BiasedRandomUnionMoveIterator.
                    selectorProbabilityWeightFactory = new FixedSelectorProbabilityWeightFactory<MoveSelector>(fixedProbabilityWeightMap);
                }
            }
            else
            {
                selectorProbabilityWeightFactory = null;
            }
            return new UnionMoveSelector(moveSelectorList, randomSelection, selectorProbabilityWeightFactory);
        }


    }
}
