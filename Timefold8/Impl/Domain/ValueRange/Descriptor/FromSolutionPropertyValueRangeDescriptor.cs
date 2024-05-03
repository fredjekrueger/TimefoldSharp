using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor
{
    public class FromSolutionPropertyValueRangeDescriptor : AbstractFromPropertyValueRangeDescriptor, EntityIndependentValueRangeDescriptor
    {
        public FromSolutionPropertyValueRangeDescriptor(
                GenuineVariableDescriptor variableDescriptor, bool addNullInValueRange,
                MemberAccessor memberAccessor) : base(variableDescriptor, addNullInValueRange, memberAccessor)
        {
        }

        public ValueRange<object> ExtractValueRange(ISolution solution)
        {
            return ReadValueRange(solution);
        }

        public override bool IsEntityIndependent()
        {
            return true;
        }
    }
}
