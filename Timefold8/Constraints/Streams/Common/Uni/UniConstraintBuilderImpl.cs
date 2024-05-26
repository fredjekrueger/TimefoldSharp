using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Uni
{ 
    public class UniConstraintBuilderImpl<A> : AbstractConstraintBuilder<A, A>, UniConstraintBuilder<A>
    {
        public UniConstraintBuilderImpl(UniConstraintConstructor<A> constraintConstructor, ScoreImpactType impactType, API.Score.Score constraintWeight)
            :base((ConstraintConstructor<A,A>)constraintConstructor, impactType, constraintWeight)
        {

        }

        public UniConstraintBuilderImpl(Func<string, string, API.Score.Score, ScoreImpactType, object, object, Constraint> factory, ScoreImpactType impactType, API.Score.Score constraintWeight)
           : base(factory, impactType, constraintWeight)
        {

        }

        protected override IndictedObjectsMapping_ GetIndictedObjectsMapping<IndictedObjectsMapping_>()
        {
            return default(IndictedObjectsMapping_);
        }

        protected override JustificationMapping_ GetJustificationMapping<JustificationMapping_>()
        {
            return default(JustificationMapping_);
        }
    }
}
