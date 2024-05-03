using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public sealed class FixedSelectorProbabilityWeightFactory<Selector_>
        : SelectionProbabilityWeightFactory<Selector_>
        where Selector_ : Selector
    {

        private readonly Dictionary<Selector_, double?> fixedProbabilityWeightMap;

        public FixedSelectorProbabilityWeightFactory(Dictionary<Selector_, double?> fixedProbabilityWeightMap)
        {
            this.fixedProbabilityWeightMap = fixedProbabilityWeightMap;
        }
    }
}
