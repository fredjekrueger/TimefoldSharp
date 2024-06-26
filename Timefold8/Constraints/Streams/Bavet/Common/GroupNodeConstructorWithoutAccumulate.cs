using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public class GroupNodeConstructorWithoutAccumulate<Tuple_> : GroupNodeConstructor<Tuple_> where Tuple_ : AbstractTuple
    {
        Func<int, TupleLifecycle, int, EnvironmentMode, AbstractNode> applyAction;

        public GroupNodeConstructorWithoutAccumulate(Func<int, TupleLifecycle, int, EnvironmentMode, AbstractNode> applyAction)
        {
            this.applyAction = applyAction;
        }

        public void Build(NodeBuildHelper buildHelper, BavetAbstractConstraintStream parentTupleSource, BavetAbstractConstraintStream aftStream,
            List<ConstraintStream> aftStreamChildList, BavetAbstractConstraintStream bridgeStream, List<ConstraintStream> bridgeStreamChildList, EnvironmentMode environmentMode)
        {
            if (bridgeStreamChildList.Any())
            {
                throw new Exception("Impossible state: the stream (" + bridgeStream
                        + ") has an non-empty childStreamList (" + bridgeStreamChildList + ") but it's a groupBy bridge.");
            }
            int groupStoreIndex = buildHelper.ReserveTupleStoreIndex(parentTupleSource);
            var tupleLifecycle = buildHelper.GetAggregatedTupleLifecycle(aftStreamChildList);
            int outputStoreSize = buildHelper.ExtractTupleStoreSize(aftStream);
            var node = applyAction.Invoke(groupStoreIndex, tupleLifecycle, outputStoreSize, environmentMode);
            buildHelper.AddNode(node, bridgeStream);
        }
    }
}
