using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite
{
    public class UnionMoveSelectorConfig : MoveSelectorConfig<UnionMoveSelectorConfig>, AbstractMoveSelectorConfig
    {
        public MoveSelectorConfigImpl MoveSelectorConfigImpl { get; set; } = new MoveSelectorConfigImpl();

        protected Type selectorProbabilityWeightFactoryClass;

        private double? fixedProbabilityWeight = null;

        public UnionMoveSelectorConfig()
        {
        }

        public UnionMoveSelectorConfig(List<AbstractMoveSelectorConfig> moveSelectorConfigList)
        {
            this.moveSelectorConfigList = moveSelectorConfigList;
        }

        public UnionMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public UnionMoveSelectorConfig Inherit(UnionMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig Inherit(AbstractMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        private List<AbstractMoveSelectorConfig> moveSelectorConfigList = null;

        public UnionMoveSelectorConfig WithMoveSelectors(List<AbstractMoveSelectorConfig> moveSelectorConfigs)
        {
            this.moveSelectorConfigList = moveSelectorConfigs;
            return this;
        }

        AbstractMoveSelectorConfig AbstractConfig<AbstractMoveSelectorConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }

        public List<AbstractMoveSelectorConfig> GetMoveSelectorList()
        {
            return moveSelectorConfigList;
        }

        internal Type GetSelectorProbabilityWeightFactoryClass()
        {
            return selectorProbabilityWeightFactoryClass;
        }
    }
}
