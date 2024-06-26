using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Buildin.HardSoft;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.API.Score.Buildin.HardSoftLong
{
    public class HardSoftLongScore : Score
    {
        public static HardSoftLongScore ZERO { get; } = new HardSoftLongScore(0, 0, 0);
        public static HardSoftLongScore ONE_HARD { get; } = new HardSoftLongScore(0, 1, 0);
        public static HardSoftLongScore ONE_SOFT = new HardSoftLongScore(0, 0, 1);
        private static HardSoftLongScore MINUS_ONE_SOFT = new HardSoftLongScore(0, 0, -1);
        private static HardSoftLongScore MINUS_ONE_HARD = new HardSoftLongScore(0, -1, 0);

        private int initScore;
        private long hardScore;
        private long softScore;

        private HardSoftLongScore(int initScore, long hardScore, long softScore)
        {
            this.initScore = initScore;
            this.hardScore = hardScore;
            this.softScore = softScore;
        }

        public long HardScore()
        {
            return hardScore;
        }

        public long SoftScore()
        {
            return softScore;
        }

        public int CompareTo(Score o)
        {
            var other = (HardSoftLongScore)o;
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

        public int InitScore()
        {
            return initScore;
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

        public static string INIT_LABEL = "init";
        public static string HARD_LABEL = "hard";
        public static string SOFT_LABEL = "soft";

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

        public bool IsSolutionInitialized()
        {
            return InitScore() >= 0;
        }

        public Score Zero()
        {
            return ZERO;
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

        public static HardSoftLongScore Of(long hardScore, long softScore)
        {
            // Optimization for frequently seen values.
            if (hardScore == 0L)
            {
                if (softScore == -1L)
                {
                    return MINUS_ONE_SOFT;
                }
                else if (softScore == 0L)
                {
                    return ZERO;
                }
                else if (softScore == 1L)
                {
                    return ONE_SOFT;
                }
            }
            else if (softScore == 0L)
            {
                if (hardScore == 1L)
                {
                    return ONE_HARD;
                }
                else if (hardScore == -1L)
                {
                    return MINUS_ONE_HARD;
                }
            }
            // Every other case is constructed.
            return new HardSoftLongScore(0, hardScore, softScore);
        }

        public static HardSoftLongScore OfUninitialized(int initScore, long hardScore, long softScore)
        {
            if (initScore == 0)
            {
                return Of(hardScore, softScore);
            }
            return new HardSoftLongScore(initScore, hardScore, softScore);
        }

        public Score Subtract(Score subtrahend)
        {
            var other = (HardSoftLongScore)subtrahend;
            return OfUninitialized(
                initScore - other.InitScore(),
                hardScore - other.HardScore(),
                softScore - other.SoftScore());
        }

        public static HardSoftLongScore OfHard(long hardScore)
        {
            // Optimization for frequently seen values.
            if (hardScore == -1L)
            {
                return MINUS_ONE_HARD;
            }
            else if (hardScore == 0L)
            {
                return ZERO;
            }
            else if (hardScore == 1L)
            {
                return ONE_HARD;
            }
            // Every other case is constructed.
            return new HardSoftLongScore(0, hardScore, 0L);
        }

        public static HardSoftLongScore OfSoft(long softScore)
        {
            // Optimization for frequently seen values.
            if (softScore == -1L)
            {
                return MINUS_ONE_SOFT;
            }
            else if (softScore == 0L)
            {
                return ZERO;
            }
            else if (softScore == 1L)
            {
                return ONE_SOFT;
            }
            // Every other case is constructed.
            return new HardSoftLongScore(0, 0L, softScore);
        }

        public Score WithInitScore(int newInitScore)
        {
            throw new NotImplementedException();
        }

        public bool IsFeasible()
        {
            throw new NotImplementedException();
        }
    }
}
