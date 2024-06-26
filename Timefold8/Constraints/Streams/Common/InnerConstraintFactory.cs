﻿using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Constraints.Streams.Common.Bi;
using TimefoldSharp.Core.Constraints.Streams.Common.Uni;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Constraints.Streams.Common
{
    public abstract class InnerConstraintFactory<Constraint_> : ConstraintFactory
        where Constraint_ : Constraint
    {
        public List<Constraint_> BuildConstraints(ConstraintProvider constraintProvider)
        {
            List<Constraint> constraints = constraintProvider.DefineConstraints(this);
            if (constraints == null)
            {
                throw new Exception("The constraintProvider class ("
                        + ")'s defineConstraints() must not return null.\n"
                        + "Maybe return an empty array instead if there are no constraints.");
            }


            return constraints.Select(c => (Constraint_)c).ToList();
        }

        public abstract UniConstraintStream<A> ForEach<A>();

        public BiConstraintStream<A, A> ForEachUniquePair<A>(params BiJoiner<A, A>[] joiners)
        {
            BiJoinerComber<A, A> joinerComber = BiJoinerComber<A, A>.Comb(joiners);
            joinerComber.AddJoiner(BuildLessThanId<A>());
            return ((InnerUniConstraintStream<A>)ForEach<A>()).Join(ForEach<A>(), joinerComber);
        }

        public BiConstraintStream<A, A> ForEachUniquePair<A>(BiJoiner<A, A> joiner1, BiJoiner<A, A> joiner2)
        {
            return ForEachUniquePair(new BiJoiner<A, A>[] { joiner1, joiner2 });
        }

        public abstract SolutionDescriptor GetSolutionDescriptor();

        private DefaultBiJoiner<A, A> BuildLessThanId<A>()
        {
            SolutionDescriptor solutionDescriptor = GetSolutionDescriptor();
            MemberAccessor planningIdMemberAccessor = solutionDescriptor.GetPlanningIdAccessor(typeof(A));
            if (planningIdMemberAccessor == null)
            {
                throw new Exception("The fromClass  annotation,"
                    + " so the pairs cannot be made unique ([A,B] vs [B,A]).");
            }
            Func<A, object> planningIdGetter = planningIdMemberAccessor.GetGetterFunction<A>();
            return (DefaultBiJoiner<A, A>)Joiners.LessThan(planningIdGetter);
        }
    }
}
