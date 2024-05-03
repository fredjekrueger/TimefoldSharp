using TimefoldSharp.Core.Config.Heuristics.Selector.Move;
using TimefoldSharp.Core.Config.Heuristics.Selector.Value;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.ConstructHeuristic.Placer
{
    public class QueuedValuePlacerConfig : EntityPlacerConfig<QueuedValuePlacerConfig>, IAbstractEntityPlacerConfig
    {

        private MoveSelectorConfig<AbstractMoveSelectorConfig> moveSelectorConfig = null;
        protected ValueSelectorConfig valueSelectorConfig = null;

        public QueuedValuePlacerConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public QueuedValuePlacerConfig Inherit(QueuedValuePlacerConfig inheritedConfig)
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

        public QueuedValuePlacerConfig WithValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.SetValueSelectorConfig(valueSelectorConfig);
            return this;
        }

        public void SetValueSelectorConfig(ValueSelectorConfig valueSelectorConfig)
        {
            this.valueSelectorConfig = valueSelectorConfig;
        }

        public void SetMoveSelectorConfig(MoveSelectorConfig<AbstractMoveSelectorConfig> moveSelectorConfig)
        {
            this.moveSelectorConfig = moveSelectorConfig;
        }

        public QueuedValuePlacerConfig WithMoveSelectorConfig(MoveSelectorConfig<AbstractMoveSelectorConfig> moveSelectorConfig)
        {
            this.SetMoveSelectorConfig(moveSelectorConfig);
            return this;
        }

        IAbstractEntityPlacerConfig AbstractConfig<IAbstractEntityPlacerConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }
    }
}
