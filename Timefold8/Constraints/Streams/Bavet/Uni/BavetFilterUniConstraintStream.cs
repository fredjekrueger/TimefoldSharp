using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public class BavetFilterUniConstraintStream<A> : BavetAbstractUniConstraintStream<A>
    {
        private Func<A, bool> predicate;

        public BavetFilterUniConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractUniConstraintStream<A> parent, Func<A, bool> predicate)
            : base(constraintFactory, parent)
        {
            this.predicate = predicate;
            if (predicate == null)
            {
                throw new Exception("The predicate (null) cannot be null.");
            }
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            buildHelper.PutInsertUpdateRetract(this, childStreamList, tupleLifecycle => new ConditionalUniTupleLifecycle<A>(predicate, tupleLifecycle));
        }
    }
}
