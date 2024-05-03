using TimefoldSharp.Core.Config.Phase;
using TimefoldSharp.Core.Config.Solver.Termination;
using TimefoldSharp.Core.Impl.Heurisitic;
using TimefoldSharp.Core.Impl.Solver.Termination;

namespace TimefoldSharp.Core.Impl.Phase
{
    public abstract class AbstractPhaseFactory<PhaseConfig_> : PhaseFactory
        where PhaseConfig_ : PhaseConfig<PhaseConfig_>
    {

        protected readonly PhaseConfig_ phaseConfig;

        public AbstractPhaseFactory(PhaseConfig_ phaseConfig)
        {
            this.phaseConfig = phaseConfig;
        }

        protected Termination BuildPhaseTermination(HeuristicConfigPolicy configPolicy, Termination solverTermination)
        {
            TerminationConfig terminationConfig_ = phaseConfig.GetTerminationConfig() ?? new TerminationConfig();
            Termination phaseTermination = new PhaseToSolverTerminationBridge(solverTermination);
            return TerminationFactory.Create(terminationConfig_).BuildTermination(configPolicy, phaseTermination);
        }
    }
}
