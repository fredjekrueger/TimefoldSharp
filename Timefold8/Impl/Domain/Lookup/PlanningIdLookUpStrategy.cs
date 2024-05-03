using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Impl.Domain.Lookup
{
    public class PlanningIdLookUpStrategy : LookUpStrategy
    {
        private readonly MemberAccessor planningIdMemberAccessor;

        public PlanningIdLookUpStrategy(MemberAccessor planningIdMemberAccessor)
        {
            this.planningIdMemberAccessor = planningIdMemberAccessor;
        }

        public void AddWorkingObject(Dictionary<object, object> idToWorkingObjectMap, object workingObject)
        {
            object planningId = ExtractPlanningId(workingObject);
            idToWorkingObjectMap.Add(planningId, workingObject);
        }

        protected object ExtractPlanningId(object externalObject)
        {
            object planningId = planningIdMemberAccessor.ExecuteGetter(externalObject);
            if (planningId == null)
            {
                throw new Exception("The planningId (" + planningId
                        + ") of the member (" + planningIdMemberAccessor + ") of the class.");
            }
            return PairHelper<Type, object>.Of(externalObject.GetType(), planningId);
        }
    }
}
