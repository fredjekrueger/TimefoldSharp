using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic
{
    public class SwapMoveSelectorConfig : MoveSelectorConfig<SwapMoveSelectorConfig>, AbstractMoveSelectorConfig
    {

        private EntitySelectorConfig entitySelectorConfig = null;
        private ValueSelectorConfig valueSelectorConfig = null;

        private EntitySelectorConfig secondaryEntitySelectorConfig = null;
        private List<string> variableNameIncludeList = null;

        public MoveSelectorConfigImpl MoveSelectorConfigImpl { get; set; } = new MoveSelectorConfigImpl();

        public SwapMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public SwapMoveSelectorConfig Inherit(SwapMoveSelectorConfig inheritedConfig)
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

        AbstractMoveSelectorConfig AbstractConfig<AbstractMoveSelectorConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }

        public EntitySelectorConfig GetEntitySelectorConfig()
        {
            return entitySelectorConfig;
        }

        public EntitySelectorConfig GetSecondaryEntitySelectorConfig()
        {
            return secondaryEntitySelectorConfig;
        }

        public void SetEntitySelectorConfig(EntitySelectorConfig entitySelectorConfig)
        {
            this.entitySelectorConfig = entitySelectorConfig;
        }

        public void SetSecondaryEntitySelectorConfig(EntitySelectorConfig secondaryEntitySelectorConfig)
        {
            this.secondaryEntitySelectorConfig = secondaryEntitySelectorConfig;
        }

        public void SetVariableNameIncludeList(List<string> variableNameIncludeList)
        {
            this.variableNameIncludeList = variableNameIncludeList;
        }

        public List<string> GetVariableNameIncludeList()
        {
            return variableNameIncludeList;
        }
    }
}
