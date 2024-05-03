using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Phase.Event;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector
{
    public interface Selector : PhaseLifecycleListener
    {
        SelectionCacheType GetCacheType();
        bool IsNeverEnding();
        bool IsCountable();
    }
}
