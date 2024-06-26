using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Tri;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class Group1Mapping1CollectorUniNode<OldA, A, B, ResultContainer_> : AbstractGroupUniNode<OldA, BiTuple<A, B>, A, ResultContainer_, B>
    {
        private int outputStoreSize;

        public Group1Mapping1CollectorUniNode(Func<OldA, A> groupKeyMapping, int groupStoreIndex, int undoStoreIndex, UniConstraintCollector<OldA, ResultContainer_, B> collector,
                TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode)
            :base(groupStoreIndex, undoStoreIndex, tuple=> Group1Mapping0CollectorUniNode<OldA, A>.CreateGroupKey(groupKeyMapping, tuple), collector, nextNodesTupleLifecycle, environmentMode)
        {
            this.outputStoreSize = outputStoreSize;
        }

        protected override void UpdateOutTupleToResult(BiTuple<A, B> outTuple, B b)
        {
            outTuple.factB = b;
        }

        protected override BiTuple<A, B> CreateOutTuple(A a)
        {
            return new BiTuple<A,B>(a, default(B), outputStoreSize);
        }
    }
}
