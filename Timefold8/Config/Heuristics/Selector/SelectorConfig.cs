using TimefoldSharp.Core.Config.Solver;

namespace TimefoldSharp.Core.Config.Heuristics.Selector
{
    public interface SelectorConfig<Config_> : AbstractConfig<Config_> where Config_ : SelectorConfig<Config_>
    {

    }
}
