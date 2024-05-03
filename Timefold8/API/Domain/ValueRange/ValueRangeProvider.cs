namespace TimefoldSharp.Core.API.Domain.ValueRange
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class ValueRangeProviderAttribute : Attribute
    {
        public string Id { get; set; } = "";
    }
}
