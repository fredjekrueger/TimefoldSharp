using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.API.Domain.Variable
{
    public interface VariableListener<Entity_> : AbstractVariableListener<Entity_>
    {
        bool RequiresUniqueEntityEvents();
        void BeforeVariableChanged(ScoreDirector scoreDirector, Entity_ entity);
        void AfterVariableChanged(ScoreDirector scoreDirector, Entity_ entity);
    }
}
