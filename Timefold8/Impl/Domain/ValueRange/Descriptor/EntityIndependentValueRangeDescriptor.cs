using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor
{
    public interface EntityIndependentValueRangeDescriptor : ValueRangeDescriptor
    {
        ValueRange<object> ExtractValueRange(ISolution solution);
    }
}
