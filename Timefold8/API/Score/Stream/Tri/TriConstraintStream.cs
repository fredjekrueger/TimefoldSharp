using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Bi;

namespace TimefoldSharp.Core.API.Score.Stream.Tri
{
    public interface TriConstraintStream<A, B, C> : ConstraintStream
    {
        TriConstraintStream<GroupKeyA_, GroupKeyB_, Result_> GroupBy<GroupKeyA_, GroupKeyB_, ResultContainer_, Result_>(
            Func<A, B, C, GroupKeyA_> groupKeyAMapping, Func<A, B, C, GroupKeyB_> groupKeyBMapping, TriConstraintCollector<A, B, C, ResultContainer_, Result_> collector);

        BiConstraintStream<GroupKey_, Result_> GroupBy<GroupKey_, ResultContainer_, Result_>(
            Func<A, B, C, GroupKey_> groupKeyMapping, TriConstraintCollector<A, B, C, ResultContainer_, Result_> collector);
    }
}