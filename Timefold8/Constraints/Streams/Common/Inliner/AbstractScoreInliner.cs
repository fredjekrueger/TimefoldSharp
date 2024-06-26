using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Score.Buidin;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Inliner
{
    public abstract class AbstractScoreInliner
    {
        public abstract Score ExtractScore(int initScore);
        protected readonly bool constraintMatchEnabled;
        protected Dictionary<Constraint, Score> constraintWeightMap;

        public abstract IWeightedScoreImpacter BuildWeightedScoreImpacter(AbstractConstraint constraint);

        protected AbstractScoreInliner(Dictionary<Constraint, Score> constraintWeightMap, bool constraintMatchEnabled)
        {
            this.constraintMatchEnabled = constraintMatchEnabled;
            this.constraintWeightMap = constraintWeightMap;
        }

        public bool IsConstraintMatchEnabled()
        {
            return constraintMatchEnabled;
        }


        public static ScoreInliner_ BuildScoreInliner<ScoreInliner_>(ScoreDefinition scoreDefinition, Dictionary<Constraint, Score> constraintWeightMap, bool constraintMatchEnabled)
            where ScoreInliner_ : AbstractScoreInliner
        {
            if (scoreDefinition is SimpleScoreDefinition)
            {
                return new SimpleScoreInliner(constraintWeightMap.ToDictionary(k => k.Key, v => v.Value), constraintMatchEnabled) as ScoreInliner_;
            }
            else if (scoreDefinition is HardSoftScoreDefinition)
            {
                return new HardSoftScoreInliner(constraintWeightMap.ToDictionary(k => k.Key, v => v.Value), constraintMatchEnabled) as ScoreInliner_;
            }
            else if (scoreDefinition is HardSoftLongScoreDefinition)
            {
                return new HardSoftLongScoreInliner(constraintWeightMap.ToDictionary(k => k.Key, v => v.Value), constraintMatchEnabled) as ScoreInliner_;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public class ConstraintMatchCarrier
        {

        }
    }
}
