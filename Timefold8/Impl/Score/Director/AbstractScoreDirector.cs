using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Lookup;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Listener.Support;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;
using TimefoldSharp.Core.Impl.Heurisitic.Move;

namespace TimefoldSharp.Core.Impl.Score.Director
{
    public abstract class AbstractScoreDirector<Factory_> : InnerScoreDirector, ICloneable

            where Factory_ : AbstractScoreDirectorFactory
    {

        protected readonly Factory_ scoreDirectorFactory;
        protected readonly bool lookUpEnabled;
        protected readonly LookUpManager lookUpManager;
        protected readonly bool expectShadowVariablesInCorrectState;
        protected readonly VariableListenerSupport variableListenerSupport;
        protected readonly bool constraintMatchEnabledPreference;
        protected long calculationCount = 0;
        protected ISolution workingSolution;
        protected int? workingInitScore = null;
        protected long workingEntityListRevision = 0L;
        protected bool allChangesWillBeUndoneBeforeStepEnds = false;

        protected AbstractScoreDirector(Factory_ scoreDirectorFactory, bool lookUpEnabled, bool constraintMatchEnabledPreference, bool expectShadowVariablesInCorrectState)
        {
            this.scoreDirectorFactory = scoreDirectorFactory;
            this.lookUpEnabled = lookUpEnabled;
            this.lookUpManager = lookUpEnabled
                    ? new LookUpManager(scoreDirectorFactory.GetSolutionDescriptor().GetLookUpStrategyResolver())
                    : null;
            this.expectShadowVariablesInCorrectState = expectShadowVariablesInCorrectState;
            this.variableListenerSupport = VariableListenerSupport.Create((InnerScoreDirector)this);
            this.variableListenerSupport.LinkVariableListeners();
            this.constraintMatchEnabledPreference = constraintMatchEnabledPreference;
        }



        protected void SetCalculatedScore(API.Score.Score score)
        {
            GetSolutionDescriptor().SetScore(workingSolution, score);
            calculationCount++;
        }

        public ISolution GetWorkingSolution()
        {
            return workingSolution;
        }

        public virtual void SetWorkingSolution(ISolution workingSolution)
        {
            this.workingSolution = workingSolution;
            SolutionDescriptor solutionDescriptor = GetSolutionDescriptor();
            workingInitScore = -solutionDescriptor.CountUninitialized(workingSolution);
            if (lookUpEnabled)
            {
                lookUpManager.Reset();
                solutionDescriptor.VisitAll(workingSolution, c =>
                {
                    lookUpManager.AddWorkingObject(c);
                });
            }

            variableListenerSupport.ResetWorkingSolution();
            SetWorkingEntityListDirty();
        }

        protected void SetWorkingEntityListDirty()
        {
            workingEntityListRevision++;
        }

        public ISolution CloneSolution(ISolution originalSolution)
        {
            SolutionDescriptor solutionDescriptor = GetSolutionDescriptor();
            API.Score.Score originalScore = (API.Score.Score)solutionDescriptor.GetScore(originalSolution);
            ISolution cloneSolution = solutionDescriptor.GetSolutionCloner().CloneSolution(originalSolution);
            API.Score.Score cloneScore = (API.Score.Score)solutionDescriptor.GetScore(cloneSolution);
            if (scoreDirectorFactory.IsAssertClonedSolution())
            {
                if (!Object.Equals(originalScore, cloneScore))
                {
                    throw new Exception("Cloning corruption: "
                            + "the original's score (" + originalScore
                            + ") is different from the clone's score (" + cloneScore + ").\n"
                            + "Check the.");
                }
                Dictionary<Object, Object> originalEntityMap = new Dictionary<object, object>();
                solutionDescriptor.VisitAllEntities(originalSolution, originalEntity => originalEntityMap.Add(originalEntity, null));
            }
            return cloneSolution;
        }

        public ISolution CloneWorkingSolution()
        {
            return CloneSolution(workingSolution);
        }

        public void ResetCalculationCount()
        {
            this.calculationCount = 0;
        }

        public abstract API.Score.Score CalculateScore();

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            workingSolution = null;
            workingInitScore = null;
            if (lookUpEnabled)
            {
                lookUpManager.Reset();
            }
            variableListenerSupport.Close();
        }

        public SolutionDescriptor GetSolutionDescriptor()
        {
            return scoreDirectorFactory.GetSolutionDescriptor();
        }

        public long GetCalculationCount()
        {
            return calculationCount;
        }

        public void AssertPredictedScoreFromScratch(API.Score.Score predictedScore, object completedAction)
        {
            throw new NotImplementedException();
        }

        public void AssertExpectedWorkingScore(API.Score.Score expectedWorkingScore, object completedAction)
        {
            throw new NotImplementedException();
        }

        public void AssertShadowVariablesAreNotStale(API.Score.Score expectedWorkingScore, object completedAction)
        {
            throw new NotImplementedException();
        }

        public void AssertExpectedUndoMoveScore(Move move, API.Score.Score beforeMoveScore)
        {
            throw new NotImplementedException();
        }

        public long GetWorkingEntityListRevision()
        {
            return workingEntityListRevision;
        }

        public bool IsWorkingEntityListDirty(long expectedWorkingEntityListRevision)
        {
            return workingEntityListRevision != expectedWorkingEntityListRevision;
        }

        public void DoAndProcessMove(Move move, bool assertMoveScoreFromScratch, Action<API.Score.Score> moveProcessor)
        {
            Move undoMove = move.DoMove(this);
            API.Score.Score score = CalculateScore();

            moveProcessor.Invoke(score);
            undoMove.DoMoveOnly(this);
        }

        public API.Score.Score DoAndProcessMove(Move move, bool assertMoveScoreFromScratch)
        {
            throw new NotImplementedException();
        }

        public void TriggerVariableListeners()
        {
            variableListenerSupport.TriggerVariableListenersInNotificationQueues();
        }

        public void ChangeVariableFacade(VariableDescriptor variableDescriptor, object entity, object newValue)
        {
            BeforeVariableChanged(variableDescriptor, entity);
            variableDescriptor.SetValue(entity, newValue);
            AfterVariableChanged(variableDescriptor, entity);
        }

        public void BeforeVariableChanged(VariableDescriptor variableDescriptor, object entity)
        {
            if (variableDescriptor.IsGenuineAndUninitialized(entity))
            {
                workingInitScore++;
            }
            variableListenerSupport.BeforeVariableChanged(variableDescriptor, entity);
        }

        public virtual void AfterVariableChanged(VariableDescriptor variableDescriptor, object entity)
        {
            if (variableDescriptor.IsGenuineAndUninitialized(entity))
            {
                workingInitScore--;
            }
        }

        public void AssertWorkingScoreFromScratch(API.Score.Score workingScore, object completedAction)
        {
            throw new NotImplementedException();
        }

        public InnerScoreDirectorFactory GetScoreDirectorFactory()
        {
            return scoreDirectorFactory;
        }

        public void SetAllChangesWillBeUndoneBeforeStepEnds(bool allChangesWillBeUndoneBeforeStepEnds)
        {
            this.allChangesWillBeUndoneBeforeStepEnds = allChangesWillBeUndoneBeforeStepEnds;
        }

        public SupplyManager GetSupplyManager()
        {
            return variableListenerSupport;
        }
    }
}
