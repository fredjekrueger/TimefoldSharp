using TimefoldSharp.Core.Config.Solver.Random;

namespace TimefoldSharp.Core.Impl.Solver.Random
{
    public class DefaultRandomFactory : RandomFactory
    {
        protected readonly RandomType randomType;
        protected readonly long? randomSeed;

        public System.Random CreateRandom()
        {
            switch (randomType)
            {
                case RandomType.JDK:
                    return randomSeed == null ? new System.Random() : new System.Random((int)randomSeed.Value);
                /*case RandomType.MERSENNE_TWISTER:
                    return new RandomAdaptor(randomSeed == null ? new MersenneTwister() : new MersenneTwister(randomSeed));
                case RandomType.WELL512A:
                    return new RandomAdaptor(randomSeed == null ? new Well512a() : new Well512a(randomSeed));
                case RandomType.WELL1024A:
                    return new RandomAdaptor(randomSeed == null ? new Well1024a() : new Well1024a(randomSeed));
                case RandomType.WELL19937A:
                    return new RandomAdaptor(randomSeed == null ? new Well19937a() : new Well19937a(randomSeed));
                case RandomType.WELL19937C:
                    return new RandomAdaptor(randomSeed == null ? new Well19937c() : new Well19937c(randomSeed));
                case RandomType.WELL44497A:
                    return new RandomAdaptor(randomSeed == null ? new Well44497a() : new Well44497a(randomSeed));
                case RandomType.WELL44497B:
                    return new RandomAdaptor(randomSeed == null ? new Well44497b() : new Well44497b(randomSeed));*/
                default:
                    throw new Exception("The randomType (" + randomType + ") is not implemented.");
            }
        }

        public DefaultRandomFactory(RandomType randomType, long? randomSeed)
        {
            this.randomType = randomType;
            this.randomSeed = randomSeed;
        }
    }
}
