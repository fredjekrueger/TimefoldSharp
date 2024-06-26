using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public sealed class NotifiableRegistry
    {

        private readonly Dictionary<EntityDescriptor, HashSet<EntityNotifiable>> sourceEntityToNotifiableMap = new Dictionary<EntityDescriptor, HashSet<EntityNotifiable>>();
        private readonly Dictionary<VariableDescriptor, List<ListVariableListenerNotifiable>> sourceListVariableToNotifiableMap = new Dictionary<VariableDescriptor, List<ListVariableListenerNotifiable>>();
        private readonly Dictionary<VariableDescriptor, List<VariableListenerNotifiable>> sourceVariableToNotifiableMap = new Dictionary<VariableDescriptor, List<VariableListenerNotifiable>>();

        private readonly List<Notifiable> notifiableList = new List<Notifiable>();

        public List<VariableListenerNotifiable> Get(VariableDescriptor variableDescriptor)
        {
            return sourceVariableToNotifiableMap.GetValueOrDefault(variableDescriptor, new List<VariableListenerNotifiable>()); // Avoids null for chained swap move on an unchained var.
        }

        public NotifiableRegistry(SolutionDescriptor solutionDescriptor)
        {
            foreach (var entityDescriptor in solutionDescriptor.GetEntityDescriptors())
            {
                sourceEntityToNotifiableMap.Add(entityDescriptor, new HashSet<EntityNotifiable>());
                foreach (var variableDescriptor in entityDescriptor.GetDeclaredVariableDescriptors())
                {
                    if (variableDescriptor.IsGenuineListVariable())
                    {
                        sourceListVariableToNotifiableMap.Add(variableDescriptor, new List<ListVariableListenerNotifiable>());
                    }
                    else
                    {
                        sourceVariableToNotifiableMap.Add(variableDescriptor, new List<VariableListenerNotifiable>());
                    }
                }
            }
        }

        public IEnumerable<Notifiable> GetAll()
        {
            return notifiableList;
        }

        public void RegisterNotifiable(VariableDescriptor source, EntityNotifiable notifiable)
        {
            RegisterNotifiable(new List<VariableDescriptor>() { source }, notifiable);
        }

        public void RegisterNotifiable(List<VariableDescriptor> sources, EntityNotifiable notifiable)
        {
            foreach (var source in sources)
            {
                if (source.IsGenuineListVariable())
                {
                    sourceListVariableToNotifiableMap[source].Add(((ListVariableListenerNotifiable)notifiable));
                }
                else
                {
                    sourceVariableToNotifiableMap[source].Add(((VariableListenerNotifiable)notifiable));
                }
                sourceEntityToNotifiableMap[source.EntityDescriptor].Add(notifiable);
            }
            notifiableList.Add(notifiable);
        }
    }
}