using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Tri;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Bridge
{
    public class BavetAftBridgeTriConstraintStream<A,B,C> : BavetAbstractTriConstraintStream<A, B, C>, TupleSource
    {
        public BavetAftBridgeTriConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractConstraintStream parent)
            : base(constraintFactory, parent) 
        {

        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            
        }

        public override int GetHashCode()
        {
            return parent.GetHashCode();
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
            BavetAftBridgeTriConstraintStream <A, B, C> that = (BavetAftBridgeTriConstraintStream<A, B, C>) o;
            return parent.Equals(that.parent);
        }
    }
}
