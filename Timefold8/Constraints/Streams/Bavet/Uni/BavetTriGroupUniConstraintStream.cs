using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Bi;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Bridge;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class BavetTriGroupUniConstraintStream<A, NewA, NewB, NewC>
        : BavetAbstractUniConstraintStream<A> 
    {
        private GroupNodeConstructor<TriTuple<NewA, NewB, NewC>> nodeConstructor;
        private BavetAftBridgeTriConstraintStream<NewA, NewB, NewC> aftStream;

        public BavetTriGroupUniConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractUniConstraintStream<A> parent, GroupNodeConstructor<TriTuple<NewA, NewB, NewC>> nodeConstructor)
            : base(constraintFactory, parent) 
        {
            this.nodeConstructor = nodeConstructor;
        }

        public void SetAftBridge(BavetAftBridgeTriConstraintStream<NewA, NewB, NewC> aftStream)
        {
            this.aftStream = aftStream;
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            var aftStreamChildList = aftStream.GetChildStreamList();
            nodeConstructor.Build(buildHelper, parent.GetTupleSource(), aftStream, aftStreamChildList.OfType<ConstraintStream>().ToList(), this, childStreamList.OfType<ConstraintStream>().ToList(),
                    constraintFactory.GetEnvironmentMode());
        }
    }
}
