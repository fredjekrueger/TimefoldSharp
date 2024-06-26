using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Uni;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public class DefaultUniConstraintCollector<A, ResultContainer_, Result_> : UniConstraintCollector<A, ResultContainer_, Result_>
    {
        private Func<ResultContainer_> supplier;
        private Func<ResultContainer_, A, Action> accumulator;
        private Func<ResultContainer_, Result_> finisher;

        public DefaultUniConstraintCollector(Func<ResultContainer_> supplier, Func<ResultContainer_, A, Action> accumulator, Func<ResultContainer_, Result_> finisher)
        {
            this.supplier = supplier;
            this.accumulator = accumulator;
            this.finisher = finisher;
        }

        public Func<ResultContainer_, A, Action> Accumulator()
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
