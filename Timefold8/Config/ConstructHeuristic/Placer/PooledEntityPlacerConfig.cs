using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.ConstructHeuristic.Placer
{
    public class PooledEntityPlacerConfig : EntityPlacerConfig<PooledEntityPlacerConfig>, IAbstractEntityPlacerConfig
    {
        public PooledEntityPlacerConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public PooledEntityPlacerConfig Inherit(PooledEntityPlacerConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public IAbstractEntityPlacerConfig Inherit(IAbstractEntityPlacerConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        IAbstractEntityPlacerConfig AbstractConfig<IAbstractEntityPlacerConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }
    }
}
