using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.API.Domain.Variable
{
    public interface AbstractVariableListener<Entity_> : IDisposable
    {
        void ResetWorkingSolution(ScoreDirector scoreDirector);
    }
}
