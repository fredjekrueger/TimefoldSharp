using TimefoldSharp.Core.Config.ConstructHeuristic.Decider.Forager;
using TimefoldSharp.Core.Config.ConstructHeuristic.Placer;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.ConstructHeuristic
{
    public class ConstructionHeuristicPhaseConfig : AbstractPhaseConfig
    {
        public IAbstractEntityPlacerConfig EntityPlacerConfig { get; set; }
        protected ConstructionHeuristicType? constructionHeuristicType;
        protected EntitySorterManner? entitySorterManner;
        protected ValueSorterManner? valueSorterManner;
        //protected EntityPlacerConfig<IAbstractEntityPlacerConfig> entityPlacerConfig = null;
        protected List<AbstractMoveSelectorConfig> moveSelectorConfigList = null;
        protected ConstructionHeuristicForagerConfig foragerConfig = null;

        public override AbstractPhaseConfig CopyConfig()
        {
            return new ConstructionHeuristicPhaseConfig().Inherit(this);
        }

        public ValueSorterManner? GetValueSorterManner()
        {
            return valueSorterManner;
        }

        public ConstructionHeuristicType? GetConstructionHeuristicType()
        {
            return constructionHeuristicType;
        }

        public List<AbstractMoveSelectorConfig> GetMoveSelectorConfigList()
        {
            return moveSelectorConfigList;
        }

        public EntityPlacerConfig<IAbstractEntityPlacerConfig> GetEntityPlacerConfig()
        {
            return EntityPlacerConfig;
        }

        public override void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public EntitySorterManner? GetEntitySorterManner()
        {
            return entitySorterManner;
        }

        public ConstructionHeuristicForagerConfig GetForagerConfig()
        {
            return foragerConfig;
        }
    }
}
