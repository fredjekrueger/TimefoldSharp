using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Core.Constraints.Streams.Common.Uni
{
    public interface UniConstraintConstructor<A> : ConstraintConstructor<Func<A, object>, Func<A, ICollection>>
    {
    }
}