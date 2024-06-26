using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Domain.Variable.Listener.Support;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support
{
    public class VariableChangedNotification : AbstractNotification, BasicVariableNotification
    {
        public VariableChangedNotification(object entity) : base(entity)
        {
        }

        public void TriggerAfter(VariableListener<object> variableListener, ScoreDirector scoreDirector)
        {
            variableListener.AfterVariableChanged(scoreDirector, entity);
        }

        public void TriggerBefore(VariableListener<object> variableListener, ScoreDirector scoreDirector)
        {
            variableListener.BeforeVariableChanged(scoreDirector, entity);
        }

        public override string ToString()
        {
            return "VariableChanged(" + entity + ")";
        }
    }
}
