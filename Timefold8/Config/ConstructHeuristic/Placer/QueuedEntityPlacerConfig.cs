using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.ConstructHeuristic.Placer
{
    public class QueuedEntityPlacerConfig : EntityPlacerConfig<QueuedEntityPlacerConfig>, IAbstractEntityPlacerConfig
    {
        protected EntitySelectorConfig entitySelectorConfig = null;
        protected List<AbstractMoveSelectorConfig> moveSelectorConfigList = null;

        public QueuedEntityPlacerConfig CopyConfig()
        {
            return new QueuedEntityPlacerConfig().Inherit(this);
        }

        public QueuedEntityPlacerConfig Inherit(QueuedEntityPlacerConfig inheritedConfig)
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

        public QueuedEntityPlacerConfig WithEntitySelectorConfig(EntitySelectorConfig entitySelectorConfig)
        {
            this.entitySelectorConfig = entitySelectorConfig;
            return this;
        }

        public void SetEntitySelectorConfig(EntitySelectorConfig entitySelectorConfig)
        {
            this.entitySelectorConfig = entitySelectorConfig;
        }

        IAbstractEntityPlacerConfig AbstractConfig<IAbstractEntityPlacerConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }

        public void SetMoveSelectorConfigList(List<AbstractMoveSelectorConfig> moveSelectorConfigList)
        {
            this.moveSelectorConfigList = moveSelectorConfigList;
        }

        public List<AbstractMoveSelectorConfig> GetMoveSelectorConfigList()
        {
            return moveSelectorConfigList;
        }

        public EntitySelectorConfig GetEntitySelectorConfig()
        {
            return entitySelectorConfig;
        }
    }
}
