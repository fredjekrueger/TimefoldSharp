using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index;

namespace TimefoldSharp.Core.Impl.Solver.Random
{
    public  class RandomUtils
    {
        public static long NextLong(System.Random random, long n)
        {
            // This code is based on java.util.Random#nextInt(int)'s javadoc.
            if (n <= 0L)
            {
                throw new Exception("n must be positive");
            }
            if (n < int.MaxValue)
            {
                return random.Next((int)n);
            }

            long bits;
            long val;
            do
            {
                bits = (random.NextInt64() << 1) >>> 1;
                val = bits % n;
            } while (bits - val + (n - 1L) < 0L);
            return val;
        }
    }
}
