using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Impl.Solver;

namespace TimefoldSharp.Core.API.Solver
{
    public abstract class SolverFactory
    {

        /*static SolverFactory<> CreateFromXmlResource(string solverConfigResource)
        {
            SolverConfig solverConfig = SolverConfig.CreateFromXmlResource(solverConfigResource);
            return new DefaultSolverFactory<_>(solverConfig);
        }

        static SolverFactory<> CreateFromXmlResource(string solverConfigResource, ClassLoader classLoader)
        {
            SolverConfig solverConfig = SolverConfig.CreateFromXmlResource(solverConfigResource, classLoader);
            return new DefaultSolverFactory<_>(solverConfig);
        }

        static SolverFactory<> CreateFromXmlFile(File solverConfigFile)
        {
            SolverConfig solverConfig = SolverConfig.CreateFromXmlFile(solverConfigFile);
            return new DefaultSolverFactory<>(solverConfig);
        }

        static SolverFactory<> CreateFromXmlFile(File solverConfigFile, ClassLoader classLoader)
        {
            SolverConfig solverConfig = SolverConfig.CreateFromXmlFile(solverConfigFile, classLoader);
            return new DefaultSolverFactory<>(solverConfig);
        }*/

        public static SolverFactory Create(SolverConfig solverConfig)
        {
            solverConfig = new SolverConfig(solverConfig);
            return new DefaultSolverFactory(solverConfig);
        }

        public abstract Solver BuildSolver();
    }
}