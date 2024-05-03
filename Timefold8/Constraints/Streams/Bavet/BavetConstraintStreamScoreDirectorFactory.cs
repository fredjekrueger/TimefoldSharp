using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet
{
    public sealed class BavetConstraintStreamScoreDirectorFactory : AbstractConstraintStreamScoreDirectorFactory
    {

        private readonly List<BavetConstraint> constraintList;
        private readonly BavetConstraintSessionFactory constraintSessionFactory;

        public BavetConstraintStreamScoreDirectorFactory(SolutionDescriptor solutionDescriptor, ConstraintProvider constraintProvider, EnvironmentMode environmentMode) : this(solutionDescriptor)
        {
            BavetConstraintFactory constraintFactory = new BavetConstraintFactory(solutionDescriptor, environmentMode);
            this.constraintList = constraintFactory.BuildConstraints(constraintProvider);
            this.constraintSessionFactory = new BavetConstraintSessionFactory(solutionDescriptor, this.constraintList);
        }

        protected BavetConstraintStreamScoreDirectorFactory(SolutionDescriptor solutionDescriptor) : base(solutionDescriptor)
        {
        }

        public BavetConstraintSession NewSession(bool constraintMatchEnabled, ISolution workingSolution)
        {
            return constraintSessionFactory.BuildSession(constraintMatchEnabled, workingSolution);
        }

        public override InnerScoreDirector BuildScoreDirector(bool lookUpEnabled, bool constraintMatchEnabledPreference, bool expectShadowVariablesInCorrectState)
        {
            return new BavetConstraintStreamScoreDirector(this, lookUpEnabled, constraintMatchEnabledPreference, expectShadowVariablesInCorrectState);
        }


    }
}
