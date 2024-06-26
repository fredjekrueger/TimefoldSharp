using TimefoldSharp.Core.API.Score.Buildin.HardSoftLong;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Tri;

namespace TimefoldSharp.Core.API.Score.Stream.Uni
{
    public interface UniConstraintStream<A> : ConstraintStream
    {
        BiConstraintStream<A, B> Join<B>(Type otherClass, BiJoiner<A, B> joiner1, BiJoiner<A, B> joiner2);
        BiConstraintStream<A, B> Join<B>(Type otherClass, params BiJoiner<A, B>[] joiners);
        BiConstraintStream<A, B> Join<B>(UniConstraintStream<B> otherStream, params BiJoiner<A, B>[] joiners);

        UniConstraintStream<A> Filter(Func<A, bool> predicate);
        UniConstraintBuilder<A> Penalize(Score constraintWeight);
        UniConstraintBuilder<A> PenalizeLong(Score constraintWeight, Func<A, long> matchWeigher);

        TriConstraintStream<GroupKeyA_, GroupKeyB_, Result_> GroupBy<GroupKeyA_, GroupKeyB_, ResultContainer_, Result_>
            (Func<A, GroupKeyA_> groupKeyAMapping, Func<A, GroupKeyB_> groupKeyBMapping, UniConstraintCollector<A, ResultContainer_, Result_> collector);

        BiConstraintStream<GroupKey_, Result_> GroupBy<GroupKey_, ResultContainer_, Result_>(
            Func<A, GroupKey_> groupKeyMapping, UniConstraintCollector<A, ResultContainer_, Result_> collector);
    }
}
