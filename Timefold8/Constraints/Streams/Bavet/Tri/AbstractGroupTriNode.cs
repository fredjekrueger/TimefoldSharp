using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Tri;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Uni;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public abstract class AbstractGroupTriNode<OldA, OldB, OldC, OutTuple_ , GroupKey_, ResultContainer_, Result_>
        : AbstractGroupNode<TriTuple<OldA, OldB, OldC>, OutTuple_, GroupKey_, ResultContainer_, Result_> where OutTuple_ : AbstractTuple
    {

        private Func<ResultContainer_, OldA, OldB, OldC, Action> accumulator;

        protected AbstractGroupTriNode(int groupStoreIndex, int undoStoreIndex,
                Func<TriTuple<OldA, OldB, OldC>, GroupKey_> groupKeyFunction,
                TriConstraintCollector<OldA, OldB, OldC, ResultContainer_, Result_> collector,
                TupleLifecycle nextNodesTupleLifecycle, EnvironmentMode environmentMode)
            : base(groupStoreIndex, undoStoreIndex, groupKeyFunction,
                    collector == null ? null : collector.Supplier(),
                    collector == null ? null : collector.Finisher(),
                    nextNodesTupleLifecycle, environmentMode)
        {
            accumulator = collector == null ? null : collector.Accumulator();
        }

        protected AbstractGroupTriNode(int groupStoreIndex,Func<TriTuple<OldA, OldB, OldC>, GroupKey_> groupKeyFunction, TupleLifecycle nextNodesTupleLifecycle, EnvironmentMode environmentMode)
            : base(groupStoreIndex, groupKeyFunction, nextNodesTupleLifecycle, environmentMode)
        {
            accumulator = null;
        }

        protected override Action Accumulate(ResultContainer_ resultContainer, TriTuple<OldA, OldB, OldC> tuple)
        {
            return accumulator.Invoke(resultContainer, tuple.factA, tuple.factB, tuple.factC);
        }
    }
}
