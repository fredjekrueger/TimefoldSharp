using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Core.API.Score.Stream.Tri
{
    public interface TriConstraintCollector<A, B, C, ResultContainer_, Result_>
    {
        Func<ResultContainer_> Supplier();
        Func<ResultContainer_, A, B, C, Action> Accumulator();
        Func<ResultContainer_, Result_> Finisher();
    }
}
