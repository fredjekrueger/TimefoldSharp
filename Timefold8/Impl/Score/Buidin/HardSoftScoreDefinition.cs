using TimefoldSharp.Core.API.Score.Buildin.HardSoft;
using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Score.Definition;

namespace TimefoldSharp.Core.Impl.Score.Buidin
{
    public class HardSoftScoreDefinition : AbstractScoreDefinition, ScoreDefinition
    {
        public HardSoftScoreDefinition() : base(new String[] { "hard score", "soft score" })
        {

        }

        public int CompareTo(API.Score.Score other)
        {
            throw new NotImplementedException();
        }

        public override API.Score.Score GetOneSoftestScore()
        {
            throw new NotImplementedException();
        }

        public override API.Score.Score GetZeroScore()
        {
            return HardSoftScore.ZERO;
        }

        public override bool IsNegativeOrZero(API.Score.Score score)
        {
            throw new NotImplementedException();
        }

        public override API.Score.Score ParseScore(string scoreString)
        {
            throw new NotImplementedException();
        }
    }
}
