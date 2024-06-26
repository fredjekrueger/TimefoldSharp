using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Impl.Score.Definition;
using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.API.Score.Buildin.HardSoftLong;

namespace TimefoldSharp.Core.Impl.Score.Buidin
{
    public class HardSoftLongScoreDefinition : AbstractScoreDefinition, ScoreDefinition
    {
        public HardSoftLongScoreDefinition() : base(new string[] { "hard score", "soft score" })
        {
            
        }

        public override API.Score.Score GetOneSoftestScore()
        {
            return HardSoftLongScore.ONE_SOFT;
        }

        public override API.Score.Score GetZeroScore()
        {
            return HardSoftLongScore.ZERO;
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
