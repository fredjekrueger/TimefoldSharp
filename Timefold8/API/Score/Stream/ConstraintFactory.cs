using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.Constraints.Streams.Common.Uni;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public interface ConstraintFactory
    {
        BiConstraintStream<A, A> ForEachUniquePair<A, Property_>(params BiJoiner<A, A, Property_>[] joiners);

        UniConstraintStream<A> ForEach<A>(Type sourceClass);

        SolutionDescriptor GetSolutionDescriptor();
    }
}
