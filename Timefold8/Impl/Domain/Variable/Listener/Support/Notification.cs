using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Domain.Variable.Listener.Support;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public interface Notification<T> where T : AbstractVariableListener<object>
    {
        void TriggerBefore(T variableListener, ScoreDirector scoreDirector);
        void TriggerAfter(T variableListener, ScoreDirector scoreDirector);
    }

    public static class NotificationHelper<T> where T : AbstractVariableListener<object>
    {
        public static BasicVariableNotification VariableChanged(Object entity)
        {
            return new VariableChangedNotification(entity);
        }
    }
}
