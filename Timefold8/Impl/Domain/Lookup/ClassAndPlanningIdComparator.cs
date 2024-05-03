using TimefoldSharp.Core.API.Domain.Common;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;

namespace TimefoldSharp.Core.Impl.Domain.Lookup
{
    public sealed class ClassAndPlanningIdComparator //implements Comparator<Object> {
    {
        private readonly MemberAccessorFactory memberAccessorFactory;
        private readonly DomainAccessType domainAccessType;
        private readonly bool failFastIfNoPlanningId;

        public ClassAndPlanningIdComparator(MemberAccessorFactory memberAccessorFactory, DomainAccessType domainAccessType, bool failFastIfNoPlanningId)
        {
            this.memberAccessorFactory = memberAccessorFactory;
            this.domainAccessType = domainAccessType;
            this.failFastIfNoPlanningId = failFastIfNoPlanningId;
        }
    }
}
