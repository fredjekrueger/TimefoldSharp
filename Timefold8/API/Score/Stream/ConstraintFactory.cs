using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public interface ConstraintFactory
    {
        BiConstraintStream<A, A> ForEachUniquePair<A>(params BiJoiner<A, A>[] joiners);
        BiConstraintStream<A, A> ForEachUniquePair<A>(BiJoiner<A, A> joiner1, BiJoiner<A, A> joiner2);
        UniConstraintStream<A> ForEach<A>();
        SolutionDescriptor GetSolutionDescriptor();
    }
}
