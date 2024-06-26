using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Bridge;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public class BavetTriGroupTriConstraintStream<A, B, C, NewA, NewB, NewC>: BavetAbstractTriConstraintStream<A, B, C> 
    {

        private GroupNodeConstructor<TriTuple<NewA, NewB, NewC>> nodeConstructor;
        private BavetAftBridgeTriConstraintStream<NewA, NewB, NewC> aftStream;

        public BavetTriGroupTriConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractTriConstraintStream<A, B, C> parent, GroupNodeConstructor<TriTuple<NewA, NewB, NewC>> nodeConstructor)
            :base(constraintFactory, parent)
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

        public override string ToString()
        {
            return "TriGroup()";
        }
    }
}
