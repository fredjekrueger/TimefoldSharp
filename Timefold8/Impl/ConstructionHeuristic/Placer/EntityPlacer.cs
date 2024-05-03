using TimefoldSharp.Core.Impl.Phase.Event;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public interface EntityPlacer : IEnumerable<Placement>, PhaseLifecycleListener
    {
    }
}
