namespace TimefoldSharp.Core.API.Domain.Variable
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class AnchorShadowVariableAttribute : Attribute
    {
        public string SourceVariableName { get; set; }
    }
}
