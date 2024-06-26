using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Tri;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public class Group2Mapping1CollectorTriNode<OldA, OldB, OldC, A, B, C, ResultContainer_> : AbstractGroupTriNode<OldA, OldB, OldC, TriTuple<A, B, C>, Pair<A, B>, ResultContainer_, C>
    {

        private int outputStoreSize;

        public Group2Mapping1CollectorTriNode(Func<OldA, OldB, OldC, A> groupKeyMappingA, Func<OldA, OldB, OldC, B> groupKeyMappingB, int groupStoreIndex, int undoStoreIndex,
                TriConstraintCollector<OldA, OldB, OldC, ResultContainer_, C> collector, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode)
                : base(groupStoreIndex, undoStoreIndex, tuple => Group2Mapping0CollectorTriNode < OldA, OldB, OldC, A, B>.CreateGroupKey(groupKeyMappingA, groupKeyMappingB, tuple), collector, nextNodesTupleLifecycle, environmentMode)
        {
            this.outputStoreSize = outputStoreSize;
        }

        protected override TriTuple<A, B, C> CreateOutTuple(Pair<A, B> groupKey)
        {
            return new TriTuple<A, B, C>(groupKey.GetKey(), groupKey.GetValue(), default(C), outputStoreSize);
        }

        protected override void UpdateOutTupleToResult(TriTuple<A, B, C> outTuple, C c)
        {
            outTuple.factC = c;
        }

    }
}
