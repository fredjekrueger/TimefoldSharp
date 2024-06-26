using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{
    public class TriTuple<A, B, C> : AbstractTuple
    {
        // Only a tuple's origin node may modify a fact.
        public A factA;
        public B factB;
        public C factC;

        public TriTuple(A factA, B factB, C factC, int storeSize) : base(storeSize)
        {
            this.factA = factA;
            this.factB = factB;
            this.factC = factC;
        }

        public bool UpdateIfDifferent(A newFactA, B newFactB, C newFactC)
        {
            bool different = false;
            if (!factA.Equals(newFactA))
            {
                factA = newFactA;
                different = true;
            }
            if (!factB.Equals(newFactB))
            {
                factB = newFactB;
                different = true;
            }
            if (!factC.Equals(newFactC))
            {
                factC = newFactC;
                different = true;
            }
            return different;
        }

        public override string ToString()
        {
            return "{" + factA + ", " + factB + ", " + factC + "}";
        }
    }
}
