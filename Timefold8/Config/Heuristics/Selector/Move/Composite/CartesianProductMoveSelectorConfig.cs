using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Heuristics.Selector.Move.Composite
{
    public class CartesianProductMoveSelectorConfig : MoveSelectorConfig<CartesianProductMoveSelectorConfig>, AbstractMoveSelectorConfig
    {
        private bool? ignoreEmptyChildIterators = null;

        public MoveSelectorConfigImpl MoveSelectorConfigImpl { get; set; } = new MoveSelectorConfigImpl();

        private List<AbstractMoveSelectorConfig> moveSelectorConfigList = null;

        public CartesianProductMoveSelectorConfig(List<AbstractMoveSelectorConfig> moveSelectorConfigList)
        {
            this.moveSelectorConfigList = moveSelectorConfigList;
        }

        public CartesianProductMoveSelectorConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public CartesianProductMoveSelectorConfig Inherit(CartesianProductMoveSelectorConfig inheritedConfig)
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

        public List<AbstractMoveSelectorConfig> GetMoveSelectorList()
        {
            return moveSelectorConfigList;
        }

        AbstractMoveSelectorConfig AbstractConfig<AbstractMoveSelectorConfig>.CopyConfig()
        {
            throw new NotImplementedException();
        }

        public bool? GetIgnoreEmptyChildIterators()
        {
            return ignoreEmptyChildIterators;
        }
    }
}
