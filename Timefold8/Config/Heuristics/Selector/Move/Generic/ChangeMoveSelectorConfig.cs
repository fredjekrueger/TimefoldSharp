using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common.Decorator;
using TimefoldSharp.Core.Config.Heuristics.Selector.Entity;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic
{
    public class ChangeMoveSelectorConfig : MoveSelectorConfig<ChangeMoveSelectorConfig>, AbstractMoveSelectorConfig
    {

        private EntitySelectorConfig entitySelectorConfig = null;
        private ValueSelectorConfig valueSelectorConfig = null;

        public MoveSelectorConfigImpl MoveSelectorConfigImpl { get; set; } = new MoveSelectorConfigImpl();

        public void SetEntitySelectorConfig(EntitySelectorConfig entitySelectorConfig)
        {
            this.entitySelectorConfig = entitySelectorConfig;
        }

        public EntitySelectorConfig GetEntitySelectorConfig()
        {
            return entitySelectorConfig;
        }

        public ChangeMoveSelectorConfig WithValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.SetValueSelectorConfig(valueSelectorConfig);
            return this;
        }

        public void SetValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.valueSelectorConfig = valueSelectorConfig;
        }

        public ValueSelectorConfig GetValueSelectorConfig()
        {
            return valueSelectorConfig;
        }

        public AbstractMoveSelectorConfig Inherit(AbstractMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public ChangeMoveSelectorConfig Inherit(ChangeMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        ChangeMoveSelectorConfig AbstractConfig<ChangeMoveSelectorConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }
    }
}
