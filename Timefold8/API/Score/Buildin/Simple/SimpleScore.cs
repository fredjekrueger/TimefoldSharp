namespace TimefoldSharp.Core.API.Score.Buildin.Simple
{
    public sealed class SimpleScore : Score
    {

        private readonly int initScore;

        public int CompareTo(SimpleScore other)
        {
            throw new NotImplementedException();
        }

        public bool IsSolutionInitialized()
        {
            return initScore >= 0;
        }

        public int GetInitScore()
        {
            return initScore;
        }

        public int InitScore()
        {
            return initScore;
        }



        public int CompareTo(Score other)
        {
            throw new NotImplementedException();
        }

        public SimpleScore Zero()
        {
            throw new NotImplementedException();
        }

        public SimpleScore Negate()
        {
            throw new NotImplementedException();
        }

        public SimpleScore Subtract(SimpleScore subtrahend)
        {
            throw new NotImplementedException();
        }

        Score Score.Zero()
        {
            throw new NotImplementedException();
        }

        Score Score.Negate()
        {
            throw new NotImplementedException();
        }

        public Score Subtract(Score subtrahend)
        {
            throw new NotImplementedException();
        }

        public Score WithInitScore(int newInitScore)
        {
            throw new NotImplementedException();
        }

        public bool IsFeasible()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
