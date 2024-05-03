using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Score.Definition;

namespace TimefoldSharp.Core.Impl.Score.Buidin
{
    public class SimpleScoreDefinition : AbstractScoreDefinition, ScoreDefinition
    {
        public SimpleScoreDefinition() : base(new String[] { "score" })
        {
        }

        public override API.Score.Score GetOneSoftestScore()
        {
            throw new NotImplementedException();
        }


        public override API.Score.Score GetZeroScore()
        {
            throw new NotImplementedException();
        }

        public override bool IsNegativeOrZero(API.Score.Score score)
        {
            throw new NotImplementedException();
        }

        public override API.Score.Score ParseScore(string scoreString)
        {
            throw new NotImplementedException();
        }

        API.Score.Score ScoreDefinition.GetOneSoftestScore()
        {
            throw new NotImplementedException();
        }

        API.Score.Score ScoreDefinition.GetZeroScore()
        {
            throw new NotImplementedException();
        }

        API.Score.Score ScoreDefinition.ParseScore(string scoreString)
        {
            throw new NotImplementedException();
        }
    }
}
