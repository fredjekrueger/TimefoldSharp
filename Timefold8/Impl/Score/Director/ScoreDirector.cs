using TimefoldSharp.Core.API.Score;

namespace TimefoldSharp.Core.Impl.Score.Director
{
    public interface ScoreDirector
    {
        ISolution GetWorkingSolution();
        void TriggerVariableListeners();
    }

    public enum ScoreDirectorType
    {

        EASY,
        CONSTRAINT_STREAMS,
        INCREMENTAL

    }

}
