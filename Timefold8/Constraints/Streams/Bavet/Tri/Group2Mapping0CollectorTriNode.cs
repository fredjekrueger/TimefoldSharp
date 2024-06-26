using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public class Group2Mapping0CollectorTriNode<OldA, OldB, OldC, A, B> : AbstractGroupTriNode<OldA, OldB, OldC, BiTuple<A, B>, Pair<A, B>, object, object>
    {

        private int outputStoreSize;

        public Group2Mapping0CollectorTriNode(Func<OldA, OldB, OldC, A> groupKeyMappingA, Func<OldA, OldB, OldC, B> groupKeyMappingB, int groupStoreIndex, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode)
                : base(groupStoreIndex,
                    tuple=>CreateGroupKey(groupKeyMappingA, groupKeyMappingB, tuple), nextNodesTupleLifecycle, environmentMode)
        {
            this.outputStoreSize = outputStoreSize;
        }

        public static Pair<A, B> CreateGroupKey<A, B, OldA, OldB, OldC>(Func<OldA, OldB, OldC, A> groupKeyMappingA,
            Func<OldA, OldB, OldC, B> groupKeyMappingB, TriTuple<OldA, OldB, OldC> tuple)
        {
            OldA oldA = tuple.factA;
            OldB oldB = tuple.factB;
            OldC oldC = tuple.factC;
            A a = groupKeyMappingA.Invoke(oldA, oldB, oldC);
            B b = groupKeyMappingB.Invoke(oldA, oldB, oldC);
            return PairHelper<A, B>.Of(a, b);
        }

        protected override Action Accumulate(object resultContainer, TriTuple<OldA, OldB, OldC> tuple)
        {
            throw new NotImplementedException();
        }

        protected override BiTuple<A, B> CreateOutTuple(Pair<A, B> groupKey)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateOutTupleToResult(BiTuple<A, B> outTuple, object result)
        {
            throw new NotImplementedException();
        }
    }
}
