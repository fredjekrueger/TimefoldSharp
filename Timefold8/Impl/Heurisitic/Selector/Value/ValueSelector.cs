using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value
{
    public interface ValueSelector : Selector
    {
        GenuineVariableDescriptor GetVariableDescriptor();
        IEnumerator<Object> Iterator(Object entity);
        long GetSize(Object entity);
    }
}
