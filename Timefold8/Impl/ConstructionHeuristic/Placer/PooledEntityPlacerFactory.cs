using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Heurisitic;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public class PooledEntityPlacerFactory
        : AbstractEntityPlacerFactory<PooledEntityPlacerConfig>
    {
        public PooledEntityPlacerFactory(PooledEntityPlacerConfig placerConfig)
            : base(placerConfig)
        {

        }

        public static PooledEntityPlacerConfig UnfoldNew(HeuristicConfigPolicy configPolicy, MoveSelectorConfig<AbstractMoveSelectorConfig> templateMoveSelectorConfig)
        {
            throw new NotImplementedException();
        }

        public override EntityPlacer BuildEntityPlacer(HeuristicConfigPolicy configPolicy)
        {
            throw new NotImplementedException();
        }
    }
}
