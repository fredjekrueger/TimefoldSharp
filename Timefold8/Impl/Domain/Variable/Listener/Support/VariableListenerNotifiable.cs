using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Domain.Variable.Listener.Support;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public sealed class VariableListenerNotifiable : AbstractNotifiable<VariableListener<object>>
    {

        public VariableListenerNotifiable(
                ScoreDirector scoreDirector,
                VariableListener<object> variableListener,
                List<Notification<VariableListener<object>>> notificationQueue,
                int globalOrder)
        : base(scoreDirector, variableListener, notificationQueue, globalOrder)
        {
        }

        public void NotifyBefore(BasicVariableNotification notification)
        {
            if (StoreForLater(notification))
            {
                TriggerBefore(notification);
            }
        }
    }
}
