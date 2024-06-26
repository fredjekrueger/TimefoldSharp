using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.Impl.Score.Director;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public abstract class AbstractNotifiable<T>
        : EntityNotifiable
        where T : AbstractVariableListener<object>
    {

        private readonly ScoreDirector scoreDirector;
        private readonly T variableListener;
        private readonly ICollection<Notification<T>> notificationQueue;
        private readonly int globalOrder;

        public AbstractNotifiable(ScoreDirector scoreDirector, T variableListener, ICollection<Notification<T>> notificationQueue, int globalOrder)
        {
            this.scoreDirector = scoreDirector;
            this.variableListener = variableListener;
            this.notificationQueue = notificationQueue;
            this.globalOrder = globalOrder;
        }

        protected bool StoreForLater(Notification<T> notification)
        {
            notificationQueue.Add(notification);
            return true; // hier stond het een beetje anders
        }

        protected void TriggerBefore(Notification<T> notification)
        {
            notification.TriggerBefore(variableListener, scoreDirector);
        }

        public static EntityNotifiable BuildNotifiable(ScoreDirector scoreDirector, AbstractVariableListener<object> variableListener, int globalOrder)
        {
            if (variableListener is ListVariableListener<object, object>)
            {
                return new ListVariableListenerNotifiable(scoreDirector, ((ListVariableListener<Object, Object>)variableListener), new List<Notification<ListVariableListener<object, object>>>(), globalOrder);
            }
            else
            {
                   VariableListener<object> basicVariableListener = (VariableListener<object>)variableListener;
                if(basicVariableListener.RequiresUniqueEntityEvents())
                   return new VariableListenerNotifiable(scoreDirector, basicVariableListener, new ListBasedScalingOrderedSet<Notification<VariableListener<object>>>(),  globalOrder);
                else
                    return new VariableListenerNotifiable(scoreDirector, basicVariableListener, new List<Notification<VariableListener<object>>>(), globalOrder);//this was queue, but prob with Q not inheriting from ICOllection<T>
            }
        }

        public void CloseVariableListener()
        {
            variableListener.Dispose();
        }

        public void ResetWorkingSolution()
        {
            variableListener.ResetWorkingSolution(scoreDirector);
        }

        public void TriggerAllNotifications()
        {
            int notifiedCount = 0;
            foreach (var notification in notificationQueue)
            {
                notification.TriggerAfter(variableListener, scoreDirector);
                notifiedCount++;
            }
            if (notifiedCount != notificationQueue.Count())
            {
                throw new Exception("The variableListener ( has been notified with notifiedCount (" + notifiedCount
                        + ") but after being triggered, its notificationCount (" + notificationQueue.Count()
                        + ") is different.\n"
                        + "Maybe that variableListener () changed an upstream shadow variable (which is illegal).");
            }
            notificationQueue.Clear();
        }
    }
}
