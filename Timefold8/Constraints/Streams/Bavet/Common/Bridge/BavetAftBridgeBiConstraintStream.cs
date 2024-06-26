using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Bridge
{
    public class BavetAftBridgeBiConstraintStream<A, B>: BavetAbstractBiConstraintStream<A, B>, TupleSource 
    {

        public BavetAftBridgeBiConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractConstraintStream parent)
            :base(constraintFactory, parent)
        {
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (o == null || GetType() != o.GetType())
            {
                return false;
            }
            BavetAftBridgeBiConstraintStream<A,B> that = (BavetAftBridgeBiConstraintStream <A,B>) o;
            return parent.Equals(that.parent);
        }

        public override string ToString()
        {
            return "Bridge from " + parent + " with " + childStreamList.Count + " children";
        }

        public override int GetHashCode()
        {
            return parent.GetHashCode();
        }
    }
}
