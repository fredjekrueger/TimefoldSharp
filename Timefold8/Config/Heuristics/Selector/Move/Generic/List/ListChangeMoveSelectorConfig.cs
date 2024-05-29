using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Generic.List
{
    public class ListChangeMoveSelectorConfig : MoveSelectorConfig<AbstractMoveSelectorConfig>
    {
        public MoveSelectorConfigImpl MoveSelectorConfigImpl { get; set; } = new MoveSelectorConfigImpl();

        private ValueSelectorConfig valueSelectorConfig = null;

        public ListChangeMoveSelectorConfig Inherit(ListChangeMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public ListChangeMoveSelectorConfig WithValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.SetValueSelectorConfig(valueSelectorConfig);
            return this;
        }

        public void SetValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.valueSelectorConfig = valueSelectorConfig;
        }

        public AbstractMoveSelectorConfig Inherit(AbstractMoveSelectorConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public AbstractMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }
    }
}
