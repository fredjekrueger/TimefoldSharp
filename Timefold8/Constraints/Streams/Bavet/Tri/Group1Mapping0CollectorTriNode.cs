using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public class Group1Mapping0CollectorTriNode<OldA, OldB, OldC, A> : AbstractGroupTriNode<OldA, OldB, OldC, UniTuple<A>, A, object, object> {

        private int outputStoreSize;

        public Group1Mapping0CollectorTriNode(Func<OldA, OldB, OldC, A> groupKeyMapping, int groupStoreIndex,
                TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode)
                : base(groupStoreIndex,
                    tuple => CreateGroupKey(groupKeyMapping, tuple), nextNodesTupleLifecycle, environmentMode)
        {
            this.outputStoreSize = outputStoreSize;
        }
        public static A CreateGroupKey<A, OldA, OldB, OldC>(Func<OldA, OldB, OldC, A> groupKeyMapping, TriTuple<OldA, OldB, OldC> tuple)
        {
            return groupKeyMapping.Invoke(tuple.factA, tuple.factB, tuple.factC);
        }

        protected override Action Accumulate(object resultContainer, TriTuple<OldA, OldB, OldC> tuple)
        {
            throw new NotImplementedException();
        }

        protected override UniTuple<A> CreateOutTuple(A groupKey)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateOutTupleToResult(UniTuple<A> outTuple, object result)
        {
            throw new NotImplementedException();
        }
    }
}
