using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Bridge;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public class BavetBiGroupTriConstraintStream<A, B, C, NewA, NewB> : BavetAbstractTriConstraintStream<A, B, C>
    {

        private BavetAftBridgeBiConstraintStream<NewA, NewB> aftStream;
        private GroupNodeConstructor<BiTuple<NewA, NewB>> nodeConstructor;

        public BavetBiGroupTriConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractTriConstraintStream<A, B, C> parent, GroupNodeConstructor<BiTuple<NewA, NewB>> nodeConstructor)
                : base(constraintFactory, parent)
        {
            this.nodeConstructor = nodeConstructor;
        }

        public void SetAftBridge(BavetAftBridgeBiConstraintStream<NewA, NewB> aftStream)
        {
            this.aftStream = aftStream;
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            var aftStreamChildList = aftStream.GetChildStreamList();
            nodeConstructor.Build(buildHelper, parent.GetTupleSource(), aftStream, aftStreamChildList.OfType<ConstraintStream>().ToList(),
                this, childStreamList.OfType<ConstraintStream>().ToList(), constraintFactory.GetEnvironmentMode());
        }

        public override string ToString()
        {
            return "BiGroup()";
        }
    }
}

