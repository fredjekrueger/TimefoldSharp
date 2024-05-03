using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener
{
    public sealed class VariableListenerWithSources
    {
        private readonly List<VariableDescriptor> sourceVariableDescriptors;
        private readonly AbstractVariableListener<object> variableListener;

        public VariableListenerWithSources(AbstractVariableListener<object> variableListener,
            List<VariableDescriptor> sourceVariableDescriptors)
        {
            this.variableListener = variableListener;
            this.sourceVariableDescriptors = sourceVariableDescriptors;
        }

        public AbstractVariableListener<object> GetVariableListener()
        {
            return variableListener;
        }

        public List<VariableDescriptor> GetSourceVariableDescriptors()
        {
            return sourceVariableDescriptors;
        }
    }
}
