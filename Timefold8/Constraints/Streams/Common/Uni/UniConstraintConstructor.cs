using System.Collections;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Uni
{
    public interface UniConstraintConstructor<A> : ConstraintConstructor<Func<A, object>, Func<A, ICollection>>
    {
    }
}