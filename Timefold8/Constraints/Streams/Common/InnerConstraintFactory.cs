using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score.Stream.Bi;
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

        public abstract UniConstraintStream<A> ForEach<A>(Type sourceClass);

        public BiConstraintStream<A, A> ForEachUniquePair<A, Property_>(params BiJoiner<A, A, Property_>[] joiners)
        {
            BiJoinerComber<A, A, Property_> joinerComber = BiJoinerComber<A, A, Property_>.Comb(joiners);
            joinerComber.AddJoiner(BuildLessThanId<A, Property_>(typeof(A)));
            return ((InnerUniConstraintStream<A>)ForEach<A>(typeof(A))).Join(ForEach<A>(typeof(A)), joinerComber);
        }

        public abstract SolutionDescriptor GetSolutionDescriptor();

        private DefaultBiJoiner<A, A, Property_> BuildLessThanId<A, Property_>(Type sourceClass)
        {
            SolutionDescriptor solutionDescriptor = GetSolutionDescriptor();
            MemberAccessor planningIdMemberAccessor = solutionDescriptor.GetPlanningIdAccessor(sourceClass);
            if (planningIdMemberAccessor == null)
            {
                throw new Exception("The fromClass  annotation,"
                    + " so the pairs cannot be made unique ([A,B] vs [B,A]).");
            }
            Func<A, Property_> planningIdGetter = planningIdMemberAccessor.GetGetterFunction<A, Property_>(); // JDEF hier werkt cast niet
            return (DefaultBiJoiner<A, A, Property_>)Joiners.LessThan(planningIdGetter);
        }
    }
}
