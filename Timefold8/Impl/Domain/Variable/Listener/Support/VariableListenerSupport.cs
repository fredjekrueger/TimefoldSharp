using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Domain.Variable.Listener.Support;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public sealed class VariableListenerSupport : SupplyManager
    {

        private readonly InnerScoreDirector scoreDirector;
        private readonly NotifiableRegistry notifiableRegistry;

        private readonly Dictionary<Demand, Supply.Supply> supplyMap = new Dictionary<Demand, Supply.Supply>();
        private readonly Dictionary<Demand, long> demandCounterMap = new Dictionary<Demand, long>();

        public static VariableListenerSupport Create(InnerScoreDirector scoreDirector)
        {
            return new VariableListenerSupport(scoreDirector, new NotifiableRegistry(scoreDirector.GetSolutionDescriptor()));
        }

        public void TriggerVariableListenersInNotificationQueues()
        {
            foreach (var notifiable in notifiableRegistry.GetAll())
            {
                notifiable.TriggerAllNotifications();
            }
        }

        public void BeforeVariableChanged(VariableDescriptor variableDescriptor, object entity)
        {
            var notifiables = notifiableRegistry.Get(variableDescriptor);
            if (notifiables.Count > 0)
            {
                BasicVariableNotification notification = NotificationHelper<AbstractVariableListener<object>>.VariableChanged(entity);
                foreach (var notifiable in notifiables)
                {
                    notifiable.NotifyBefore(notification);
                }
            }
        }

        VariableListenerSupport(InnerScoreDirector scoreDirector, NotifiableRegistry notifiableRegistry)
        {
            this.scoreDirector = scoreDirector;
            this.notifiableRegistry = notifiableRegistry;
        }

        public void Close()
        {
            foreach (var notifiable in notifiableRegistry.GetAll())
            {
                notifiable.CloseVariableListener();
            }
        }

        public void LinkVariableListeners()
        {
            scoreDirector.GetSolutionDescriptor().GetEntityDescriptors()
                    .SelectMany(entity => entity.GetDeclaredShadowVariableDescriptors())
                    .Where(descriptor => descriptor.HasVariableListener())
                    .OrderBy(descriptor => descriptor.GlobalShadowOrder).ToList()
                    .ForEach(descriptor => ProcessShadowVariableDescriptor(descriptor));
        }

        private void ProcessShadowVariableDescriptor(ShadowVariableDescriptor shadowVariableDescriptor)
        {
            foreach (var listenerWithSources in shadowVariableDescriptor.BuildVariableListeners(this))
            {
                AbstractVariableListener<object> variableListener = listenerWithSources.GetVariableListener();
                if (variableListener is Supply.Supply supply)
                {
                    // Non-sourced variable listeners (ie. ones provided by the user) can never be a supply.
                    Demand demand = shadowVariableDescriptor.GetProvidedDemand<Supply.Supply>();
                    supplyMap.Add(demand, supply);
                    demandCounterMap.Add(demand, 1L);
                }
                int globalOrder = shadowVariableDescriptor.GlobalShadowOrder;
                notifiableRegistry.RegisterNotifiable(
                        listenerWithSources.GetSourceVariableDescriptors(), AbstractNotifiable<AbstractVariableListener<object>>.BuildNotifiable(scoreDirector, variableListener, globalOrder));
            }
        }

        public void ResetWorkingSolution()
        {
            foreach (var notifiable in notifiableRegistry.GetAll())
            {
                notifiable.ResetWorkingSolution();
            }
        }

        public Supply.Supply Demand(Demand demand)
        {
            long activeDemandCount;
            if (demandCounterMap.TryGetValue(demand, out var count))
            {
                activeDemandCount = demandCounterMap[demand] = count + 1;
            }
            else
            {
                activeDemandCount = demandCounterMap[demand] = 1L;
            }
            if (activeDemandCount == 1L)
            { // This is a new demand, create the supply.
                Supply.Supply supply = CreateSupply(demand);
                supplyMap.Add(demand, supply);
                return supply;
            }
            else
            { // Return existing supply.
                return supplyMap[demand];
            }
        }
        private int nextGlobalOrder = 0;

        private Supply.Supply CreateSupply(Demand demand)
        {
            var supply = demand.CreateExternalizedSupply(this);
            if (supply is SourcedVariableListener) {
                SourcedVariableListener variableListener = (SourcedVariableListener)supply;
                // An external ScoreDirector can be created before the working solution is set
                if (scoreDirector.GetWorkingSolution() != null)
                {
                    variableListener.ResetWorkingSolution(scoreDirector);
                }
                notifiableRegistry.RegisterNotifiable(variableListener.GetSourceVariableDescriptor(),
                        AbstractNotifiable<AbstractVariableListener<object>>.BuildNotifiable(scoreDirector, variableListener, nextGlobalOrder++));
            }
            return supply;
        }
    }
}
