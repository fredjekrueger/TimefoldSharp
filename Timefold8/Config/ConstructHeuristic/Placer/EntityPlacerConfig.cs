using TimefoldSharp.Core.Config.Solver;

namespace TimefoldSharp.Core.Config.ConstructHeuristic.Placer
{


    public interface EntityPlacerConfig<Config_> : AbstractConfig<Config_> where Config_ : EntityPlacerConfig<Config_>
    {
    }
}
