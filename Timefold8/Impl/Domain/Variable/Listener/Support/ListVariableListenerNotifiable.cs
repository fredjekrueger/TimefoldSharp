using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public sealed class ListVariableListenerNotifiable
        : AbstractNotifiable<ListVariableListener<object, object>>
    {
        public ListVariableListenerNotifiable(ScoreDirector scoreDirector, ListVariableListener<Object, Object> variableListener,
            List<Notification<ListVariableListener<Object, Object>>> notificationQueue, int globalOrder)
            : base(scoreDirector, variableListener, notificationQueue, globalOrder)
        {
        }
    }
}
