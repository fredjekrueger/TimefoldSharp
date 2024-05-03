using TimefoldSharp.Core.Config.Solver;

namespace TimefoldSharp.Core.Impl.Solver.Recaller
{
    public class BestSolutionRecallerFactory
    {
        public static BestSolutionRecallerFactory Create()
        {
            return new BestSolutionRecallerFactory();
        }

        public BestSolutionRecaller BuildBestSolutionRecaller(EnvironmentMode environmentMode)
        {
            BestSolutionRecaller bestSolutionRecaller = new BestSolutionRecaller();
            if (EnvironmentModeEnumHelper.IsNonIntrusiveFullAsserted(environmentMode))
            {
                bestSolutionRecaller.AssertInitialScoreFromScratch = true;
                bestSolutionRecaller.AssertShadowVariablesAreNotStale = true;
                bestSolutionRecaller.AssertBestScoreIsUnmodified = true;
            }
            return bestSolutionRecaller;
        }
    }

}
