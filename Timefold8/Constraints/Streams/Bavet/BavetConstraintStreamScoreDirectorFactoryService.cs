using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Config.Score.Director;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet
{
    public sealed class BavetConstraintStreamScoreDirectorFactoryService
       : AbstractConstraintStreamScoreDirectorFactoryService
    {
        public override Func<AbstractScoreDirectorFactory> BuildScoreDirectorFactory(SolutionDescriptor solutionDescriptor, ScoreDirectorFactoryConfig config, EnvironmentMode environmentMode)
        {
            ConstraintStreamImplType constraintStreamImplType_ = config.ConstraintStreamImplType ?? ConstraintStreamImplType.BAVET;
            if (constraintStreamImplType_ != ConstraintStreamImplType.BAVET)
            {
                return null;
            }


            return new Func<AbstractScoreDirectorFactory>(() =>
            {
                ConstraintProvider constraintProvider = ConfigUtils.NewInstance<ConstraintProvider>(config.ConstraintProviderClass);
                ConfigUtils.ApplyCustomProperties(constraintProvider, "constraintProviderClass", config.ConstraintProviderCustomProperties, "constraintProviderCustomProperties");
                return BuildScoreDirectorFactory(solutionDescriptor, constraintProvider, environmentMode);
            });

        }

        public AbstractConstraintStreamScoreDirectorFactory BuildScoreDirectorFactory(SolutionDescriptor solutionDescriptor, ConstraintProvider constraintProvider, EnvironmentMode environmentMode)
        {
            return new BavetConstraintStreamScoreDirectorFactory(solutionDescriptor, constraintProvider, environmentMode);
        }


        public override ScoreDirectorType GetSupportedScoreDirectorType()
        {
            return ScoreDirectorType.CONSTRAINT_STREAMS;
        }
    }
}
