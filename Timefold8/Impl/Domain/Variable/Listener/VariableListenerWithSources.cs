using System.Collections.ObjectModel;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener
{
    public sealed class VariableListenerWithSources
    {
        private readonly List<VariableDescriptor> sourceVariableDescriptors;
        private readonly AbstractVariableListener<object> variableListener;

        public VariableListenerWithSources(AbstractVariableListener<object> variableListener, List<VariableDescriptor> sourceVariableDescriptors)
        {
            this.variableListener = variableListener;
            this.sourceVariableDescriptors = sourceVariableDescriptors;
        }

        public VariableListenerWithSources(AbstractVariableListener<Object> variableListener, VariableDescriptor sourceVariableDescriptor)
            : this(variableListener, new List<VariableDescriptor>() { sourceVariableDescriptor })
        {
        }

        public AbstractVariableListener<object> GetVariableListener()
        {
            return variableListener;
        }

        public List<VariableDescriptor> GetSourceVariableDescriptors()
        {
            return sourceVariableDescriptors;
        }

        public List<VariableListenerWithSources> ToCollection()
        {
            return new List<VariableListenerWithSources>() { this};
        }
    }
}
