using TimefoldSharp.Core.Config.Heuristics.Selector.Common;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move
{
    public interface MoveSelectorFactory
    {
        MoveSelector BuildMoveSelector(HeuristicConfigPolicy configPolicy,
            SelectionCacheType minimumCacheType, SelectionOrder inheritedSelectionOrder, bool skipNonDoableMoves);
    }
}
