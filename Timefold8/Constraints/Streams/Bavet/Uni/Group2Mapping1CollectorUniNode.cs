using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class Group2Mapping1CollectorUniNode<OldA, A, B, C, ResultContainer_> : AbstractGroupUniNode<OldA, TriTuple<A, B, C>, Pair<A, B>, ResultContainer_, C>
    {

        private int outputStoreSize;

        public Group2Mapping1CollectorUniNode(Func<OldA, A> groupKeyMappingA, Func<OldA, B> groupKeyMappingB, int groupStoreIndex, int undoStoreIndex,
            UniConstraintCollector<OldA, ResultContainer_, C> collector, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode)
                : base(groupStoreIndex, undoStoreIndex,
                    tuple=>CreateGroupKey(groupKeyMappingA, groupKeyMappingB, tuple), collector,
                    nextNodesTupleLifecycle, environmentMode)
        {
            this.outputStoreSize = outputStoreSize;
        }

        static Pair<A, B> CreateGroupKey(Func<OldA, A> groupKeyMappingA, Func<OldA, B> groupKeyMappingB, UniTuple<OldA> tuple)
        {
            OldA oldA = tuple.factA;
            A a = groupKeyMappingA.Invoke(oldA);
            B b = groupKeyMappingB.Invoke(oldA);
            return PairHelper<A, B>.Of(a, b);
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