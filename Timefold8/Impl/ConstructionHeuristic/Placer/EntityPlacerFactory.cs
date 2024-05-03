using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Heurisitic;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public interface EntityPlacerFactory
    {
        EntityPlacer BuildEntityPlacer(HeuristicConfigPolicy configPolicy);
    }

    public static class EntityPlacerFactoryHelper
    {
        public static EntityPlacerFactory Create(EntityPlacerConfig<IAbstractEntityPlacerConfig> entityPlacerConfig)
        {
            if (typeof(PooledEntityPlacerConfig).IsAssignableFrom(entityPlacerConfig.GetType()))
            {
                return new PooledEntityPlacerFactory((PooledEntityPlacerConfig)entityPlacerConfig);
            }
            else if (typeof(QueuedEntityPlacerConfig).IsAssignableFrom(entityPlacerConfig.GetType()))
            {
                return new QueuedEntityPlacerFactory((QueuedEntityPlacerConfig)entityPlacerConfig);
            }/*
            else if (typeof(QueuedValuePlacerConfig).IsAssignableFrom(entityPlacerConfig.GetType()))
            {
                return new QueuedValuePlacerFactory<>((QueuedValuePlacerConfig)entityPlacerConfig);
            }*/
            else
            {
                throw new Exception("unknown type");
            }
        }
    }
}

