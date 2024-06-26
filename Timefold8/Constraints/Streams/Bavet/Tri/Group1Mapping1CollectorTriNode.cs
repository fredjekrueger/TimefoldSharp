using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Tri;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public class Group1Mapping1CollectorTriNode<OldA, OldB, OldC, A, B, ResultContainer_> : AbstractGroupTriNode<OldA, OldB, OldC, BiTuple<A, B>, A, ResultContainer_, B>
    {

        private int outputStoreSize;

        public Group1Mapping1CollectorTriNode(Func<OldA, OldB, OldC, A> groupKeyMapping, int groupStoreIndex, int undoStoreIndex, TriConstraintCollector<OldA, OldB, OldC, ResultContainer_, B> collector,
                TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode)
                : base(groupStoreIndex, undoStoreIndex, tuple => Group1Mapping0CollectorTriNode<OldA, OldB,OldC,A>.CreateGroupKey(groupKeyMapping, tuple), collector, nextNodesTupleLifecycle, environmentMode)
        {
            this.outputStoreSize = outputStoreSize;
        }

        protected override BiTuple<A, B> CreateOutTuple(A a)
        {
            return new BiTuple<A, B>(a, default(B), outputStoreSize);
        }

        protected override void UpdateOutTupleToResult(BiTuple<A, B> outTuple, B b)
        {
            outTuple.factB = b;
        }
    }
}

