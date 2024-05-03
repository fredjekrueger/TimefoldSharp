using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet
{
    public sealed class BavetConstraintStreamScoreDirector :
        AbstractScoreDirector<BavetConstraintStreamScoreDirectorFactory>
    {
        public BavetConstraintStreamScoreDirector(BavetConstraintStreamScoreDirectorFactory scoreDirectorFactory,
            bool lookUpEnabled, bool constraintMatchEnabledPreference, bool expectShadowVariablesInCorrectState)
            : base(scoreDirectorFactory, lookUpEnabled, constraintMatchEnabledPreference, expectShadowVariablesInCorrectState)
        {

        }

        public override void AfterVariableChanged(VariableDescriptor variableDescriptor, object entity)
        {
            session.Update(entity);
            base.AfterVariableChanged(variableDescriptor, entity);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.session = null;
        }

        public override void SetWorkingSolution(ISolution workingSolution)
        {
            base.SetWorkingSolution(workingSolution);
            ResetConstraintStreamingSession();
        }

        private void ResetConstraintStreamingSession()
        {
            session = scoreDirectorFactory.NewSession(constraintMatchEnabledPreference, workingSolution);
            GetSolutionDescriptor().VisitAll(workingSolution, (o) => session.Insert(o));
        }

        BavetConstraintSession session;

        public override Score CalculateScore()
        {
            Score score = session.CalculateScore(workingInitScore.Value);
            SetCalculatedScore(score);
            return score;
        }

    }
}
