using TimefoldSharp.Core.Config.Solver;

namespace TimefoldSharp.Core.Config.ConstructHeuristic.Decider.Forager
{
    public class ConstructionHeuristicForagerConfig : AbstractConfig<ConstructionHeuristicForagerConfig>
    {
        private ConstructionHeuristicPickEarlyType? pickEarlyType = null;

        public ConstructionHeuristicForagerConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public ConstructionHeuristicForagerConfig Inherit(ConstructionHeuristicForagerConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public ConstructionHeuristicPickEarlyType? GetPickEarlyType()
        {
            return pickEarlyType;
        }
    }
}
