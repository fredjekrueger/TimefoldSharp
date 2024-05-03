using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor
{
    public class FromEntityPropertyValueRangeDescriptor : AbstractFromPropertyValueRangeDescriptor
    {
        public FromEntityPropertyValueRangeDescriptor(GenuineVariableDescriptor variableDescriptor,
               bool addNullInValueRange, MemberAccessor memberAccessor) : base(variableDescriptor, addNullInValueRange, memberAccessor)
        {

        }

        public override bool IsEntityIndependent()
        {
            return false;
        }
    }
}
