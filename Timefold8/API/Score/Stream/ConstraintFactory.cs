using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public interface ConstraintFactory
    {
        BiConstraintStream<A, A> ForEachUniquePair<A, Property_>(params BiJoiner<A, A, Property_>[] joiners);
        BiConstraintStream<A, A> ForEachUniquePair<A, Property_>(Type sourceClass, BiJoiner<A, A, Property_> joiner1, BiJoiner<A, A, Property_> joiner2);
        BiConstraintStream<A, A> ForEachUniquePair<A, Property_>(Type sourceClass, params BiJoiner<A, A, Property_>[] joiners);
        UniConstraintStream<A> ForEach<A>(Type sourceClass);
        SolutionDescriptor GetSolutionDescriptor();
    }
}
