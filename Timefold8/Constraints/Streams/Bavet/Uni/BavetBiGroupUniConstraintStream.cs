using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Bi;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Bridge;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.API.Score.Stream;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class BavetBiGroupUniConstraintStream<A, NewA, NewB> : BavetAbstractUniConstraintStream<A>
    {
        private GroupNodeConstructor<BiTuple<NewA, NewB>> nodeConstructor;
        private BavetAftBridgeBiConstraintStream<NewA, NewB> aftStream;

        public BavetBiGroupUniConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractUniConstraintStream<A> parent, GroupNodeConstructor<BiTuple<NewA, NewB>> nodeConstructor)
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
            nodeConstructor.Build(buildHelper, parent.GetTupleSource(), aftStream, aftStreamChildList.OfType<ConstraintStream>()
                .ToList(), this, childStreamList.OfType<ConstraintStream>().ToList(), constraintFactory.GetEnvironmentMode());
        }

        public override string ToString()
        {
            return "BiGroup()";
        }
    }
}
