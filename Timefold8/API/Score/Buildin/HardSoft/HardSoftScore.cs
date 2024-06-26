using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.API.Score.Buildin.HardSoft
{
    public sealed class HardSoftScore : Score
    {
        public static HardSoftScore ZERO { get; } = new HardSoftScore(0, 0, 0);
        public static HardSoftScore ONE_HARD { get; } = new HardSoftScore(0, 1, 0);
        public static HardSoftScore ONE_SOFT = new HardSoftScore(0, 0, 1);
        private static HardSoftScore MINUS_ONE_SOFT = new HardSoftScore(0, 0, -1);
        private static HardSoftScore MINUS_ONE_HARD = new HardSoftScore(0, -1, 0);

        public static readonly String HARD_LABEL = "hard";
        public static readonly String SOFT_LABEL = "soft";
        public static readonly String INIT_LABEL = "init";

        private readonly int initScore;
        private readonly int hardScore;
        private readonly int softScore;

        private HardSoftScore(int initScore, int hardScore, int softScore)
        {
            this.initScore = initScore;
            this.hardScore = hardScore;
            this.softScore = softScore;
        }

        public int CompareTo(HardSoftScore other)
        {
            throw new NotImplementedException();
        }

        public bool IsSolutionInitialized()
        {
            return initScore >= 0;
        }

        public int InitScore()
        {
            return initScore;
        }

        public int GetInitScore()
        {
            return initScore;
        }

        public int CompareTo(Score o)
        {
            var other = (HardSoftScore)o;
            if (initScore != other.InitScore())
            {
                return initScore.CompareTo(other.InitScore());
            }
            else if (hardScore != other.HardScore())
            {
                return hardScore.CompareTo(other.HardScore());
            }
            else
            {
                return softScore.CompareTo(other.SoftScore());
            }
        }

        public Score Zero()
        {
            return ZERO;
        }

        public Score Subtract(HardSoftScore subtrahend)
        {
            throw new NotImplementedException();
        }

        public Score Negate()
        {
            Score zero = Zero();
            Score current = this;
            if (zero.Equals(current))
            {
                return current;
            }
            return zero.Subtract(current);
        }

        public int HardScore()
        {
            return hardScore;
        }

        public int SoftScore()
        {
            return softScore;
        }

        public Score Subtract(Score subtrahend)
        {
            var s = (HardSoftScore)subtrahend;
            return OfUninitialized(
              initScore - s.InitScore(),
              hardScore - s.HardScore(),
              softScore - s.SoftScore());
        }

        public static HardSoftScore Of(int hardScore, int softScore)
        {
            // Optimization for frequently seen values.
            if (hardScore == 0)
            {
                if (softScore == -1)
                {
                    return MINUS_ONE_SOFT;
                }
                else if (softScore == 0)
                {
                    return ZERO;
                }
                else if (softScore == 1)
                {
                    return ONE_SOFT;
                }
            }
            else if (softScore == 0)
            {
                if (hardScore == 1)
                {
                    return ONE_HARD;
                }
                else if (hardScore == -1)
                {
                    return MINUS_ONE_HARD;
                }
            }
            // Every other case is constructed.
            return new HardSoftScore(0, hardScore, softScore);
        }

        public static HardSoftScore OfUninitialized(int initScore, int hardScore, int softScore)
        {
            if (initScore == 0)
            {
                return Of(hardScore, softScore);
            }
            return new HardSoftScore(initScore, hardScore, softScore);
        }

        public static HardSoftScore OfHard(int hardScore)
        {
            // Optimization for frequently seen values.
            if (hardScore == -1)
            {
                return MINUS_ONE_HARD;
            }
            else if (hardScore == 0)
            {
                return ZERO;
            }
            else if (hardScore == 1)
            {
                return ONE_HARD;
            }
            // Every other case is constructed.
            return new HardSoftScore(0, hardScore, 0);
        }

        public static HardSoftScore OfSoft(int softScore)
        {
            // Optimization for frequently seen values.
            if (softScore == -1)
            {
                return MINUS_ONE_SOFT;
            }
            else if (softScore == 0)
            {
                return ZERO;
            }
            else if (softScore == 1)
            {
                return ONE_SOFT;
            }
            // Every other case is constructed.
            return new HardSoftScore(0, 0, softScore);
        }

        public Score WithInitScore(int newInitScore)
        {
            throw new NotImplementedException();
        }

        public bool IsFeasible()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            else if (o is HardSoftScore other)
            {
                return initScore == other.InitScore()
                        && hardScore == other.HardScore()
                        && softScore == other.SoftScore();
            }
            else
            {
                return false;
            }
        }

        string GetInitPrefix(int initScore)
        {
            if (initScore == 0)
            {
                return "";
            }
            return initScore + INIT_LABEL + "/";
        }

        public override string ToString()
        {
            return GetInitPrefix(initScore) + hardScore + HARD_LABEL + "/" + softScore + SOFT_LABEL;
        }

        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(initScore, hardScore, softScore);
        }
    }
}
