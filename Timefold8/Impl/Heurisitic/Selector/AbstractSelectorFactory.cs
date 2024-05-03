using TimefoldSharp.Core.Config.Heuristics.Selector;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector
{
    public abstract class AbstractSelectorFactory<SelectorConfig_>
        : AbstractFromConfigFactory<SelectorConfig_>
        where SelectorConfig_ : SelectorConfig<SelectorConfig_>
    {

        protected AbstractSelectorFactory(SelectorConfig_ selectorConfig)
            : base(selectorConfig)
        {

        }
    }
}
