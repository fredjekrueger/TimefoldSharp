using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    internal class SimpleScoreInliner : AbstractScoreInliner
    {
        public SimpleScoreInliner(Dictionary<Constraint, API.Score.Score> constraintWeightMap, bool constraintMatchEnabled)
            : base(constraintWeightMap, constraintMatchEnabled)
        {

        }

        public override IWeightedScoreImpacter BuildWeightedScoreImpacter(AbstractConstraint constraint)
        {
            throw new NotImplementedException();
        }

        public override API.Score.Score ExtractScore(int initScore)
        {
            throw new NotImplementedException();
        }
    }
}
