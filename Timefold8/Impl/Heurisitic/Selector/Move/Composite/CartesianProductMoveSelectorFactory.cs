using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public class CartesianProductMoveSelectorFactory : AbstractCompositeMoveSelectorFactory<CartesianProductMoveSelectorConfig>
    {

        public CartesianProductMoveSelectorConfig cartConfig => (CartesianProductMoveSelectorConfig)config;

        public CartesianProductMoveSelectorFactory(CartesianProductMoveSelectorConfig moveSelectorConfig)
            : base(moveSelectorConfig)
        {
        }

        protected override MoveSelector BuildBaseMoveSelector(HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, bool randomSelection)
        {
            List<MoveSelector> moveSelectorList = BuildInnerMoveSelectors(cartConfig.GetMoveSelectorList(),
               configPolicy, minimumCacheType, randomSelection);
            bool ignoreEmptyChildIterators_ = cartConfig.GetIgnoreEmptyChildIterators() ?? true;
            return new CartesianProductMoveSelector(moveSelectorList, ignoreEmptyChildIterators_, randomSelection);
        }
    }
}
