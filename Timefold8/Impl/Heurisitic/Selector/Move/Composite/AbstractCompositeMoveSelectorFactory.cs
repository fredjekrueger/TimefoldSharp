using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public abstract class AbstractCompositeMoveSelectorFactory<MoveSelectorConfig_>
        : AbstractMoveSelectorFactory<AbstractMoveSelectorConfig>
    {
        public AbstractCompositeMoveSelectorFactory(AbstractMoveSelectorConfig moveSelectorConfig)
            : base(moveSelectorConfig)
        {
        }

        protected List<MoveSelector> BuildInnerMoveSelectors(List<AbstractMoveSelectorConfig> innerMoveSelectorList,
           HeuristicConfigPolicy configPolicy, SelectionCacheType minimumCacheType, bool randomSelection)
        {
            return innerMoveSelectorList.Select(moveSelectorConfig =>
            {
                AbstractMoveSelectorFactory<AbstractMoveSelectorConfig> moveSelectorFactory = AbstractMoveSelectorFactory<AbstractMoveSelectorConfig>.Create(moveSelectorConfig);
                SelectionOrder selectionOrder = SelectionOrderHelper.FromRandomSelectionBoolean(randomSelection);
                return moveSelectorFactory.BuildMoveSelector(configPolicy, minimumCacheType, selectionOrder, false);
            }).ToList();
        }
    }
}
