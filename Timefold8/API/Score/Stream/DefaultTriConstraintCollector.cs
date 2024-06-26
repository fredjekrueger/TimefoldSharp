using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Tri;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public class DefaultTriConstraintCollector<A, B, C, ResultContainer_, Result_> : TriConstraintCollector<A, B, C, ResultContainer_, Result_>
    {

        private Func<ResultContainer_> supplier;
        private Func<ResultContainer_, A, B, C, Action> accumulator;
        private Func<ResultContainer_, Result_> finisher;

        public DefaultTriConstraintCollector(Func<ResultContainer_> supplier, Func<ResultContainer_, A, B, C, Action> accumulator, Func<ResultContainer_, Result_> finisher)
        {
            this.supplier = supplier;
            this.accumulator = accumulator;
            this.finisher = finisher;
        }

        public Func<ResultContainer_, A, B, C, Action> Accumulator()
        {
            return accumulator;
        }

        public Func<ResultContainer_, Result_> Finisher()
        {
            return finisher;
        }

        public Func<ResultContainer_> Supplier()
        {
            return supplier;
        }
    }
}
