using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public abstract class AbstractGroupUniNode<OldA, OutTuple_, GroupKey_, ResultContainer_, Result_>
        : AbstractGroupNode<UniTuple<OldA>, OutTuple_, GroupKey_, ResultContainer_, Result_> where OutTuple_ : AbstractTuple
    {
        private Func<ResultContainer_, OldA, Action> accumulator;

        protected AbstractGroupUniNode(int groupStoreIndex, int undoStoreIndex,
                Func<UniTuple<OldA>, GroupKey_> groupKeyFunction,
                UniConstraintCollector<OldA, ResultContainer_, Result_> collector,
                TupleLifecycle nextNodesTupleLifecycle, EnvironmentMode environmentMode)
            : base(groupStoreIndex, undoStoreIndex,
                    groupKeyFunction,
                    collector == null ? null : collector.Supplier(),
                    collector == null ? null : collector.Finisher(),
                    nextNodesTupleLifecycle, environmentMode)
        {
            accumulator = collector == null ? null : collector.Accumulator();
        }

        protected AbstractGroupUniNode(int groupStoreIndex, Func<UniTuple<OldA>, GroupKey_> groupKeyFunction, TupleLifecycle nextNodesTupleLifecycle, EnvironmentMode environmentMode)
            :base(groupStoreIndex, groupKeyFunction, nextNodesTupleLifecycle, environmentMode)
        {
            accumulator = null;
        }

        protected override Action Accumulate(ResultContainer_ resultContainer, UniTuple<OldA> tuple)
        {
            return accumulator.Invoke(resultContainer, tuple.factA);
        }
    }
}