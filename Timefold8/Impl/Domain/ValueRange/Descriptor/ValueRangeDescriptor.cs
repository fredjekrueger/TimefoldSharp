using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor
{
    public interface ValueRangeDescriptor
    {
        bool IsCountable();
        bool IsEntityIndependent();
        bool MightContainEntity();
        GenuineVariableDescriptor GetVariableDescriptor();
        long ExtractValueRangeSize(ISolution solution, object entity);
        ValueRange<object> ExtractValueRange(ISolution solution, object entity);
    }
}
