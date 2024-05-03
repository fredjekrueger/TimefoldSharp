using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Move;

namespace TimefoldSharp.Core.Impl.Score.Director
{
    public interface InnerScoreDirector : ScoreDirector, IDisposable
    {
        SolutionDescriptor GetSolutionDescriptor();
        API.Score.Score CalculateScore();
        void ResetCalculationCount();
        void SetWorkingSolution(ISolution workingSolution);
        ISolution CloneSolution(ISolution originalSolution);
        ISolution CloneWorkingSolution();
        InnerScoreDirectorFactory GetScoreDirectorFactory();
        void SetAllChangesWillBeUndoneBeforeStepEnds(bool allChangesWillBeUndoneBeforeStepEnds);
        long GetCalculationCount();
        void AssertPredictedScoreFromScratch(API.Score.Score predictedScore, object completedAction);
        void AssertExpectedWorkingScore(API.Score.Score expectedWorkingScore, object completedAction);
        void AssertShadowVariablesAreNotStale(API.Score.Score expectedWorkingScore, object completedAction);
        void DoAndProcessMove(Move move, bool assertMoveScoreFromScratch, Action<API.Score.Score> moveProcessor);
        API.Score.Score DoAndProcessMove(Move move, bool assertMoveScoreFromScratch);
        void AssertExpectedUndoMoveScore(Move move, API.Score.Score beforeMoveScore);
        long GetWorkingEntityListRevision();
        bool IsWorkingEntityListDirty(long expectedWorkingEntityListRevision);
        void ChangeVariableFacade(VariableDescriptor variableDescriptor, object entity, object newValue);
        void BeforeVariableChanged(VariableDescriptor variableDescriptor, object entity);
        void AfterVariableChanged(VariableDescriptor variableDescriptor, object entity);
        void AssertWorkingScoreFromScratch(API.Score.Score workingScore, Object completedAction);
    }
}
