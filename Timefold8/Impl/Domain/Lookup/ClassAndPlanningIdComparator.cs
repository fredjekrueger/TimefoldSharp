using TimefoldSharp.Core.Impl.Domain.Common.Accessor;

namespace TimefoldSharp.Core.Impl.Domain.Lookup
{
    public sealed class ClassAndPlanningIdComparator //implements Comparator<Object> {
    {
        private readonly MemberAccessorFactory memberAccessorFactory;
        private readonly bool failFastIfNoPlanningId;

        public ClassAndPlanningIdComparator(MemberAccessorFactory memberAccessorFactory, bool failFastIfNoPlanningId)
        {
            this.memberAccessorFactory = memberAccessorFactory;
            this.failFastIfNoPlanningId = failFastIfNoPlanningId;
        }
    }
}
