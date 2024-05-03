using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Config.Heuristics.Selector.Move;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Heurisitic;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public class QueuedValuePlacerFactory : AbstractEntityPlacerFactory<QueuedValuePlacerConfig>
    {
        public QueuedValuePlacerFactory(QueuedValuePlacerConfig placerConfig) : base(placerConfig)
        {
        }

        public static QueuedValuePlacerConfig UnfoldNew(MoveSelectorConfig<AbstractMoveSelectorConfig> templateMoveSelectorConfig)
        {
            throw new Exception("The <constructionHeuristic> contains a moveSelector ("
                    + templateMoveSelectorConfig + ") and the <queuedValuePlacer> does not support unfolding those yet.");
        }

        public override EntityPlacer BuildEntityPlacer(HeuristicConfigPolicy configPolicy)
        {
            throw new NotImplementedException();
        }
    }
}
