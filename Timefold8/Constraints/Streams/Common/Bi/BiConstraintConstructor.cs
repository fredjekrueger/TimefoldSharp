using System.Collections;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Bi
{
    public interface BiConstraintConstructor<A, B> : ConstraintConstructor<Func<A, B, Object>, Func<A, B, ICollection>>
    {
    }
}
