using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class Group1Mapping0CollectorUniNode<OldA, A> : AbstractGroupUniNode<OldA, UniTuple<A>, A, object, object>
    {
        private int outputStoreSize;

        public Group1Mapping0CollectorUniNode(Func<OldA, A> groupKeyMapping, int groupStoreIndex, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode)
            :base(groupStoreIndex, tuple=>CreateGroupKey(groupKeyMapping, tuple), nextNodesTupleLifecycle, environmentMode)
        {
            this.outputStoreSize = outputStoreSize;
        }

        public static A CreateGroupKey<A, OldA>(Func<OldA, A> groupKeyMapping, UniTuple<OldA> tuple)
        {
            return groupKeyMapping.Invoke(tuple.factA);
        }

        protected override UniTuple<A> CreateOutTuple(A a)
        {
            return new UniTuple<A>(a, outputStoreSize);
        }

        protected override void UpdateOutTupleToResult(UniTuple<A> outTuple, object result)
        {
            throw new Exception("Impossible state: collector is null.");
        }
    }
}
